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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
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

	[CanBeNull] private List<Vector3> _waypoints;
	[CanBeNull] private GameObject _localParent;

	private float _currentPathPercent;
	private float _runningTime;
	private bool _reverseAnim;
	private ArchetypeMove _parentMove;

	/**************
		UNITY METHODS
	***************/

	public void Awake()
	{
		Events.instance.AddListener<PowerUpEvent> (OnPowerUpEvent);
		
		if(transform.parent != null)
			_parentMove = transform.parent.GetComponent<ArchetypeMove>();
		
		if(GetType().Name != "ArchetypeSpawner")
			SetupWaypoints();
	
	}
	
	public void Update () {
		
		#if IS_DEBUG
		if(!Input.GetMouseButton(0)) return;
		#endif
		
		// Find target for movement and change target vector based on direction
		var target = _localParent != null ? _localParent.transform.position : transform.position;
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
			default:
				target.y -= MoveSpeed;
				deltaPos.y -= MoveSpeed;
				break;
		}

		// Move to target via lerp if movement allowed
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
		
		Animate();
		
	}
	
  public void OnTriggerEnter(Collider collider) {
	  
  	if(collider.gameObject.GetComponent<ArchetypeMove>() != null) {

  		if (gameObject.tag == "Player") {
  			// Check if player hit a fly, poop, or villager. 

  			bool die = collider.gameObject.tag == "Fly" || collider.gameObject.tag == "Poop" || collider.gameObject.tag == "Villager";

			  if (die && !gameObject.GetComponent<ArchetypePlayer>().wonGame)
			  {
			  	Debug.Log("Game Over, you died.");
				  gameObject.SetActive(false);

				  gameObject.GetComponent<ArchetypePlayer>().gameOverText.SetActive(true);
			  }

			  if(collider.gameObject.tag == "PowerUp") {
			  	// powerUpType = collider.gameObject.name;
			  	// // What kinda power up? 
			  	// if (powerUpType == "PowerUpMatrix") {
			  	// 	// Slow down the whole world except the player

		  		// } else if (powerUpType == "PowerUpSpeedShoot") {
		  		// 	// Speed up bubble rate
		  		// 	gameObject.GetComponent<ArchetypeShooting>().bubbleSpeed = gameObject.GetComponent<ArchetypeShooting>().bubbleSpeed * 2;

		  		// } else if (powerUpType == "PowerUpScatterShoot") {
		  		// 	// Make those bubbles scatter

		  		// }

		  		// StartCoroutine(collider.gameObject.GetComponent<ArchetypePowerUp>().Timer(10, powerUpType));

			  	// Destroy(collider.gameObject);

			  	return;
			  }

  		} else if (gameObject.tag == "Bubble") {
	  		// if(!inBossBattle)
					// Destroy(gameObject);

				// Events.instance.Raise (new HitEvent(HitEvent.Type.Spawn, collider, gameObject));  
  			if (collider.gameObject.tag == "Fly") {
	  			Debug.Log("The Player shot a Fly! It should die!");

	  			Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Good));	
		  		Destroy(collider.gameObject);
		  		GameConfig.fliesCaught++;

		  		PowerUp(collider.gameObject.transform.position);

	  		}

	  		if (collider.gameObject.tag == "Poop") {
	  			Debug.Log("The Player shot a Poop! Nothing happens.");

	  		}
  		}


  	}
  }
	 
	#if UNITY_EDITOR
	public void OnDrawGizmosSelected()
	{
		if(Application.isPlaying) return;
		
		if(transform.parent == null)
		{
			Vector3 lineDir = transform.position;
			Quaternion lookDir = transform.rotation;
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
			Handles.ArrowHandleCap(0, lineDir, lookDir, 5, EventType.Repaint);
			
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

		_waypoints = new List<Vector3>();
		var waypointTransforms = new List<Transform>();
		
		foreach(Transform tr in transform)
		{
			if(tr.tag == "WaypointsPattern" && tr.gameObject.activeInHierarchy)
			{
				foreach(Transform wp in tr) 
					waypointTransforms.Add(wp);

			} 
			else
				waypointTransforms.Add(tr);
		}

		// Iterate through all transform children or waypoint prefab patterns and pull out any waypoints
		foreach(var tr in waypointTransforms)
		{
			if(tr.tag != "Waypoint" || !tr.gameObject.activeInHierarchy) continue;
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
	
	public Vector3 ClampToScreen(Vector3 vector) {

		var pos = Camera.main.ScreenToWorldPoint(vector);
		pos.z = 0;

		return pos;

	}

	private void Animate()
	{
	
		// Calculate current percentage on waypoints path (basically ping pong but time, not frame, based)
		_runningTime += Time.deltaTime * (_reverseAnim ? AnimationReverseSpeed : AnimationForwardSpeed);
		var perClamp = Mathf.Clamp(_runningTime / AnimationDuration, 0, 1);
		
		//- Forward motion?
		if(!_reverseAnim)
		{
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
			if(_currentPathPercent <= 0)
			{
				_runningTime = 0;
				_reverseAnim = false;
			}
		}

		// Place object at current %
		iTween.PutOnPath(gameObject, _waypoints.ToArray(), _currentPathPercent);
		
	}

	private IEnumerator RemoveVillager()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
	
	private IEnumerator PowerUpMatrixMode()
	{
		MoveSpeed /= 2;
		
		yield return new WaitForSeconds(5);
		
		MoveSpeed *= 2;
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

			GameObject powerUp = GameObject.FindWithTag("Player").GetComponent<ArchetypeMove>().powerUps[0];

			// Drop that power up
			Instantiate(powerUp, location, Quaternion.identity);
		// }

	}

	

}