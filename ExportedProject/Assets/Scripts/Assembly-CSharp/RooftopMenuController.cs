using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RooftopMenuController : MonoBehaviour
{
	public GameObject tgLevel;

	public GameObject tgFighterOrder;

	private List<Toggle> listLevel;

	private List<Toggle> listFighterOrder;

	private void Start()
	{
		listLevel = new List<Toggle>(5);
		listFighterOrder = new List<Toggle>(5);
		SetToggles("str_level", listLevel, tgLevel);
		SetToggles("str_fighterOrder", listFighterOrder, tgFighterOrder);
		Text[] array = Object.FindObjectsOfType<Text>();
		Text[] array2 = array;
		foreach (Text text in array2)
		{
			text.text = Localizer.LocalizedString(text.text);
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
		SaveToggles("str_fighterOrder", listFighterOrder, tgFighterOrder);
		PlayerPrefs.SetString("str_player", "0");
		PlayerPrefs.Save();
	}

	public void DoStart()
	{
		DoSave();
		GameInfo.round = 1;
		GameInfo.score = 0f;
		SoundController.instance.PlaySoundStart();
		SceneController.instance.LoadScene("RooftopField");
	}
}
