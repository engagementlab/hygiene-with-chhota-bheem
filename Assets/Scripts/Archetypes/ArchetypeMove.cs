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
using System.Net.Configuration;
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
	public Spells SpellGiven;
	public bool SpellRandom;
	public bool KillsPlayer;
	
	private Spells _powerUpGiven;

	[HideInInspector]
	public float MoveSpeed;
	[HideInInspector]
	public Dirs MovementDir = Dirs.Down;
	
	[HideInInspector]
	public float AnimationDuration = 1;
	[HideInInspector]
	public float AnimationUpwardSpeed = 1;	
	[HideInInspector]
	public float AnimationDownwardSpeed = 1;	
	[HideInInspector]
	public AnimType AnimationType = AnimType.PingPong;
	
	[HideInInspector]
	public bool UseParentSpeed;
	[HideInInspector]
	public bool RotateOnWaypoints = true;
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

	public GameObject[] SpellJuices;
	
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
	[CanBeNull] private Transform _waypointsParent;
	[CanBeNull] private GameObject _localParent;

	private float _currentPathPercent;
	private float _runningTime;
	private bool _reverseAnim;
	private float _reversingAngle;
	private float _targetAnimSpeed;
	private int _nextPoint;
	internal Camera MainCamera;
	private ArchetypeMove _parentMove;
	private Transform _movingTransform;
	private RectTransform _bgRectTransform;
	private Queue<SplineSegment> _pathSegments;
	private Vector3 _lastPoint;
	private Vector3 _toPoint;
	private Vector3 _lerpPoint;
	private Transform[] _waypointPositions;
	private Quaternion _startingRotation;

	/**************
		UNITY METHODS
	***************/

	public void Awake()
	{
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);

		// For use in Update
		_movingTransform = transform;
		MainCamera = Camera.main;

		AnimationDuration = 10 / AnimationDuration;
		_targetAnimSpeed = AnimationDuration * AnimationUpwardSpeed;
		_startingRotation = transform.rotation;

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
		if(gameObject.layer != 8 && MainCamera.WorldToViewportPoint(_movingTransform.position).y < -1)
			Destroy(gameObject);
		
		else if(gameObject.layer == 8)
		{
			Vector3[] bgCorners = new Vector3[4];
			_bgRectTransform.GetWorldCorners(bgCorners);
			float cameraTop = MainCamera.WorldToViewportPoint(bgCorners[1]).y;
			
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
		
		if(_waypoints == null || _waypoints.Count <= 0) return;
		if(!RotateOnWaypoints) return;
		
		_lerpPoint = Vector3.Lerp(_lastPoint, _toPoint, Time.deltaTime);
		
		var lookDelta = _lerpPoint - transform.position;
		var angle =  -Mathf.Atan2(lookDelta.x, lookDelta.y) * Mathf.Rad2Deg;
		var newRotation = Quaternion.Euler(0f, 0f, angle);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 2);
		
		
	}
	
  public void OnTriggerEnter(Collider collider)
  {
	  
	  if(collider.tag == "Player")
	  {
		  // Check if player hit and object that ends game 
		  var die = KillsPlayer;

		  if(die && !collider.GetComponent<ArchetypePlayer>().WonGame && !collider.GetComponent<ArchetypePlayer>().PoweredUp)
			  Events.instance.Raise(new DeathEvent(false));
		  else if (die && collider.GetComponent<ArchetypePlayer>().PoweredUp)
			  Events.instance.Raise(new SpellEvent(collider.GetComponent<ArchetypePlayer>()._spellsType, false));		  
	  }

	  switch(collider.gameObject.tag)
	  {
		  case "Bubble":
			  switch(gameObject.tag)
			  {
				  case "Fly":

					  Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Fly));	
					  Destroy(collider.gameObject);
					  Destroy(gameObject);
					  GameConfig.fliesCaught++;

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
		
		Utilities.DrawWaypoints(transform);
	}

	private void OnDrawGizmos() {
		
		if(!SceneEditor.ShowGizmos || Application.isPlaying) return;

		Gizmos.color = Color.cyan;
	
		if(_waypoints != null && _waypoints.Count > 0)
			iTween.DrawPath(_waypoints.ToArray());
	
	}

	private void OnDestroy() {
		
		Events.instance.RemoveListener<SpellEvent> (OnSpellEvent);

	}

	private void OnDisable()
	{
	
		Events.instance.RemoveListener<SpellEvent> (OnSpellEvent);		
		
	}

	private void OnEnable()
	{
	
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);		
		
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
		_waypointsParent = new GameObject("Waypoints").transform;
		
		foreach(Transform tr in transform)
		{
			if(tr.tag == "WaypointsPattern" && tr.gameObject.activeInHierarchy)
			{
				foreach(Transform wp in tr)
					waypointTransforms.Add(wp);
			}
			
			waypointTransforms.Add(tr);
		}

		// Iterate through all transform children or waypoint prefab patterns and pull out any waypoints
		foreach(var tr in waypointTransforms)
		{
			if(tr.tag != "Waypoint" || !tr.gameObject.activeInHierarchy) continue;
			_waypoints.Add(tr);

			// Assign waypoint to parent
			tr.parent = _waypointsParent;
		}

		if(_waypoints.Count <= 0)
		{
			Destroy(_waypointsParent.gameObject);
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
		
		if(_waypointsParent != null) _waypointsParent.parent = _localParent.transform;

		transform.SetParent(_localParent.transform);
		transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

		_waypointPositions = _waypoints.ToArray();
		
		Animate();
		
	}
	
	private void Animate()
	{
	
		if(_waypoints == null || _waypoints.Count <= 0) return;
		
		// Calculate current percentage on waypoints path (basically ping pong but time, not frame, based)
		bool isDownward = transform.InverseTransformDirection(Vector3.up).y < 0;

		if(!isDownward)
			_targetAnimSpeed = AnimationDuration * AnimationUpwardSpeed;
		else
			_targetAnimSpeed = AnimationDuration * AnimationDownwardSpeed;
			
		// Place object at current %
		_lastPoint = transform.position;
		_toPoint = _waypointPositions[_nextPoint].position;	
		
		var distance = Vector3.Distance(_toPoint, transform.position);
		Debug.Log(distance/_targetAnimSpeed);
		iTween.MoveTo(gameObject, iTween.Hash("position", _toPoint, "time", distance/_targetAnimSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "Complete"));
	}

	void Complete()
	{

		if(!_reverseAnim)
		{
			
			// Reset if not animating once
			if(AnimationType != AnimType.Once)
			{
				// Go into reverse if ping ponging
				if(_nextPoint < _waypoints.Count-1)
					_nextPoint++;
				else
				{
					if(AnimationType == AnimType.PingPong)
					{
						_reverseAnim = true;
						_nextPoint--;
					} 
					else if(AnimationType == AnimType.LoopFromStart)
					{
						_nextPoint = 0;
						transform.position = _waypointPositions[0].position;
						transform.rotation = _startingRotation;
					}
				}
			} 
			else if(_nextPoint < _waypoints.Count-1)
				_nextPoint++;
			
		}
		else
		{
			if(_nextPoint > 0)
				_nextPoint--;
			else
			{
				_nextPoint++;
				_reverseAnim = false;
			}
		}

		Animate();
		
	}

	private IEnumerator SpellMatrixMode()
	{
		GuiManager.Instance.DisplayCurrentSpell("Slow Enemies");
		MoveSpeed /= 2;
		
		yield return new WaitForSeconds(5);
		
		MoveSpeed *= 2;
		GuiManager.Instance.HideSpell();
	}
	
	
	private void OnSpellEvent(SpellEvent e)
	{
		if (e.powerUp)
		{
			// Spell ON
			switch (e.powerType)
			{

				case Spells.Matrix:
					// Slow down the whole world except the player
					if (GameObject.FindWithTag("Player").GetComponent<ArchetypePlayer>().PowerInfinite)
					{
						GuiManager.Instance.DisplayCurrentSpell("Slow Enemies");
						MoveSpeed /= 2;
					}
					else
					{
						StartCoroutine(SpellMatrixMode());
					}
					
					break;

				default:

					break;

			}
		}
		else
		{
			// Spell OFF
			switch (e.powerType)
			{

				case Spells.Matrix:
					GuiManager.Instance.HideSpell();
					MoveSpeed *= 2;
					
					break;

				default:

					break;

			}
		}

	}

	protected void SpawnSpellComponent()
	{
		if (SpellRandom || SpellGiven == Spells.None)
		{
			// Randomly decide which spell for which to spawn juice
			_powerUpGiven = Enum.GetValues(typeof(Spells)).Cast<Spells>().ToList()[Random.Range(1, 3)];
		}
		else
		{
			// Use Publically Selected Spell
			_powerUpGiven = SpellGiven;
		}
		
		// Instantiate Spell Object as the random spell type
		var spellObject = Instantiate(Resources.Load("SpellObject") as GameObject, transform.position, Quaternion.identity);
		spellObject.GetComponent<ArchetypeSpellJuice>().Type = _powerUpGiven;

	}

}