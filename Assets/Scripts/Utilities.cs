using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Utilities : MonoBehaviour {


	public static Vector3 ClampToScreen(Vector3 vector, Camera camera) {

		Vector3 pos = camera.WorldToViewportPoint(vector);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos.z = vector.z;

		Vector3 worldPos = camera.ViewportToWorldPoint(pos);
		worldPos.x = Mathf.Clamp(worldPos.x, -6.9f, 6.9f);
		worldPos.z = vector.z;

		return worldPos;

	}

	public static void DrawWaypoints(Transform t)
	{
		
		#if UNITY_EDITOR
		var waypointChildren = new List<Transform>();
		foreach(Transform tr in t)
		{
			if(tr.tag == "WaypointsPattern" && tr.gameObject.activeInHierarchy)
			{
				foreach(Transform wp in tr)
					waypointChildren.Add(wp);
			}
			else if(tr.tag == "Waypoint" && tr.gameObject.activeInHierarchy)
				waypointChildren.Add(tr);
		}

		if(waypointChildren.Count > 0)
		{
			
			if(Selection.activeGameObject == t.gameObject)
				Gizmos.color = Color.yellow;
			else
				Gizmos.color = Color.cyan;
			
			Gizmos.DrawLine(t.position, waypointChildren[0].transform.position);
			
		}
			
		if(waypointChildren.Count > 1)
		{
			for(var i = 0; i < waypointChildren.Count; i++)
			{
				if(waypointChildren.Count - 1 > i)
				{
					if(Selection.activeGameObject == waypointChildren[i].gameObject)
						Gizmos.color = Color.yellow;
					else
						Gizmos.color = Color.cyan;
						
					Gizmos.DrawLine(waypointChildren[i].transform.position, waypointChildren[i+1].transform.position);
				}

			}
		}
		#endif
	}

	public static float GetZPosition(GameObject obj)
	{
		float zPos;
		
		switch(obj.tag)
		{
			case "ScorpionSnake":
				zPos = -1;
				break;
			case "Bubble":
				zPos = -2;
				break;
			default:
				zPos = 0;
				break;
		}

		return zPos;
	}
}
