using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class RoboLoading : MonoBehaviour
{
	public bool isTestMode;

	private float elappsedTimeFromBeginning;

	private float elappsedTimeAfterAdLoaded;

	private bool lastSplashScreenStatus;

	private bool isDisappearedSlpashScreen;

	private bool isAdLoaded;

	private bool isAdShown;

	private InterstitialAd interstitial;

	private AdRequest adRequest;

	private AlertViewController avc;

	private void Start()
	{
		Text[] array = UnityEngine.Object.FindObjectsOfType<Text>();
		Text[] array2 = array;
		foreach (Text text in array2)
		{
			text.text = Localizer.LocalizedString(text.text);
		}
		Handheld.StartActivityIndicator();
		avc = UnityEngine.Object.FindObjectOfType<AlertViewController>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		elappsedTimeFromBeginning += Time.deltaTime;
		if (!isAdShown && elappsedTimeFromBeginning > 12f)
		{
			StartCoroutine(DoMenu());
		}
		LoadingAndroid();
	}

	private void LoadingAndroid()
	{
		if (!isAdLoaded)
		{
			return;
		}
		elappsedTimeAfterAdLoaded += Time.deltaTime;
		if (!isAdShown)
		{
			avc.sliderLoading.value = elappsedTimeAfterAdLoaded;
			if (elappsedTimeAfterAdLoaded > 2f)
			{
				ShowInterstitial();
			}
		}
	}

	private void LoadingIOS()
	{
		if (lastSplashScreenStatus && !Application.isShowingSplashScreen)
		{
			isDisappearedSlpashScreen = true;
		}
		lastSplashScreenStatus = Application.isShowingSplashScreen;
		if (!isAdLoaded || !isDisappearedSlpashScreen)
		{
			return;
		}
		elappsedTimeAfterAdLoaded += Time.deltaTime;
		if (!isAdShown)
		{
			avc.sliderLoading.value = elappsedTimeAfterAdLoaded;
			if (elappsedTimeAfterAdLoaded > 2f)
			{
				ShowInterstitial();
			}
		}
	}

	private void InitAdRequest()
	{
		string deviceId = "03c99f8753e690809305e2e0b9570f9b";
		string deviceId2 = "3a1c429a7412243fb479289100d309c5";
		string deviceId3 = "DDCF01BC14038B71AA41973C66C0832B";
		string deviceId4 = "1A945FD4CFB8950A6C77AF5CA3E8169D";
		if (isTestMode)
		{
			adRequest = new AdRequest.Builder().AddTestDevice(deviceId).AddTestDevice(deviceId2).AddTestDevice(deviceId3)
				.AddTestDevice(deviceId4)
				.Build();
		}
		else
		{
			adRequest = new AdRequest.Builder().Build();
		}
	}

	private void InterstitialLoaded(object sender, EventArgs e)
	{
		isAdLoaded = true;
	}

	public void ShowInterstitial()
	{
		isAdShown = true;
		avc.gameObject.SetActive(false);
		AudioListener.volume = 0f;
	}

	private void InterstitialClosed(object sender, EventArgs e)
	{
		StartCoroutine(DoMenu());
	}

	private void InterstitialFailedToLoad(object sender, EventArgs e)
	{
		StartCoroutine(DoMenu());
	}

	private IEnumerator DoMenu()
	{
		yield return new WaitForSeconds(0.1f);
		if (interstitial != null)
		{
			interstitial.Destroy();
		}
		Handheld.StartActivityIndicator();
		if (Application.productName.StartsWith("Rooftop"))
		{
			SceneController.instance.LoadScene("RooftopMain", 0f);
		}
		else
		{
			SceneController.instance.LoadScene("Main", 0f);
		}
	}
}
