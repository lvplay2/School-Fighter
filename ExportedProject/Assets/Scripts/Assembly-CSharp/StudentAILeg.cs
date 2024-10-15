using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class StudentAILeg : MonoBehaviour
{
	public Transform target;

	private AnimTag animTag;

	public UnityEngine.AI.NavMeshAgent agent { get; private set; }

	public StudentLeg character { get; private set; }

	private void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		character = GetComponent<StudentLeg>();
		animTag = GetComponent<AnimTag>();
		agent.updateRotation = false;
		agent.updatePosition = true;
	}

	private void Update()
	{
		CheckOnGround();
	}

	private void FixedUpdate()
	{
		if (GameInfo.mode == GameInfo.Mode.Simple)
		{
			agent.SetDestination(base.transform.position);
			character.Move(Vector3.zero, false, false);
		}
		else if (target != null && GameController.instance.isPlaying)
		{
			agent.SetDestination(target.position);
			character.Move(agent.desiredVelocity, false, false);
		}
		else
		{
			character.Move(Vector3.zero, false, false);
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	private void CheckOnGround()
	{
		if (animTag.Equals("Grounded"))
		{
			agent.Resume();
		}
		else
		{
			agent.Stop();
		}
	}
}
