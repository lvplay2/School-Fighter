using UnityEngine;

public class FootSound : MonoBehaviour
{
	private AudioSource audioSource;

	private StudentLeg character;

	public AudioClip clipFootstep;

	public AudioClip clipFootLanding;

	private bool previousOnGoundStatus;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		character = GetComponent<StudentLeg>();
		previousOnGoundStatus = true;
	}

	private void Update()
	{
		CheckPlaySoundFootLanding();
	}

	private void CheckPlaySoundFootLanding()
	{
		if (base.gameObject.CompareTag("Player"))
		{
			if (!previousOnGoundStatus && character.onGround)
			{
				audioSource.PlayOneShot(clipFootLanding);
			}
			previousOnGoundStatus = character.onGround;
		}
	}

	public void PlaySoundFootStep()
	{
		audioSource.PlayOneShot(clipFootstep);
	}
}
