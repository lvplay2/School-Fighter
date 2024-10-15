using System.Collections;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Utility;

public class FinishAnim : MonoBehaviour
{
	private Vector3 endPivotPosition;

	public void StartAnim(bool isWin)
	{
		if (GameInfo.appName == GameInfo.AppName.SchoolFighter)
		{
			StartCoroutine(CoStartCamera());
			StartCoroutine(CoStartAnim(isWin));
		}
		else if (isWin)
		{
			StartCoroutine(CoStartCamera());
			StartCoroutine(CoStartAnim(isWin));
		}
	}

	private IEnumerator CoStartCamera()
	{
		Transform cam = Camera.main.transform;
		AutoCam autoCam = cam.GetComponentInParent<AutoCam>();
		Transform pivot = autoCam.transform.GetChild(0);
		ProtectCameraFromWallClip pcfwc = autoCam.GetComponent<ProtectCameraFromWallClip>();
		autoCam.m_MoveSpeed = 1f;
		autoCam.m_TurnSpeed = 1f;
		FreeLookCam freeLookCam = cam.GetComponentInParent<FreeLookCam>();
		if (freeLookCam != null)
		{
			freeLookCam.enabled = false;
		}
		LookatTarget lookAtTarget = cam.GetComponentInParent<LookatTarget>();
		if (lookAtTarget != null)
		{
			lookAtTarget.enabled = false;
		}
		TargetFieldOfView targetFieldOfView = cam.GetComponentInParent<TargetFieldOfView>();
		if (targetFieldOfView != null)
		{
			targetFieldOfView.enabled = false;
		}
		AutoMoveAndRotate autoMoveAndRotate = cam.GetComponentInParent<AutoMoveAndRotate>();
		if (autoMoveAndRotate != null)
		{
			autoMoveAndRotate.enabled = false;
		}
		autoCam.enabled = true;
		autoCam.isLookFromFront = true;
		pcfwc.enabled = true;
		yield return new WaitForEndOfFrame();
		Vector3 startPivotPosition = pivot.localPosition;
		Quaternion startPivotRotation = pivot.localRotation;
		Quaternion startCameraRotation = cam.localRotation;
		float startFOV = Camera.main.fieldOfView;
		float startDist = pcfwc.m_OriginalDist;
		endPivotPosition = new Vector3(0f, 1.8f, 0f);
		Quaternion endPivotRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		Quaternion endCameraRotation = Quaternion.Euler(new Vector3(10f, 0f, 0f));
		float endFOV = 50f;
		float endDist = 1.5f;
		float timeMove = 2f;
		float elappsedTime = 0f;
		while (elappsedTime < timeMove)
		{
			elappsedTime += Time.deltaTime;
			float ratio = elappsedTime / timeMove;
			pivot.localPosition = Vector3.Lerp(startPivotPosition, endPivotPosition, ratio);
			pivot.localRotation = Quaternion.Lerp(startPivotRotation, endPivotRotation, ratio);
			cam.localRotation = Quaternion.Lerp(startCameraRotation, endCameraRotation, ratio);
			Camera.main.fieldOfView = Mathf.Lerp(startFOV, endFOV, ratio);
			pcfwc.m_OriginalDist = Mathf.Lerp(startDist, endDist, ratio);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator CoStartAnim(bool isWin)
	{
		yield return new WaitForEndOfFrame();
		if (GameInfo.appName == GameInfo.AppName.RooftopFighter)
		{
			endPivotPosition = new Vector3(0f, 1.5f, 0f);
		}
		AutoCam autoCam = Camera.main.GetComponentInParent<AutoCam>();
		Transform target;
		CapsuleCollider collider;
		if (isWin)
		{
			target = GameController.instance.fighterPlayer.transform;
			collider = GameController.instance.fighterCPU.GetComponent<CapsuleCollider>();
			GameController.instance.fighterCPU.GetComponent<Student>().DoFinishDown();
		}
		else
		{
			target = GameController.instance.fighterCPU.transform;
			collider = GameController.instance.fighterPlayer.GetComponent<CapsuleCollider>();
			GameController.instance.fighterPlayer.GetComponent<Student>().DoFinishDown();
			ProtectCameraFromWallClip pcfwc = autoCam.GetComponent<ProtectCameraFromWallClip>();
			pcfwc.dontClipTag = "CPU";
		}
		autoCam.SetTarget(target.transform);
		collider.height = 0f;
		collider.center = new Vector3(0f, 0.5f, 0f);
		yield return new WaitForSeconds(3f);
		if (GameInfo.appName == GameInfo.AppName.SchoolFighter)
		{
			target.GetComponent<Student>().DoGreet();
		}
	}
}
