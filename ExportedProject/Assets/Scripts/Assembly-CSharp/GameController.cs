using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour
{
	private float timeLeft = 90f;

	private int maxRound = 5;

	public bool isPlaying;

	public GameObject fighterPlayer;

	public GameObject fighterCPU;

	private RoboVoice roboVoice;

	public static GameController instance;

	private void Awake()
	{
		instance = this;
		if (Application.productName.StartsWith("Rooftop"))
		{
			GameInfo.appName = GameInfo.AppName.RooftopFighter;
		}
		else
		{
			GameInfo.appName = GameInfo.AppName.SchoolFighter;
		}
		if (SceneManager.GetActiveScene().name.Equals("Field"))
		{
			GameInfo.mode = GameInfo.Mode.Normal;
		}
		else
		{
			GameInfo.mode = GameInfo.Mode.Simple;
		}
	}

	private void Start()
	{
		roboVoice = Object.FindObjectOfType<RoboVoice>();
		Joystick joystick = Object.FindObjectOfType<Joystick>();
		if ((bool)joystick)
		{
			joystick.enabled = false;
			joystick.enabled = true;
		}
		ViewDidLoad();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			CanvasController.instance.DoMenu();
		}
		if (Input.GetKey(KeyCode.R))
		{
			CanvasController.instance.DoRestart();
		}
		AddScore(Time.deltaTime * 20f);
		ReduceTime();
	}

	public void AddScore(float amount)
	{
		if (isPlaying)
		{
			GameInfo.score += amount;
			CanvasController.instance.ShowScore(GameInfo.score);
		}
	}

	private void ReduceTime()
	{
		if (isPlaying)
		{
			timeLeft -= Time.deltaTime;
			timeLeft = Mathf.Clamp(timeLeft, 0f, timeLeft);
			CanvasController.instance.ShowTimeLeft(timeLeft);
			if (timeLeft <= 0f)
			{
				StartCoroutine(TimeUp());
			}
		}
	}

	private IEnumerator TimeUp()
	{
		isPlaying = false;
		CanvasController.instance.DisableButtonCamera();
		CanvasController.instance.ShowMessage(Localizer.LocalizedString("Times Up"));
		roboVoice.SpeakVoice(RoboVoice.Voice.TimesUp);
		yield return new WaitForSeconds(2f);
		float playerHealthScore = fighterPlayer.GetComponent<StudentHealth>().GetHealthScore();
		float cpuHealthScore = fighterCPU.GetComponent<StudentHealth>().GetHealthScore();
		if (playerHealthScore == cpuHealthScore)
		{
			DoDraw();
		}
		else if (playerHealthScore > cpuHealthScore)
		{
			StartCoroutine("DoWin");
		}
		else
		{
			DoLose();
		}
	}

	public void GameOver()
	{
		isPlaying = false;
		CanvasController.instance.DisableButtonCamera();
		StopCoroutine("DoWin");
		if (fighterCPU.GetComponent<Student>().isDied && fighterPlayer.GetComponent<Student>().isDied)
		{
			DoDraw();
		}
		else if (fighterCPU.GetComponent<Student>().isDied)
		{
			StartCoroutine("DoWin");
		}
		else
		{
			DoLose();
		}
	}

	private IEnumerator DoWin()
	{
		GetComponent<FinishAnim>().StartAnim(true);
		BGMController.instance.PlayBGM(BGMController.Clip.Win);
		if (GameInfo.round < maxRound)
		{
			string msg = Localizer.LocalizedString("CLEAR!");
			roboVoice.SpeakVoice(RoboVoice.Voice.Clear);
			CanvasController.instance.ShowMessage(msg);
			GameInfo.round++;
			yield return new WaitForSeconds(7.1f);
			SoundController.instance.PlaySoundStart();
			SceneController.instance.Reload();
		}
		else
		{
			roboVoice.SpeakVoice(RoboVoice.Voice.Victory);
			ShowPanelFinish("Congratulation!");
		}
	}

	private void DoDraw()
	{
		GetComponent<FinishAnim>().StartAnim(true);
		BGMController.instance.PlayBGM(BGMController.Clip.Win);
		roboVoice.SpeakVoice(RoboVoice.Voice.Draw);
		ShowPanelFinish("DRAW");
	}

	private void DoLose()
	{
		GetComponent<FinishAnim>().StartAnim(false);
		BGMController.instance.PlayBGM(BGMController.Clip.Lose);
		string empty = string.Empty;
		if (fighterPlayer.GetComponent<Student>().isDied)
		{
			empty += "K.O.";
			roboVoice.SpeakVoice(RoboVoice.Voice.KO);
		}
		else
		{
			empty += "Failed.";
			roboVoice.SpeakVoice(RoboVoice.Voice.Failed);
		}
		ShowPanelFinish(empty);
	}

	private void ShowPanelFinish(string msg)
	{
		CanvasController.instance.ShowPanelFinish(GameInfo.score, msg);
	}

	private IEnumerator ShowRoundInfo()
	{
		isPlaying = false;
		BGMController.instance.PlayBGM(BGMController.Clip.Play);
		yield return new WaitForEndOfFrame();
		if (GameInfo.appName == GameInfo.AppName.SchoolFighter)
		{
			fighterCPU.GetComponent<Student>().DoBow();
			fighterPlayer.GetComponent<Student>().DoBow();
		}
		yield return new WaitForSeconds(2f);
		string str = string.Format("{0} {1}", Localizer.LocalizedString("ROUND"), GameInfo.round);
		CanvasController.instance.ShowMessage(str);
		roboVoice.SpeakVoice(RoboVoice.Voice.Round);
		yield return new WaitForSeconds(1f);
		roboVoice.SpeakNumber(GameInfo.round);
		yield return new WaitForSeconds(1f);
		CanvasController.instance.ShowMessage(string.Empty);
		yield return new WaitForSeconds(0.5f);
		CanvasController.instance.ShowMessage(Localizer.LocalizedString("FIGHT!"));
		roboVoice.SpeakVoice(RoboVoice.Voice.Fight);
		isPlaying = true;
		if (GameInfo.appName == GameInfo.AppName.SchoolFighter)
		{
			fighterCPU.GetComponent<Student>().DoGreetDone();
			fighterPlayer.GetComponent<Student>().DoGreetDone();
		}
		yield return new WaitForSeconds(0.5f);
		CanvasController.instance.HideMessage();
	}

	private void ViewDidLoad()
	{
		CanvasController.instance.ShowScore(GameInfo.score);
		CanvasController.instance.ShowTimeLeft(timeLeft);
		CanvasController.instance.ShowRound(GameInfo.round, maxRound);
		StartCoroutine(ShowRoundInfo());
	}
}
