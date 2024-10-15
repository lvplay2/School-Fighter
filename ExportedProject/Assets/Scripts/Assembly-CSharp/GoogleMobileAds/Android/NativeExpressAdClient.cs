using System;
using System.Runtime.CompilerServices;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class NativeExpressAdClient : AndroidJavaProxy, INativeExpressAdClient
	{
		private AndroidJavaObject nativeExpressAdView;

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdLoaded;

		[method: MethodImpl(32)]
		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdOpening;

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdClosed;

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdLeavingApplication;

		public NativeExpressAdClient()
			: base("com.google.unity.ads.UnityAdListener")
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			nativeExpressAdView = new AndroidJavaObject("com.google.unity.ads.NativeExpressAd", @static, this);
		}

		public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
		{
			nativeExpressAdView.Call("create", adUnitId, Utils.GetAdSizeJavaObject(adSize), (int)position);
		}

		public void LoadAd(AdRequest request)
		{
			nativeExpressAdView.Call("loadAd", Utils.GetAdRequestJavaObject(request));
		}

		public void SetAdSize(AdSize adSize)
		{
			nativeExpressAdView.Call("setAdSize", Utils.GetAdSizeJavaObject(adSize));
		}

		public void ShowNativeExpressAdView()
		{
			nativeExpressAdView.Call("show");
		}

		public void HideNativeExpressAdView()
		{
			nativeExpressAdView.Call("hide");
		}

		public void DestroyNativeExpressAdView()
		{
			nativeExpressAdView.Call("destroy");
		}

		public void onAdLoaded()
		{
			this.OnAdLoaded(this, EventArgs.Empty);
		}

		public void onAdFailedToLoad(string errorReason)
		{
			AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
			adFailedToLoadEventArgs.Message = errorReason;
			AdFailedToLoadEventArgs e = adFailedToLoadEventArgs;
			this.OnAdFailedToLoad(this, e);
		}

		public void onAdOpened()
		{
			this.OnAdOpening(this, EventArgs.Empty);
		}

		public void onAdClosed()
		{
			this.OnAdClosed(this, EventArgs.Empty);
		}

		public void onAdLeftApplication()
		{
			this.OnAdLeavingApplication(this, EventArgs.Empty);
		}
	}
}
