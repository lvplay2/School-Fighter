using UnityEngine;

public class RoboVoice : MonoBehaviour
{
	public enum Voice
	{
		Round = 0,
		Fight = 1,
		TimesUp = 2,
		Clear = 3,
		Failed = 4,
		Draw = 5,
		KO = 6,
		Perfect = 7,
		Victory = 8
	}

	public AudioClip[] seVoices;

	public AudioClip[] seNumbers;

	public Voice voice;

	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void SpeakVoice(Voice voice)
	{
		audioSource.PlayOneShot(seVoices[(int)voice]);
	}

	public void SpeakNumber(int num)
	{
		audioSource.PlayOneShot(seNumbers[num]);
	}
}
