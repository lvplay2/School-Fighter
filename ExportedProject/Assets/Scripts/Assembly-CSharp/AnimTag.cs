using UnityEngine;

public class AnimTag : MonoBehaviour
{
	public string animTag;

	private Animator anim;

	private string[] names = new string[9] { "Grounded", "Jump", "Attack", "Guard", "Special", "Down", "Damage", "Greet", "Crunch" };

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		GetAnimTagHash();
	}

	private void GetAnimTagHash()
	{
		int tagHash = anim.GetCurrentAnimatorStateInfo(0).tagHash;
		int tagHash2 = anim.GetNextAnimatorStateInfo(0).tagHash;
		animTag = "Unknown";
		string[] array = names;
		foreach (string text in array)
		{
			if (tagHash2 == Animator.StringToHash(text))
			{
				animTag = text;
			}
		}
		if (!animTag.Equals("Unknown") && !animTag.Equals("Grounded"))
		{
			return;
		}
		string[] array2 = names;
		foreach (string text2 in array2)
		{
			if (tagHash == Animator.StringToHash(text2))
			{
				animTag = text2;
			}
		}
	}

	public bool Equals(string tag)
	{
		if (animTag.Equals(tag))
		{
			return true;
		}
		return false;
	}
}
