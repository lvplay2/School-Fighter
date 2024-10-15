using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class MenuController : MonoBehaviour
{
	public GameObject tgLevel;

	public GameObject tgPlayer;

	public GameObject tgMode;

	public LookatTarget lookatTarget;

	public GameObject[] playerSet;

	private List<Toggle> listLevel;

	private List<Toggle> listPlayer;

	private List<Toggle> listMode;

	private string[] studentNames = new string[3] { "Taichi", "Aoi", "Honoka" };

	private void Start()
	{
		listLevel = new List<Toggle>(5);
		listPlayer = new List<Toggle>(5);
		listMode = new List<Toggle>(5);
		SetToggles("str_level", listLevel, tgLevel);
		SetToggles("str_player", listPlayer, tgPlayer);
		SetToggles("str_mode", listMode, tgMode);
		PlayerValueChanged();
		Text[] array = Object.FindObjectsOfType<Text>();
		Text[] array2 = array;
		foreach (Text text in array2)
		{
			text.text = Localizer.LocalizedString(text.text);
		}
		Vector3 position = lookatTarget.transform.position;
		position.y = 0f;
		for (int j = 0; j < playerSet.Length; j++)
		{
			playerSet[j].transform.LookAt(position);
			playerSet[j].GetComponent<Animator>().SetTrigger(studentNames[j]);
		}
		BGMController.instance.PlayBGM(BGMController.Clip.Menu);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void SetToggles(string keyString, List<Toggle> list, GameObject tg)
	{
		int index = int.Parse((!PlayerPrefs.HasKey(keyString)) ? "0" : PlayerPrefs.GetString(keyString));
		list.AddRange(tg.transform.GetComponentsInChildren<Toggle>());
		list[index].isOn = true;
	}

	private void SaveToggles(string keyString, List<Toggle> list, GameObject tg)
	{
		List<Toggle> list2 = new List<Toggle>(5);
		list2.AddRange(tg.GetComponent<ToggleGroup>().ActiveToggles());
		int num = list.IndexOf(list2[0]);
		PlayerPrefs.SetString(keyString, string.Format("{0}", num));
	}

	public void DoSave()
	{
		SaveToggles("str_level", listLevel, tgLevel);
		SaveToggles("str_player", listPlayer, tgPlayer);
		SaveToggles("str_mode", listMode, tgMode);
		PlayerPrefs.Save();
	}

	public void DoStart()
	{
		DoSave();
		SoundController.instance.PlaySoundStart();
		GameInfo.round = 1;
		GameInfo.score = 0f;
		if (int.Parse((!PlayerPrefs.HasKey("str_mode")) ? "0" : PlayerPrefs.GetString("str_mode")) == 0)
		{
			GameInfo.mode = GameInfo.Mode.Normal;
			SceneController.instance.LoadScene("Field");
		}
		else
		{
			GameInfo.mode = GameInfo.Mode.Simple;
			SceneController.instance.LoadScene("FieldSimple");
		}
	}

	public void PlayerValueChanged()
	{
		List<Toggle> list = new List<Toggle>(5);
		list.AddRange(tgPlayer.GetComponent<ToggleGroup>().ActiveToggles());
		int num = listPlayer.IndexOf(list[0]);
		lookatTarget.SetTarget(playerSet[num].transform);
		Animator component = playerSet[num].GetComponent<Animator>();
		component.SetInteger("GreetID", Random.Range(0, 4));
		component.SetTrigger(studentNames[num]);
	}
}
