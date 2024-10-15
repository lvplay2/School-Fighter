using UnityEngine;

public class StudentAttackParts : MonoBehaviour
{
	private float damage = 10f;

	private Student student;

	private void Start()
	{
		student = base.transform.root.GetComponent<Student>();
		SetDamageWeight();
	}

	private void SetDamageWeight()
	{
		int num = int.Parse((!PlayerPrefs.HasKey("str_level")) ? "0" : PlayerPrefs.GetString("str_level"));
		float[] array = new float[3] { 1.3f, 1f, 0.7f };
		if (base.transform.root.CompareTag("Player"))
		{
			damage *= array[num];
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(student == null) && student.isAttack)
		{
			other.gameObject.SendMessageUpwards("ChangeHealth", 0f - damage, SendMessageOptions.DontRequireReceiver);
		}
	}
}
