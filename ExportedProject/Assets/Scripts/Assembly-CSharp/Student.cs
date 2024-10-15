using System.Collections;
using UnityEngine;

public class Student : MonoBehaviour
{
	public int punchMax;

	public int kickMax;

	public int damageMax;

	public int downMax;

	public int greetMax;

	public int specialMax;

	public bool isDown;

	public bool isDied;

	public bool isAttack;

	public AudioClip seDamage;

	private Animator anim;

	private AudioSource audioSource;

	private StudentHealth health;

	private CanvasController canvasController;

	private GameController gameController;

	private AnimTag animTag;

	private void Start()
	{
		anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		health = GetComponent<StudentHealth>();
		canvasController = Object.FindObjectOfType<CanvasController>();
		gameController = Object.FindObjectOfType<GameController>();
		animTag = GetComponent<AnimTag>();
		ChangeClothes(ChangeCharacter());
		SetUpStudent();
	}

	private void Update()
	{
	}

	private int ChangeCharacter()
	{
		int num = int.Parse((!PlayerPrefs.HasKey("str_player")) ? "0" : PlayerPrefs.GetString("str_player"));
		int num2 = num;
		int childCount = base.transform.childCount;
		int min = 1;
		if (!CompareTag("Player"))
		{
			if (GameInfo.appName == GameInfo.AppName.RooftopFighter)
			{
				if (int.Parse((!PlayerPrefs.HasKey("str_fighterOrder")) ? "0" : PlayerPrefs.GetString("str_fighterOrder")) == 0)
				{
					num2 = GameInfo.round;
					if (num2 >= childCount)
					{
						num2 = GameInfo.round % base.transform.childCount + 1;
					}
				}
				else
				{
					num2 = Random.Range(min, childCount);
				}
			}
			else
			{
				while (num2 == num)
				{
					num2 = Random.Range(0, 3);
				}
			}
		}
		if (num2 == 0)
		{
			GetComponent<StudentLeg>().walkSpeed = 0f;
		}
		canvasController.ShowIcon(CompareTag("Player"), num2);
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (i != num2)
			{
				Object.Destroy(base.transform.GetChild(i).gameObject);
			}
		}
		return num2;
	}

	private void ChangeClothes(int targetID)
	{
		Transform child = base.transform.GetChild(targetID);
		int childCount = child.childCount;
		int num = Random.Range(0, childCount);
		for (int i = 0; i < childCount; i++)
		{
			if (i == num)
			{
				child.GetChild(i).gameObject.SetActive(true);
				Animator component = child.GetChild(i).GetComponent<Animator>();
				anim.avatar = component.avatar;
				anim.runtimeAnimatorController = component.runtimeAnimatorController;
				Object.Destroy(component);
			}
			else
			{
				Object.Destroy(child.GetChild(i).gameObject);
			}
		}
	}

	private void SetUpStudent()
	{
		if (CompareTag("Player") || base.gameObject.name.Equals("Player"))
		{
			base.gameObject.tag = "Player";
			Object.Destroy(GetComponent<StudentAI>());
			Object.Destroy(GetComponent<StudentAILeg>());
			Object.Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
			Object.Destroy(GetComponent<StudentAttackZone>());
		}
		else
		{
			Object.Destroy(GetComponent<StudentUserControl>());
		}
	}

	public void DoGuard()
	{
		if (gameController.isPlaying && !isDown)
		{
			anim.SetBool("Guard", true);
		}
	}

	public void DoGuardRelease()
	{
		anim.SetBool("Guard", false);
	}

	public void DoAttack()
	{
		if (gameController.isPlaying && animTag.Equals("Grounded") && !isDown && !isAttack)
		{
			if (Random.value < 0.5f)
			{
				AnimateTriggerAction("Punch", "PunchID", punchMax);
			}
			else
			{
				AnimateTriggerAction("Kick", "KickID", kickMax);
			}
			StartCoroutine(DoAttack2());
		}
	}

	private IEnumerator DoAttack2()
	{
		isAttack = true;
		yield return new WaitForSeconds(0.5f);
		isAttack = false;
	}

	public void DoSpecial()
	{
		if (gameController.isPlaying && !isDown)
		{
			AnimateTriggerAction("Special", "SpecialID", specialMax);
		}
	}

	public void DoDamage()
	{
		if (!animTag.Equals("Guard") && !animTag.Equals("Damage") && !isDown)
		{
			audioSource.PlayOneShot(seDamage);
			if (GameInfo.mode == GameInfo.Mode.Simple)
			{
				AnimateTriggerAction("Damage", "DamageID", 0);
			}
			else
			{
				AnimateTriggerAction("Damage", "DamageID", damageMax);
			}
		}
	}

	public void DoDown(int downCount)
	{
		if (isDown)
		{
			return;
		}
		isDown = true;
		audioSource.PlayOneShot(seDamage);
		GetComponent<Rigidbody>().isKinematic = true;
		canvasController.ChangeSliderHealthColor(CompareTag("Player"));
		if (GameInfo.mode == GameInfo.Mode.Simple)
		{
			anim.SetTrigger("SimpleDown");
			isDied = true;
			gameController.GameOver();
		}
		else if (downCount == 0)
		{
			anim.SetInteger("DownID", 0);
			anim.SetBool("Down", true);
			StartCoroutine(DoRecover());
			if (CompareTag("Player") && GameInfo.mode == GameInfo.Mode.Simple)
			{
				Object.FindObjectOfType<StudentAI>().SetTargetToNear();
			}
		}
		else
		{
			anim.SetInteger("DownID", 1);
			anim.SetBool("Down", true);
			isDied = true;
			gameController.GameOver();
		}
	}

	private void DoGetUp()
	{
		isDown = false;
		anim.SetBool("Down", false);
		GetComponent<Rigidbody>().isKinematic = false;
	}

	private IEnumerator DoRecover()
	{
		while (!health.RecoverHealth(40f))
		{
			yield return new WaitForEndOfFrame();
		}
		DoGetUp();
	}

	public void DoGreet()
	{
		AnimateBoolAction("Greet", "GreetID", greetMax);
	}

	public void DoGreetDone()
	{
		anim.SetBool("Greet", false);
	}

	public void DoBow()
	{
		anim.SetInteger("GreetID", 0);
		anim.SetBool("Greet", true);
	}

	public void DoFinishDown()
	{
		if (!isDied)
		{
			anim.SetTrigger("FinishDown");
		}
	}

	private void AnimateTriggerAction(string strTrigger, string strInteger, int actionMax)
	{
		int value = Random.Range(0, actionMax);
		anim.SetInteger(strInteger, value);
		anim.SetTrigger(strTrigger);
	}

	private void AnimateBoolAction(string strBool, string strInteger, int actionMax)
	{
		int value = Random.Range(0, actionMax);
		anim.SetInteger(strInteger, value);
		anim.SetBool(strBool, true);
	}
}
