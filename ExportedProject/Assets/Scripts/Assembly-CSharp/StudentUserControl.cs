using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(StudentLeg))]
public class StudentUserControl : MonoBehaviour
{
	private StudentLeg m_Character;

	public Transform m_Cam;

	private Vector3 m_CamForward;

	private Vector3 m_Move;

	private bool m_Jump;

	private GameController gameController;

	private Student student;

	private AnimTag animTag;

	private void Start()
	{
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
		}
		m_Character = GetComponent<StudentLeg>();
		student = GetComponent<Student>();
		animTag = GetComponent<AnimTag>();
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}

	private void Update()
	{
		if (gameController.isPlaying)
		{
			CheckInput();
			CheckTestInput();
			if (!m_Jump)
			{
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
		}
	}

	private void FixedUpdate()
	{
		float num = CrossPlatformInputManager.GetAxis("Horizontal");
		float num2 = CrossPlatformInputManager.GetAxis("Vertical");
		if (!gameController.isPlaying)
		{
			num = 0f;
			num2 = 0f;
		}
		if (m_Cam != null)
		{
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1f, 0f, 1f)).normalized;
			m_Move = num2 * m_CamForward + num * m_Cam.right;
		}
		else
		{
			m_Move = num2 * Vector3.forward + num * Vector3.right;
		}
		m_Character.Move(m_Move, false, m_Jump);
		m_Jump = false;
	}

	private void CheckInput()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire3"))
		{
			student.DoGuard();
		}
		else if (CrossPlatformInputManager.GetButtonUp("Fire3"))
		{
			student.DoGuardRelease();
		}
		if (CrossPlatformInputManager.GetButtonDown("Fire2"))
		{
			student.DoAttack();
		}
	}

	private void CheckTestInput()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			student.DoDamage();
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			student.DoDown(0);
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (animTag.Equals("Greet"))
			{
				student.DoGreetDone();
			}
			else
			{
				student.DoGreet();
			}
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			student.DoSpecial();
		}
	}
}
