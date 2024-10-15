using System;
using System.Runtime.CompilerServices;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class RewardBasedVideoAdClient : AndroidJavaProxy, IRewardBasedVideoAdClient
	{
		private AndroidJavaObject androidRewardBasedVideo;

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdLoaded = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdOpening = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdStarted = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdClosed = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<Reward> OnAdRewarded = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdLeavingApplication = delegate
		{
		};

		public RewardBasedVideoAdClient()
			: base("com.google.unity.ads.UnityRewardBasedVideoAdListener")
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			androidRewardBasedVideo = new AndroidJavaObject("com.google.unity.ads.RewardBasedVideo", @static, this);
		}

		public void CreateRewardBasedVideoAd()
		{
			androidRewardBasedVideo.Call("create");
		}

		public void LoadAd(AdRequest request, string adUnitId)
		{
			androidRewardBasedVideo.Call("loadAd", Utils.GetAdRequestJavaObject(request), adUnitId);
		}

		public bool IsLoaded()
		{
			return androidRewardBasedVideo.Call<bool>("isLoaded", new object[0]);
		}

		public void ShowRewardBasedVideoAd()
		{
			androidRewardBasedVideo.Call("show");
		}

		public void DestroyRewardBasedVideoAd()
		{
			androidRewardBasedVideo.Call("destroy");
		}

		private void onAdLoaded()
		{
			this.OnAdLoaded(this, EventArgs.Empty);
		}

		private void onAdFailedToLoad(string errorReason)
		{
			AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
			adFailedToLoadEventArgs.Message = errorReason;
			AdFailedToLoadEventArgs e = adFailedToLoadEventArgs;
			this.OnAdFailedToLoad(this, e);
		}

		private void onAdOpened()
		{
			this.OnAdOpening(this, EventArgs.Empty);
		}

		private void onAdStarted()
		{
			this.OnAdStarted(this, EventArgs.Empty);
		}

		private void onAdClosed()
		{
			this.OnAdClosed(this, EventArgs.Empty);
		}

		private void onAdRewarded(string type, float amount)
		{
			Reward reward = new Reward();
			reward.Type = type;
			reward.Amount = amount;
			Reward e = reward;
			this.OnAdRewarded(this, e);
		}

		private void onAdLeftApplication()
		{
			this.OnAdLeavingApplication(this, EventArgs.Empty);
		}
	}
}
