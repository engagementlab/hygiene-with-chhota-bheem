using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeMove : MonoBehaviour
{

	public bool MoveEnabled = true;

	[Range(1, 10f)]
	public float MoveSpeed = 1;

	[HideInInspector]
	public int SpawnTypeIndex = 0;
	[HideInInspector]
	public int Direction= 0;
	[CanBeNull] [HideInInspector]
	public string SpawnType;
	
	[CanBeNull] [HideInInspector]
	public string MovementDir;
	
	[HideInInspector]
	public float CurrentPathPercent = 0.0f; //min 0, max 1

	[Range(0, 10f)]
	public float AnimationDuration = 1f;

	[CanBeNull] List<Vector3> waypoints;
	[CanBeNull] GameObject localParent;

	float currentPathPercent = 0.0f; //min 0, max 1
	private Vector3 delta;
	
  public Vector3 ClampToScreen(Vector3 vector) {

  	var pos = Camera.main.ScreenToWorldPoint(vector);
		pos.z = 0;

  	return pos;

  }

	public void AddWaypoint()
	{
		
		var waypointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Waypoint.prefab");
		var waypoint = Instantiate(waypointPrefab, Vector3.zero, Quaternion.identity);

		waypoint.transform.parent = transform;
		var waypointChildren = gameObject.GetComponentsInChildren<Waypoint>();
		waypoint.name = "Waypoint_" + waypointChildren.Length;

		if(waypointChildren.Length > 1)
			waypoint.transform.position = waypointChildren[waypointChildren.Length - 2].transform.position;
		else
			waypoint.transform.position = transform.position;
		
		Selection.activeGameObject = waypoint;

	}

	protected void SetupWaypoints()
	{

		waypoints = new List<Vector3>();

		foreach(Transform tr in transform)
		{
			if(tr.tag == "Waypoint" && tr.gameObject.activeInHierarchy)
				waypoints.Add(tr.position);
		}

		if(waypoints.Count <= 0) return;
		
		// Make this object child of new parent to allow local path animation along with other x/y movement
		localParent = new GameObject("Parent-"+gameObject.name);
		localParent.transform.position = transform.position;

		if(transform.parent != null)
			localParent.transform.parent = transform.parent;
			
		transform.SetParent(localParent.transform);
		transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
				
		var opts = iTween.Hash("path", waypoints.ToArray(), "movetopath", false, "islocal", true, "time", AnimationDuration, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear);
//		iTween.MoveTo(gameObject, opts);
	}

	public void Awake()
	{
		
		delta = new Vector3();
			
		if(!MoveEnabled) return;
		
		if(GetType().Name != "ArchetypeSpawner")
			SetupWaypoints();
	
	}
	
	void OnDrawGizmos() {

			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(transform.position, Vector3.one);
		
			//if(waypoints.Count > 0)
				//iTween.DrawPath(waypoints.ToArray());
	
	}

	public void OnDrawGizmosSelected()
	{

		var waypointChildren = new List<Transform>();

		foreach(Transform t in transform)
		{
			if(t.tag == "Waypoint")
				waypointChildren.Add(t);
			
		}
		
		if(waypointChildren.Count > 0)
		{
			
			if(Selection.activeGameObject == gameObject)
				Gizmos.color = Color.yellow;
			else
				Gizmos.color = Color.cyan;
			
			Gizmos.DrawLine(transform.position, waypointChildren[0].transform.position);
			
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
		
	}
	
	// Update is called once per frame
	public void Update () {
		
		var target = localParent != null ? localParent.transform.position : transform.position;
		Vector3 deltaPos = Vector3.zero;

		if(MovementDir == "up")
			target.y += MoveSpeed;
		else if(MovementDir == "right")
			target.x += MoveSpeed;
		else if(MovementDir == "left")
			target.x -= MoveSpeed;
		else
		{
			target.y -= MoveSpeed;
			deltaPos.y -= MoveSpeed;
		}


		if(!MoveEnabled || MoveSpeed == 0)
		{
			if(localParent != null)
				localParent.transform.position = Vector3.Lerp(localParent.transform.position, target, Time.deltaTime);
			else
				transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
		}

		if(waypoints.Count <= 0) return;
		currentPathPercent = Mathf.PingPong(Time.time * AnimationDuration, 1);

		for(int w = 0; w < waypoints.Count; w++)
		{
			Vector3 v = Vector3.Lerp(waypoints[w], waypoints[w] + deltaPos, Time.deltaTime);
			waypoints[w] = v;
		}
		
		Debug.Log(currentPathPercent);

		iTween.PutOnPath(gameObject, waypoints.ToArray(), currentPathPercent);
		
	}

}