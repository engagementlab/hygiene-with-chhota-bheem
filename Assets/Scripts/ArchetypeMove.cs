using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeMove : MonoBehaviour
{

	public bool MoveEnabled = true;

	[HideInInspector]
	public float MoveSpeed = 1;
	
	[HideInInspector]
	public bool UseParentSpeed = false;

	[HideInInspector]
	public int SpawnTypeIndex;
	
	[HideInInspector]
	public int Direction;
	
	[CanBeNull] [HideInInspector]
	public string SpawnType;
	
	[CanBeNull] [HideInInspector]
	public string MovementDir;
	
	[HideInInspector]
	public float CurrentPathPercent;
	
	[Range(1, 10)]
	public float AnimationDuration = 1f;

	[CanBeNull] private List<Vector3> _waypoints;
	[CanBeNull] private GameObject _localParent;

	private float _currentPathPercent;
	private float _runningTime;
	private bool _reverseAnim;
	private ArchetypeMove _parentMove;
	
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
		
		var localWaypoints = (from Transform tr in transform where tr.tag == "Waypoint" select tr.position).ToList();

		waypoint.name = "Waypoint_" + localWaypoints.Count;
		waypoint.transform.position = localWaypoints.Count > 1 ? localWaypoints[localWaypoints.Count - 2] : transform.position;
		
		Selection.activeGameObject = waypoint;
		
		Undo.RegisterCreatedObjectUndo(waypoint, "Waypoint Added");

	}

	public bool HasWaypoints()
	{
		var length = transform.Cast<Transform>().Count(tr => tr.tag == "Waypoint" && tr.gameObject.activeInHierarchy);

		return length > 0;
	}

	protected void SetupWaypoints()
	{

		_waypoints = new List<Vector3>();

		// Iterate through all transform children and pull out any waypoints
		foreach(Transform tr in transform)
		{
			if(tr.tag == "Waypoint" && tr.gameObject.activeInHierarchy)
				_waypoints.Add(tr.position);
		}

		if(_waypoints.Count <= 0) return;
		
		// Make this object child of runtime-only parent to allow local path animation along with other x/y movement
		_localParent = new GameObject("Parent-"+gameObject.name);
		_localParent.transform.position = transform.position;

		// If archetype has parent, make runtime-only parent a child of it
		if(transform.parent != null)
		{
			_localParent.transform.parent = transform.parent;
			
			// Inherit parent's movement dir and, if enabled, speed
			if(_parentMove != null)
			{
				MovementDir = _parentMove.MovementDir;
				if(UseParentSpeed)
					MoveSpeed = _parentMove.MoveSpeed;
			}
		}

		transform.SetParent(_localParent.transform);
		transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
		
	}

	public void Awake()
	{
		if(transform.parent != null)
			_parentMove = transform.parent.GetComponent<ArchetypeMove>();
		
		if(GetType().Name != "ArchetypeSpawner")
			SetupWaypoints();
	
	}
	
	void OnDrawGizmos() {

			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(transform.position, Vector3.one);
		
			if(_waypoints != null && _waypoints.Count > 0)
				iTween.DrawPath(_waypoints.ToArray());
	
	}

	public void OnDrawGizmosSelected()
	{
		if(Application.isPlaying) return;
		
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
		
		var target = _localParent != null ? _localParent.transform.position : transform.position;
		Vector3 deltaPos = Vector3.zero;

		if(MovementDir == "up")
		{
			target.y += MoveSpeed;
			deltaPos.y += MoveSpeed;
		}
		else if(MovementDir == "right")
		{
			target.x += MoveSpeed;
			deltaPos.x += MoveSpeed;
		} 
		else if(MovementDir == "left")
		{
			target.x -= MoveSpeed;
			deltaPos.x -= MoveSpeed;
		} else
		{
			target.y -= MoveSpeed;
			deltaPos.y -= MoveSpeed;
		}


		if(MoveEnabled && MoveSpeed > 0)
		{
			if(_localParent != null)
				_localParent.transform.position = Vector3.Lerp(_localParent.transform.position, target, Time.deltaTime);
			else
				transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
		}

		if(_waypoints == null || _waypoints.Count <= 0) return;

		// Translate waypoints
		if(MoveEnabled)
		{
			for(var w = 0; w < _waypoints.Count; w++)
			{
				var v = Vector3.Lerp(_waypoints[w], _waypoints[w] + deltaPos, Time.deltaTime);
				_waypoints[w] = v;
			}
		}

		// Calculate current percentage on waypoints path (basically ping pong but time, not frame, based)
		_runningTime += Time.deltaTime;
		var perClamp = Mathf.Clamp(_runningTime / AnimationDuration, 0, 1f);
		
		//- Forward motion?
		if(!_reverseAnim)
		{
			_currentPathPercent = perClamp;
			if(_currentPathPercent >= 1)
			{
				_runningTime = 0;
				_reverseAnim = true;
			}
		} 
		else
		{
			_currentPathPercent = 1 - perClamp;
			if(_currentPathPercent <= 0)
			{
				_runningTime = 0;
				_reverseAnim = false;
			}
		}

		// Place object at current %
		iTween.PutOnPath(gameObject, _waypoints.ToArray(), _currentPathPercent);
		
	}

}