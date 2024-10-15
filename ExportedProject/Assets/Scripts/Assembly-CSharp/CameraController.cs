using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraController : MonoBehaviour
{
	private int cameraID;

	private List<GameObject> listCamera;

	private StudentUserControl userControl;

	private GameObject cctvTarget;

	private void Start()
	{
		listCamera = new List<GameObject>(5);
		InitCameras();
	}

	private void InitCameras()
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			listCamera.Add(base.transform.GetChild(i).gameObject);
		}
		cameraID = int.Parse((!PlayerPrefs.HasKey("str_cameraID")) ? "0" : PlayerPrefs.GetString("str_cameraID"));
		cameraID = Mathf.Clamp(cameraID, 0, childCount);
		ActivateCamera();
	}

	private void SetCCTVTarget()
	{
		if (cctvTarget == null)
		{
			cctvTarget = new GameObject("CCTVTarget");
			cctvTarget.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
			cctvTarget.transform.localPosition = new Vector3(0f, 1.5f, 0.5f);
		}
		GameObject gameObject = GameObject.Find("CctvCamera");
		if (gameObject != null)
		{
			gameObject.GetComponent<LookatTarget>().SetTarget(cctvTarget.transform);
		}
	}

	private void ActivateCamera()
	{
		foreach (GameObject item in listCamera)
		{
			item.SetActive(false);
		}
		listCamera[cameraID].SetActive(true);
		PlayerPrefs.SetString("str_cameraID", string.Format("{0}", cameraID));
		userControl = Object.FindObjectOfType<StudentUserControl>();
		userControl.m_Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		SetCCTVTarget();
	}

	public void DoChangeCamera()
	{
		cameraID++;
		if (cameraID >= listCamera.Count)
		{
			cameraID = 0;
		}
		ActivateCamera();
	}
}
