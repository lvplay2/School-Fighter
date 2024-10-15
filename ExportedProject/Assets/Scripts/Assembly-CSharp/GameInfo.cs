using UnityEngine;

public class GameInfo : MonoBehaviour
{
	public enum Mode
	{
		Normal = 0,
		Simple = 1
	}

	public enum AppName
	{
		SchoolFighter = 0,
		RooftopFighter = 1
	}

	public static float score;

	public static int round = 1;

	public static Mode mode;

	public static AppName appName;
}
