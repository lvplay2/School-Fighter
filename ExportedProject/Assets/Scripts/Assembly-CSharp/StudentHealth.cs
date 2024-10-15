using UnityEngine;

public class StudentHealth : MonoBehaviour
{
	private float currentHealth;

	private float maxHealth = 100f;

	private int downCount;

	private Student student;

	private AnimTag animTag;

	private CanvasController canvasController;

	private GameController gameController;

	private void Start()
	{
		student = GetComponent<Student>();
		animTag = GetComponent<AnimTag>();
		canvasController = Object.FindObjectOfType<CanvasController>();
		gameController = Object.FindObjectOfType<GameController>();
		if (GameInfo.mode == GameInfo.Mode.Simple)
		{
			maxHealth = 200f;
		}
		currentHealth = maxHealth;
	}

	private void Update()
	{
		if (gameController.isPlaying)
		{
			AddHealth(Time.deltaTime * 1f);
		}
	}

	private void AddHealth(float amount)
	{
		currentHealth += amount;
		currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
		canvasController.ShowSliderHealth(CompareTag("Player"), currentHealth / maxHealth);
	}

	public void ChangeHealth(float amount)
	{
		if (animTag.Equals("Guard") || animTag.Equals("Damage") || animTag.Equals("Jump") || student.isDown)
		{
			return;
		}
		AddHealth(amount);
		if (currentHealth <= 0f)
		{
			if (!CompareTag("Player"))
			{
				gameController.AddScore(10000f);
			}
			student.DoDown(downCount++);
		}
		else
		{
			if (!CompareTag("Player"))
			{
				gameController.AddScore(1000f);
			}
			student.DoDamage();
		}
	}

	public bool RecoverHealth(float amount)
	{
		AddHealth(Time.deltaTime * amount);
		return (currentHealth >= maxHealth) ? true : false;
	}

	public float GetHealthScore()
	{
		return currentHealth + (float)(1 - downCount) * maxHealth;
	}
}
