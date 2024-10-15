using System;
using System.Runtime.CompilerServices;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class RewardBasedVideoAd
	{
		private IRewardBasedVideoAdClient client;

		private static RewardBasedVideoAd instance;

		public static RewardBasedVideoAd Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new RewardBasedVideoAd();
				}
				return instance;
			}
		}

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

		private RewardBasedVideoAd()
		{
			client = GoogleMobileAdsClientFactory.BuildRewardBasedVideoAdClient();
			client.CreateRewardBasedVideoAd();
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
			client.OnAdStarted += delegate(object sender, EventArgs args)
			{
				this.OnAdStarted(this, args);
			};
			client.OnAdRewarded += delegate(object sender, Reward args)
			{
				this.OnAdRewarded(this, args);
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

		public void LoadAd(AdRequest request, string adUnitId)
		{
			client.LoadAd(request, adUnitId);
		}

		public bool IsLoaded()
		{
			return client.IsLoaded();
		}

		public void Show()
		{
			client.ShowRewardBasedVideoAd();
		}
	}
}
