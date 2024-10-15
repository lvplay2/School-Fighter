using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
	public Image iconPlayer;

	public Slider sliderPlayer;

	public Image iconCPU;

	public Slider sliderCPU;

	public Text textScore;

	public Text textTimeLeft;

	public Text textRound;

	public GameObject panelMessage;

	public Text textMessage;

	public GameObject panelFinish;

	public Text textFinishMessage;

	public Text textFinishScore;

	public GameObject btnCamera;

	public Sprite[] iconSet;

	public Sprite[] rooftopIconSet;

	public static CanvasController instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (GameInfo.mode == GameInfo.Mode.Simple)
		{
			btnCamera.SetActive(false);
		}
		Text[] array = UnityEngine.Object.FindObjectsOfType<Text>();
		Text[] array2 = array;
		foreach (Text text in array2)
		{
			text.text = Localizer.LocalizedString(text.text);
		}
		GameObject gameObject = GameObject.Find("RoboAdBanner");
		if ((bool)gameObject)
		{
			gameObject.SendMessage("Hide");
		}
		HideMessage();
	}

	private void Update()
	{
	}

	public void ShowIcon(bool isPlayer, int id)
	{
		if (GameInfo.appName == GameInfo.AppName.SchoolFighter)
		{
			if (isPlayer)
			{
				iconPlayer.sprite = iconSet[id];
			}
			else
			{
				iconCPU.sprite = iconSet[id];
			}
		}
		else if (isPlayer)
		{
			iconPlayer.sprite = rooftopIconSet[id];
		}
		else
		{
			iconCPU.sprite = rooftopIconSet[id];
		}
	}

	public void ShowSliderHealth(bool isPlayer, float value)
	{
		if (isPlayer)
		{
			sliderPlayer.value = value;
		}
		else
		{
			sliderCPU.value = value;
		}
	}

	public void ChangeSliderHealthColor(bool isPlayer)
	{
		if (isPlayer)
		{
			sliderPlayer.transform.Find("Fill Area").Find("Fill").GetComponent<Image>()
				.color = Color.yellow;
		}
		else
		{
			sliderCPU.transform.Find("Fill Area").Find("Fill").GetComponent<Image>()
				.color = Color.yellow;
		}
	}

	public void ShowMessage(string str)
	{
		textMessage.text = str;
		panelMessage.SetActive(true);
	}

	public void HideMessage()
	{
		textMessage.text = string.Empty;
		panelMessage.SetActive(false);
	}

	public void ShowScore(float score)
	{
		textScore.text = string.Format("{0} {1:00000000}", Localizer.LocalizedString("SCORE"), score);
	}

	public void ShowRound(int round, int maxRound)
	{
		textRound.text = string.Format("{0} {1}/{2}", Localizer.LocalizedString("ROUND"), round, maxRound);
	}

	public void ShowTimeLeft(float timeLeft)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
		string text = string.Format("{0} {1:D1}'{2:D2}.{3:D1}", Localizer.LocalizedString("Time Left"), timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 100);
		textTimeLeft.text = text;
	}

	public void ShowPanelFinish(float score, string message)
	{
		HideMessage();
		textFinishMessage.text = Localizer.LocalizedString(message);
		string arg = Localizer.LocalizedString("SCORE");
		textFinishScore.text = string.Format("{0} {1:00000000}", arg, score);
		panelFinish.SetActive(true);
		GameObject gameObject = GameObject.Find("CanvasSimpleButton");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		Text[] array = UnityEngine.Object.FindObjectsOfType<Text>();
		Text[] array2 = array;
		foreach (Text text in array2)
		{
			text.text = Localizer.LocalizedString(text.text);
		}
		GameObject gameObject2 = GameObject.Find("RoboAdBanner");
		if ((bool)gameObject2)
		{
			gameObject2.SendMessage("Show");
		}
		GameObject gameObject3 = GameObject.Find("RoboInterstitial");
		if ((bool)gameObject3)
		{
			gameObject3.SendMessage("ShowLoadingSlider", GetComponentInChildren<AlertViewController>());
		}
	}

	public void DoMenu()
	{
		SoundController.instance.PlaySoundUI1();
		if (GameInfo.appName == GameInfo.AppName.RooftopFighter)
		{
			SceneController.instance.LoadScene("RooftopMain");
		}
		else
		{
			SceneController.instance.LoadScene("Main");
		}
	}

	public void DoRestart()
	{
		SoundController.instance.PlaySoundStart();
		SceneController.instance.Reload();
		GameInfo.round = 1;
		GameInfo.score = 0f;
	}

	public void DoChangeCamera()
	{
		CameraController cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		if ((bool)cameraController)
		{
			SoundController.instance.PlaySoundUI1();
			cameraController.DoChangeCamera();
		}
	}

	public void DisableButtonCamera()
	{
		btnCamera.GetComponent<Button>().interactable = false;
	}
}
