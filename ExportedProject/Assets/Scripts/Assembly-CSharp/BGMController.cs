using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
	public enum Clip
	{
		Loading = 0,
		Menu = 1,
		Win = 2,
		Lose = 3,
		Play = 4
	}

	public AudioClip bgmLoading;

	public AudioClip bgmMenu;

	public AudioClip bgmWin;

	public AudioClip bgmLose;

	public AudioClip[] bgmPlay;

	private List<AudioClip> listClips;

	[HideInInspector]
	public AudioSource audioSource;

	public static BGMController instance;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		instance = this;
		listClips = new List<AudioClip>(5);
		listClips.Add(bgmLoading);
		listClips.Add(bgmMenu);
		listClips.Add(bgmWin);
		listClips.Add(bgmLose);
	}

	public void PlayBGM(Clip clip)
	{
		audioSource.Stop();
		if (clip < Clip.Play)
		{
			audioSource.clip = listClips[(int)clip];
		}
		else
		{
			int num = Random.Range(0, bgmPlay.Length);
			audioSource.clip = bgmPlay[num];
		}
		audioSource.Play();
	}

	public void Mute(bool flg)
	{
		audioSource.mute = flg;
	}
}
