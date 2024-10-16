using System;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	internal interface IAdLoaderClient
	{
		event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

		void LoadAd(AdRequest request);
	}
}
