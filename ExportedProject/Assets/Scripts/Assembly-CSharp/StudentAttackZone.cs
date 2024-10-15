using UnityEngine;

public class StudentAttackZone : MonoBehaviour
{
	private Student student;

	private void Start()
	{
		student = GetComponent<Student>();
	}

	private void Update()
	{
		CheckAttackZone();
	}

	private void CheckAttackZone()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position + base.transform.forward * 0.3f, 0.3f);
		bool flag = false;
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (collider.CompareTag("Player") && !collider.GetComponent<Student>().isDown)
			{
				flag = true;
			}
		}
		if (flag && Random.Range(0, 5) == 0)
		{
			student.DoAttack();
		}
	}
}
