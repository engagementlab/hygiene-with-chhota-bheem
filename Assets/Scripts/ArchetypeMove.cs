using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeMove : MonoBehaviour
{

	public bool moveEnabled = true;

	public float MoveSpeed {
		get { return _MoveSpeed; }
		set { _MoveSpeed = value; }
	}

	[HideInInspector]
	public int _spawnTypeIndex = 0;
	[HideInInspector]
	public int _direction= 0;
	[HideInInspector]
	public string spawnType;
	
	[HideInInspector]
	public string movementDir;
	
	[HideInInspector]
	public Vector3[] movementPoints = new Vector3[20];
	[HideInInspector]
	public float currentPathPercent = 0.0f; //min 0, max 1

//	[HideInInspector]
	public float _MoveSpeed;
	
	[Range(0, 10f)]
	public float localMoveDuration = 1f;
	
	MeshRenderer rend;

	List<Vector3> waypoints;

  public Vector3 ClampToScreen(Vector3 vector) {

  	Vector3 pos = Camera.main.ScreenToWorldPoint(vector);
		pos.z = 0;

  	return pos;

  }

	public void OnDrawGizmosSelected()
	{

		Waypoint[] waypointChildren = gameObject.GetComponentsInChildren<Waypoint>();

		if(waypointChildren.Length > 0)
		{
			if(Selection.activeGameObject == gameObject)
				Gizmos.color = Color.yellow;
			else
				Gizmos.color = Color.cyan;
			
			Gizmos.DrawLine(transform.position, waypointChildren[0].transform.position);
		}
			
		if(waypointChildren.Length > 1)
		{
			for(int i = 0; i < waypointChildren.Length; i++)
			{
				if(waypointChildren.Length - 1 > i)
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

	public void AddWaypoint()
	{
		
		GameObject waypointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Waypoint.prefab");
		GameObject waypoint = Instantiate(waypointPrefab, Vector3.zero, Quaternion.identity);

		waypoint.transform.parent = transform;
		Waypoint[] waypointChildren = gameObject.GetComponentsInChildren<Waypoint>();
		waypoint.name = "Waypoint_" + waypointChildren.Length;

		if(waypointChildren.Length > 1)
			waypoint.transform.position = waypointChildren[waypointChildren.Length-2].transform.position;

		Selection.activeGameObject = waypoint;

	}

	public void Awake()
	{

		if(!moveEnabled) return;

		waypoints = new List<Vector3>();

		foreach(Transform tr in transform)
		{
			if(tr.tag == "Waypoint")
			{
				waypoints.Add(tr.position);
			}
		}

		if(waypoints.Count > 0)
			iTween.MoveTo(gameObject, iTween.Hash("path", waypoints.ToArray(), "islocal", true, "time", localMoveDuration, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear));
	
	}
	
	void OnDrawGizmos() {

			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(transform.position, Vector3.one);
	}
	
	// Update is called once per frame
	public void Update () {

		if(!moveEnabled || waypoints.Count > 0)
			return;

		if(_MoveSpeed == 0)
			return;
		
		float speed = _MoveSpeed;

		Vector3 target = transform.position;
		
		if(movementDir == "up")
			target.y += speed;
		else if(movementDir == "right")
			target.x += speed;
		else if(movementDir == "left")
			target.x -= speed;
		else
			target.y -= speed;

		transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
		
	}

}