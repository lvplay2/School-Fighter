using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StudentAILeg))]
public class StudentAI : MonoBehaviour
{
	private StudentAILeg aiLeg;

	private GameObject objPlayer;

	private GameObject target;

	private SpawnCloud spawnCloud;

	private Student student;

	private void Start()
	{
		aiLeg = GetComponent<StudentAILeg>();
		spawnCloud = GameObject.Find("SpawnCloud").GetComponent<SpawnCloud>();
		student = GetComponent<Student>();
		target = new GameObject("StudentTarget");
		StartCoroutine(UpdateTargetToSomewhere());
		StartCoroutine(UpdateAttack());
	}

	private void Update()
	{
		CheckAgent();
	}

	private void SetTargetToPlayer()
	{
		if (!objPlayer)
		{
			objPlayer = GameObject.FindGameObjectWithTag("Player");
		}
		aiLeg.SetTarget(objPlayer.transform);
	}

	public void SetTargetToNear()
	{
		Vector3 newGroundPositionAroundObject = spawnCloud.GetNewGroundPositionAroundObject(base.gameObject, 10f);
		target.transform.position = newGroundPositionAroundObject;
		aiLeg.SetTarget(target.transform);
	}

	private void CheckAgent()
	{
		if ((target.transform.position - base.transform.position).magnitude < 1f)
		{
			SetTargetToNear();
		}
		else if (!aiLeg.agent.hasPath)
		{
			SetTargetToNear();
		}
	}

	private IEnumerator UpdateTargetToSomewhere()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(2f, 4f));
			if (Random.value < 0.6f)
			{
				SetTargetToPlayer();
			}
			else
			{
				SetTargetToNear();
			}
		}
	}

	private IEnumerator UpdateAttack()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1f, 3f));
			student.DoAttack();
		}
	}

	private void OnDestroy()
	{
		Object.Destroy(target);
	}
}
