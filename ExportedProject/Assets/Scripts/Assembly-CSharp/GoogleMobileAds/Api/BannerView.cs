using System;
using System.Runtime.CompilerServices;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class BannerView
	{
		private IBannerClient client;

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
		public event EventHandler<EventArgs> OnAdClosed = delegate
		{
		};

		[method: MethodImpl(32)]
		public event EventHandler<EventArgs> OnAdLeavingApplication = delegate
		{
		};

		public BannerView(string adUnitId, AdSize adSize, AdPosition position)
		{
			client = GoogleMobileAdsClientFactory.BuildBannerClient();
			client.CreateBannerView(adUnitId, adSize, position);
			client.OnAdLoaded += delegate(object sender, EventArgs args)
			{
				this.OnAdLoaded(this, args);
			};
			client.OnAdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs args)
			{
				this.OnAdFailedToLoad(this, args);
			};
			client.OnAdOpening += delegate(object sender, EventArgs args)
			{
				this.OnAdOpening(this, args);
			};
			client.OnAdClosed += delegate(object sender, EventArgs args)
			{
				this.OnAdClosed(this, args);
			};
			client.OnAdLeavingApplication += delegate(object sender, EventArgs args)
			{
				this.OnAdLeavingApplication(this, args);
			};
		}

		public void LoadAd(AdRequest request)
		{
			client.LoadAd(request);
		}

		public void Hide()
		{
			client.HideBannerView();
		}

		public void Show()
		{
			client.ShowBannerView();
		}

		public void Destroy()
		{
			client.DestroyBannerView();
		}
	}
}
