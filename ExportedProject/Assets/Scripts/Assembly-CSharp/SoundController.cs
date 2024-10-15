using UnityEngine;

public class SoundController : MonoBehaviour
{
	[HideInInspector]
	public AudioSource audioSource;

	public static SoundController instance;

	public AudioClip clipStart;

	public AudioClip clipUI1;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		instance = this;
	}

	public void PlayOneShot(AudioClip clip)
	{
		audioSource.PlayOneShot(clip);
	}

	public void Play(AudioClip clip)
	{
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
	}

	public void PlaySoundStart()
	{
		Play(clipStart);
	}

	public void PlaySoundUI1()
	{
		Play(clipUI1);
	}
}
