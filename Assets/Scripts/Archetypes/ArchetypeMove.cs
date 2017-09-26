/*

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeMove.cs
	Archetype class for which all moving non-player objects use or inherit.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/ArchetypeMove.cs

	Created by Johnny Richardson, Erica Salling.
==============

*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeMove : MonoBehaviour
{

	public bool MoveEnabled = true;
	public PowerUps powerUpGiven;

	[HideInInspector]
	public float MoveSpeed = 1;
	[HideInInspector]
	public Dirs MovementDir = Dirs.Down;
	
	[HideInInspector]
	public float AnimationDuration = 1;
	[HideInInspector]
	public float AnimationForwardSpeed = 1;	
	[HideInInspector]
	public float AnimationReverseSpeed = 1;	
	[HideInInspector]
	public AnimType AnimationType = AnimType.PingPong;
	
	[HideInInspector]
	public bool UseParentSpeed;
	[HideInInspector]
	public int SpawnTypeIndex;
	[HideInInspector]
	public int Direction;
	[HideInInspector]
	public float CurrentPathPercent;
	
	[HideInInspector]
	public bool IsDestroyed;
	
	[CanBeNull] [HideInInspector]
	public string SpawnType;

	public GameObject[] powerUps;
	
	public enum Dirs
	{
		Left,
		Right,
		Up,
		Down
	}
	
	public enum AnimType
	{
		Once,
		LoopFromStart,
		PingPong
	}

	[CanBeNull] private List<Transform> _waypoints;
	[CanBeNull] private Transform waypointsParent;
	[CanBeNull] private GameObject _localParent;

	private float _currentPathPercent;
	private float _runningTime;
	private bool _reverseAnim;
	private float _reversingAngle;
	private Camera _mainCamera;
	private ArchetypeMove _parentMove;
	private Transform _movingTransform;
	private RectTransform _bgRectTransform;

	/**************
		UNITY METHODS
	***************/

	public void Awake()
	{
		Events.instance.AddListener<PowerUpEvent> (OnPowerUpEvent);

		// For use in Update
		_movingTransform = transform;
		_mainCamera = Camera.main;
		
		if(transform.parent != null)
			_parentMove = transform.parent.GetComponent<ArchetypeMove>();
		
		if(GetType().Name != "ArchetypeSpawner")
			SetupWaypoints();
		
		// Is background object?
		if(gameObject.layer == 8)
			_bgRectTransform = gameObject.GetComponentInChildren<RectTransform>();

	}
	
	public void Update () {
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.touches.Length == 0) return;
		#endif

		// Sanity check
		if (!_movingTransform)
			return;

		// Not for background layers
		if(gameObject.layer != 8 && _mainCamera.WorldToViewportPoint(_movingTransform.position).y < -1)
			Destroy(gameObject);
		
		else if(gameObject.layer == 8)
		{
			Vector3[] bgCorners = new Vector3[4];
			_bgRectTransform.GetWorldCorners(bgCorners);
			float cameraTop = _mainCamera.WorldToViewportPoint(bgCorners[1]).y;
			
			// Don't move background layer once there is none left (point of top-right coord is less than 1 relative to viewport)
			if(cameraTop < 1)
				return;
		}

		// Find target for movement and change target vector based on direction
		var target = _movingTransform.position;
		var deltaPos = Vector3.zero;

		switch(MovementDir)
		{
			case Dirs.Up:
				target.y += MoveSpeed;
				deltaPos.y += MoveSpeed;
				break;
			case Dirs.Right:
				target.x += MoveSpeed;
				deltaPos.x += MoveSpeed;
				break;
			case Dirs.Left:
				target.x -= MoveSpeed;
				deltaPos.x -= MoveSpeed;
				break;
			case Dirs.Down:
				target.y -= MoveSpeed;
				deltaPos.y -= MoveSpeed;
				break;
			default:
				throw new Exception("Unknown movement direction.");
		}

		// Move to target via lerp if movement allowed
		if(MoveEnabled && MoveSpeed > 0)
			_movingTransform.position = Vector3.Lerp(_movingTransform.position, target, Time.deltaTime);
		
		Animate();
		
	}
	
  public void OnTriggerEnter(Collider collider)
  {
	  if(collider.gameObject.GetComponent<ArchetypeMove>() == null) return;
	  
	  switch(gameObject.tag)
	  {
		  case "Player":
			  // Check if player hit a fly, poop, or villager. 
			  var die = collider.gameObject.tag == "Fly" || collider.gameObject.tag == "Poop" || collider.gameObject.tag == "Villager";

			  if (die && !gameObject.GetComponent<ArchetypePlayer>().WonGame)
			  {
				  Debug.Log("Game Over, you died.");

				  Events.instance.Raise (new DeathEvent(false));
				
			  }
			  break;
		  case "Bubble":
			  // Events.instance.Raise (new HitEvent(HitEvent.Type.Spawn, collider, gameObject));  
			  switch(collider.gameObject.tag)
			  {
				  case "Fly":
					  Debug.Log("The Player shot a Fly! It should die!");

					  Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Fly));	
					  Destroy(collider.gameObject);
					  GameConfig.fliesCaught++;

					  PowerUp(collider.gameObject.transform.position);
					  break;
				  case "Poop":
					  Debug.Log("The Player shot a Poop! Nothing happens.");
					  break;
			  }

			  break;
	  }
  }

	 
	#if UNITY_EDITOR
	public void OnDrawGizmosSelected()
	{
		if(!SceneEditor.ShowGizmos || Application.isPlaying) return;
		
		if(transform.parent == null)
		{
			var lineDir = transform.position;
			var lookDir = transform.rotation;
			Handles.color = new Color(.81176f, .4352f, 1);

			if(MovementDir == Dirs.Left)
			{
				lineDir += new Vector3(3, 0, 0);
				lookDir *= Quaternion.LookRotation(Vector3.left);
			}
			else if(MovementDir == Dirs.Right)
			{
				lineDir += new Vector3(-3, 0, 0);
				lookDir *= Quaternion.LookRotation(Vector3.right);
			}
			else if(MovementDir == Dirs.Up)
			{
				lineDir += new Vector3(0, 3, 0);
				lookDir *= Quaternion.LookRotation(Vector3.up);
			}
			else
			{
				lineDir += new Vector3(0, -3, 0);
				lookDir *= Quaternion.LookRotation(Vector3.down);
			}

			Handles.DrawDottedLine(transform.position, lineDir, 5);
			Handles.ArrowHandleCap(0, lineDir, lookDir, 15, EventType.Repaint);
			
		}
		
		var waypointChildren = new List<Transform>();
		
		foreach(Transform tr in transform)
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

	private void OnDrawGizmos() {
		
		if(!SceneEditor.ShowGizmos || Application.isPlaying) return;

		Gizmos.color = Color.cyan;
		
		if(GetComponent<ArchetypeSpawner>() != null)
			Gizmos.DrawSphere(transform.position, .5f);
		else
			Gizmos.DrawCube(transform.position, Vector3.one);
	
		if(_waypoints != null && _waypoints.Count > 0)
			iTween.DrawPath(_waypoints.ToArray());
	
	}

	private void OnDestroy() {
		
		Events.instance.RemoveListener<PowerUpEvent> (OnPowerUpEvent);

	}
	#endif

	/**************
		CUSTOM METHODS
	***************/
	
	// Add gameobject of type Waypoint as child of this archetype; used in editor only
	public void AddWaypoint()
	{
		
		#if UNITY_EDITOR
		
		var waypointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Waypoint.prefab");
		var waypoint = Instantiate(waypointPrefab, Vector3.zero, Quaternion.identity);

		waypoint.transform.parent = transform;
		
		// Find any current waypoint children
		var localWaypoints = (from Transform tr in transform where tr.tag == "Waypoint" select tr.position).ToList();

		waypoint.name = "Waypoint_" + localWaypoints.Count;
		waypoint.transform.position = localWaypoints.Count > 1 ? localWaypoints[localWaypoints.Count - 2] : transform.position;
		
		Selection.activeGameObject = waypoint;
		
		Undo.RegisterCreatedObjectUndo(waypoint, "Waypoint Added");
		
		#endif

	}

	// Does this object have any waypoints attached?
	public bool HasWaypoints()
	{
		var length = transform.Cast<Transform>().Count(tr => tr.tag == "Waypoint" && tr.gameObject.activeInHierarchy) + 
		             (from Transform tr in transform where tr.tag == "WaypointsPattern" && tr.gameObject.activeInHierarchy from Transform wp in tr select wp).Count();

		return length > 0;
	}

	// Setup waypoints for use of animation at runtime
	protected void SetupWaypoints()
	{

		_waypoints = new List<Transform>();
		var waypointTransforms = new List<Transform>();
		waypointsParent = new GameObject("Waypoints").transform;
		
		foreach(Transform tr in transform)
		{
			if(tr.tag == "WaypointsPattern" && tr.gameObject.activeInHierarchy)
			{
				foreach(Transform wp in tr)
				{
					waypointTransforms.Add(wp);
					
					// Assign waypoint to parent
					wp.parent = waypointsParent;
				}

			}
			else
				waypointTransforms.Add(tr);
		}

		// Iterate through all transform children or waypoint prefab patterns and pull out any waypoints
		foreach(var tr in waypointTransforms)
		{
			if(tr.tag != "Waypoint" || !tr.gameObject.activeInHierarchy) continue;
			_waypoints.Add(tr);

			// Assign waypoint to parent
			tr.parent = waypointsParent;
		}

		if(_waypoints.Count <= 0)
		{
			Destroy(waypointsParent.gameObject);
			return;
		}
		
		// Make this object child of runtime-only parent to allow local path animation along with other x/y movement
		_localParent = new GameObject("Parent-"+gameObject.name);
		_localParent.transform.position = transform.position;

		_movingTransform = _localParent.transform;

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
		
		if(waypointsParent != null) waypointsParent.parent = _localParent.transform;

		transform.SetParent(_localParent.transform);
		transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
		
	}
	
	public Vector3 ClampToScreen(Vector3 vector) {

		var pos = Camera.main.ScreenToWorldPoint(vector);
		pos.z = 0;

		return pos;

	}

	private void Animate()
	{
	
		if(_waypoints == null || _waypoints.Count <= 0) return;
		
		// Calculate current percentage on waypoints path (basically ping pong but time, not frame, based)
		_runningTime += Time.deltaTime * (_reverseAnim ? AnimationReverseSpeed : AnimationForwardSpeed);
		var perClamp = Mathf.Clamp(_runningTime / AnimationDuration, 0, 1);

		//- Forward motion?
		if(!_reverseAnim)
		{
			if(_reversingAngle > 0)
				_reversingAngle -= 10;
				
			_currentPathPercent = perClamp;
			
			if(_currentPathPercent >= 1)
			{
				// Reset if not animating once
				if(AnimationType != AnimType.Once)
				{
					_runningTime = 0;

					// Go into reverse if ping ponging
					if(AnimationType == AnimType.PingPong)
						_reverseAnim = true;
					else
						_currentPathPercent = 0;
				}
			}
		}
		//- Reverse motion?
		else
		{
			
			_currentPathPercent = 1 - perClamp;

			// Added 180° when reversing
			if(_reversingAngle < 180)
				_reversingAngle += 10;
			
			if(_currentPathPercent <= 0)
			{
				_runningTime = 0;
				_reversingAngle = 0;
				_reverseAnim = false;
			}
			
		}

		// Place object at current %
		iTween.PutOnPath(gameObject, _waypoints.ToArray(), _currentPathPercent);
		var arrWaypoints = _waypoints.ToArray();
		
		var lookDelta = iTween.PointOnPath(arrWaypoints, _currentPathPercent + 0.05f) - transform.position;
		var angle =  -Mathf.Atan2(lookDelta.x, lookDelta.y) * Mathf.Rad2Deg;
	
		var newRotation = Quaternion.Euler(0f, 0f, angle+_reversingAngle);

		transform.rotation = newRotation;

	}
	
	private IEnumerator RemoveVillager()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
	
	private IEnumerator PowerUpMatrixMode()
	{
		GUIManager.Instance.DisplayCurrentPowerUp("Slow Enemies");
		MoveSpeed /= 2;
		
		yield return new WaitForSeconds(5);
		
		MoveSpeed *= 2;
		GUIManager.Instance.HidePowerUp();
	}
	
	private void OnPowerUpEvent(PowerUpEvent e)
	{
		
		// What kinda power up? 
		switch(e.powerType)
		{
			
			case PowerUps.Matrix:
				// Slow down the whole world except the player
				StartCoroutine(PowerUpMatrixMode());
				break;
				
		}
		
	}

	private void PowerUp(Vector3 location) {
		// Check random to see if power up is dropped
		// if (Random.Range(0.0f, 10.0f) <= 5.0f) {
			// TO DO: Check the level to determine the power up

			var powerUp = GameObject.FindWithTag("Player").GetComponent<ArchetypeMove>().powerUps[0];

			// Drop that power up
			Instantiate(powerUp, location, Quaternion.identity);
		// }

	}

	protected void SpawnSpellComponent()
	{
		
		var neededCt = Inventory.instance.SpellComponentsNeeded.Count;
		var spellObject = Instantiate(Resources.Load("SpellObject") as GameObject, transform.position, Quaternion.identity);
		
		var comp = Inventory.instance.SpellComponentsNeeded[Random.Range(0, neededCt)];
		spellObject.GetComponent<SpellObject>().SelectComponent(comp);

	}

	

}