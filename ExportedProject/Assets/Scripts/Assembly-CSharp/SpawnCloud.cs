using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour
{
	public GameObject waypointPrefab;

	public float collisionCheckSize = 4f;

	public bool showWaypoint;

	private List<Vector3> listWaypoint;

	private void Start()
	{
		listWaypoint = new List<Vector3>(10000);
		MakeWaypoint();
	}

	private void MakeWaypoint()
	{
		int num = (int)base.transform.localScale.x / 2;
		int num2 = (int)base.transform.localScale.z / 2;
		int num3 = (int)collisionCheckSize;
		for (int i = -num2; i < num2; i += num3)
		{
			for (int j = -num; j < num; j += num3)
			{
				Vector3 targetPosition = new Vector3(j, 0f, i) + base.transform.position;
				targetPosition = GetOnRoadPosition(targetPosition);
				if (targetPosition != Vector3.zero && !CheckCollision(targetPosition))
				{
					listWaypoint.Add(targetPosition);
					if (showWaypoint)
					{
						Object.Instantiate(waypointPrefab, targetPosition, Quaternion.identity);
					}
				}
			}
		}
		Debug.Log("Waypoint Count:" + listWaypoint.Count);
	}

	private bool CheckCollision(Vector3 targetPosition)
	{
		return Physics.CheckSphere(targetPosition + Vector3.up * (collisionCheckSize + 1f), collisionCheckSize);
	}

	private Vector3 GetNewCloudPosition()
	{
		float x = Random.Range(-1f, 1f) * base.transform.localScale.x / 2f;
		float z = Random.Range(-1f, 1f) * base.transform.localScale.z / 2f;
		return new Vector3(x, 0f, z) + base.transform.position;
	}

	private Vector3 GetNewGroundPosition()
	{
		int index = Random.Range(0, listWaypoint.Count);
		return listWaypoint[index];
	}

	public Vector3 GetNewGroundPositionAroundObject(GameObject obj, float distanceThreshold)
	{
		List<Vector3> list = new List<Vector3>(10000);
		for (int i = 0; i < listWaypoint.Count; i++)
		{
			float magnitude = (listWaypoint[i] - obj.transform.position).magnitude;
			if (magnitude < distanceThreshold && !CheckCollision(listWaypoint[i]))
			{
				list.Add(listWaypoint[i]);
			}
		}
		if (list.Count == 0)
		{
			Debug.Log("SpawnClowd:No target around object");
			int index = Random.Range(0, listWaypoint.Count);
			return listWaypoint[index];
		}
		int index2 = Random.Range(0, list.Count);
		return list[index2];
	}

	public Vector3 GetNewGroundPositionAwayFromPlayer(float distanceThreshold)
	{
		List<Vector3> list = new List<Vector3>(10000);
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		for (int i = 0; i < listWaypoint.Count; i++)
		{
			float magnitude = (listWaypoint[i] - gameObject.transform.position).magnitude;
			if (magnitude > distanceThreshold && !CheckCollision(listWaypoint[i]))
			{
				list.Add(listWaypoint[i]);
			}
		}
		if (list.Count == 0)
		{
			Debug.Log("SpawnClowd:No target away from player");
			int index = Random.Range(0, listWaypoint.Count);
			return listWaypoint[index];
		}
		int index2 = Random.Range(0, list.Count);
		return list[index2];
	}

	private Vector3 GetOnRoadPosition(Vector3 targetPosition)
	{
		Vector3 result = Vector3.zero;
		RaycastHit hitInfo;
		Physics.Raycast(targetPosition, -Vector3.up, out hitInfo, base.transform.position.y * 1.5f);
		if (hitInfo.collider != null && hitInfo.collider.CompareTag("Road"))
		{
			result = hitInfo.point;
		}
		return result;
	}

	public void SpawnNewCloudObject(GameObject prefab)
	{
		Vector3 newCloudPosition = GetNewCloudPosition();
		Object.Instantiate(prefab, newCloudPosition, Quaternion.identity);
	}

	public void SpawnNewGoundObject(GameObject prefab)
	{
		Vector3 newGroundPosition = GetNewGroundPosition();
		Object.Instantiate(prefab, newGroundPosition, Quaternion.identity);
	}

	public void SpawnNewGroundObjectAroundObject(GameObject prefab, GameObject targetObject, float distanceThreshold)
	{
		Vector3 newGroundPositionAroundObject = GetNewGroundPositionAroundObject(targetObject, distanceThreshold);
		Object.Instantiate(prefab, newGroundPositionAroundObject, Quaternion.identity);
	}

	public void SpawnNewGroundObjectAwayFromPlayer(GameObject prefab, float distanceThreshold)
	{
		Vector3 newGroundPositionAwayFromPlayer = GetNewGroundPositionAwayFromPlayer(distanceThreshold);
		Object.Instantiate(prefab, newGroundPositionAwayFromPlayer, Quaternion.identity);
	}
}
