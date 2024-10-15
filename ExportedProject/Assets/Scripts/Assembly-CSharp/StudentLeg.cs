using System;
using System.Collections;
using UnityEngine;

public class StudentLeg : MonoBehaviour
{
	[Serializable]
	public class AdvancedSettings
	{
		public float stationaryTurnSpeed = 180f;

		public float movingTurnSpeed = 360f;

		public float autoTurnThresholdAngle = 100f;

		public float autoTurnSpeed = 2f;

		public float jumpRepeatDelayTime = 0.25f;

		public float runCycleLegOffset = 0.2f;

		public float groundStickyEffect = 5f;
	}

	private class RayHitComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}

	private const float half = 0.5f;

	public float walkSpeed = 4f;

	[SerializeField]
	private float jumpPower = 10f;

	[SerializeField]
	private float airSpeed = 6f;

	[SerializeField]
	private float airControl = 2f;

	[Range(1f, 4f)]
	[SerializeField]
	public float gravityMultiplier = 2f;

	[SerializeField]
	private AdvancedSettings advancedSettings;

	public bool onGround;

	private Animator animator;

	private float lastAirTime;

	private Vector3 moveInput;

	private bool jumpInput;

	private float turnAmount;

	private float forwardAmount;

	private Vector3 velocity;

	private IComparer rayHitComparer;

	private AnimTag animTag;

	private GameController gameController;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rayHitComparer = new RayHitComparer();
		animTag = GetComponent<AnimTag>();
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}

	public void Move(Vector3 move, bool crouch, bool jump)
	{
		if (move.magnitude > 1f)
		{
			move.Normalize();
		}
		moveInput = move;
		jumpInput = jump;
		velocity = GetComponent<Rigidbody>().velocity;
		ConvertMoveInput();
		if ((animTag.Equals("Grounded") || animTag.Equals("Jump")) && gameController.isPlaying)
		{
			MoveForward();
			ApplyExtraTurnRotation();
		}
		GroundCheck();
		if (onGround)
		{
			HandleGroundedVelocities();
		}
		else
		{
			HandleAirborneVelocities();
		}
		UpdateAnimator();
		GetComponent<Rigidbody>().velocity = velocity;
	}

	private void ConvertMoveInput()
	{
		Vector3 vector = base.transform.InverseTransformDirection(moveInput);
		turnAmount = Mathf.Atan2(vector.x, vector.z);
		forwardAmount = vector.z;
		if (turnAmount > 3f)
		{
			turnAmount = 0f;
		}
	}

	private void ApplyExtraTurnRotation()
	{
		float num = Mathf.Lerp(advancedSettings.stationaryTurnSpeed, advancedSettings.movingTurnSpeed, forwardAmount);
		base.transform.Rotate(0f, turnAmount * num * Time.deltaTime, 0f);
	}

	private void GroundCheck()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up * 0.1f, -Vector3.up);
		RaycastHit[] array = Physics.RaycastAll(ray, 0.5f);
		Array.Sort(array, rayHitComparer);
		if (velocity.y < jumpPower * 0.5f)
		{
			onGround = false;
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit raycastHit = array2[i];
				if (!raycastHit.collider.isTrigger)
				{
					if (velocity.y <= 0f)
					{
						GetComponent<Rigidbody>().position = Vector3.MoveTowards(GetComponent<Rigidbody>().position, raycastHit.point, Time.deltaTime * advancedSettings.groundStickyEffect);
					}
					onGround = true;
					break;
				}
			}
		}
		if (!onGround)
		{
			lastAirTime = Time.time;
		}
	}

	private void HandleGroundedVelocities()
	{
		velocity.y = 0f;
		if (moveInput.magnitude == 0f)
		{
			velocity.x = 0f;
			velocity.z = 0f;
		}
		bool flag = Time.time > lastAirTime + advancedSettings.jumpRepeatDelayTime;
		if (jumpInput && flag && animTag.Equals("Grounded"))
		{
			onGround = false;
			velocity = moveInput * airSpeed;
			velocity.y = jumpPower;
		}
	}

	private void HandleAirborneVelocities()
	{
		velocity = Vector3.Lerp(b: new Vector3(moveInput.x * airSpeed, velocity.y, moveInput.z * airSpeed), a: velocity, t: Time.deltaTime * airControl);
		GetComponent<Rigidbody>().useGravity = true;
		Vector3 force = Physics.gravity * gravityMultiplier - Physics.gravity;
		GetComponent<Rigidbody>().AddForce(force);
	}

	private void UpdateAnimator()
	{
		animator.applyRootMotion = onGround;
		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
		animator.SetBool("OnGround", onGround);
		if (!onGround)
		{
			animator.SetFloat("Jump", velocity.y);
		}
		float num = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + advancedSettings.runCycleLegOffset, 1f);
		float value = (float)((num < 0.5f) ? 1 : (-1)) * forwardAmount;
		if (onGround)
		{
			animator.SetFloat("JumpLeg", value);
		}
	}

	private void SetUpAnimator()
	{
		this.animator = GetComponent<Animator>();
		Animator[] componentsInChildren = GetComponentsInChildren<Animator>();
		foreach (Animator animator in componentsInChildren)
		{
			if (animator != this.animator)
			{
				this.animator.avatar = animator.avatar;
				UnityEngine.Object.Destroy(animator);
				break;
			}
		}
	}

	private void MoveForward()
	{
		Vector3 vector = base.transform.forward * forwardAmount;
		base.transform.position += vector * Time.fixedDeltaTime * walkSpeed;
		if (turnAmount == 0f && forwardAmount < 0f)
		{
			turnAmount = 1f;
		}
	}
}
