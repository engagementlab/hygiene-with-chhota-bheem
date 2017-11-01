/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeSpawner.cs
	Archetype object that spawns other archetype once entering camera; inherits ArchetypeMove.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Editor/ArchetypeSpawner.cs

	Created by Johnny Richardson.
==============

*/

using System.Collections;
using UnityEngine;

public class ArchetypeSpawner : MonoBehaviour
{

	public SpawnerPrefab[] SpawnedObjects;
	public Sprite SpriteAfterSpawn;
	
	[Tooltip("Should spawner object continue to move after spawning prefab?")]
	public bool MoveAfterSpawn = true;
	
	[HideInInspector]
	public bool UseSpawnerParent = true;

	public bool SpawnRepeating;
	[HideInInspector]
	public int SpawnRepeatCount;
	[HideInInspector]
	public float SpawnDelay;
	
	private float _spawnWaitTime;
	private int _prefabIndex = -1;
	private int _spawnCount;
	private bool _spriteReplaced;
	private bool _wait = true;
	
	private SpriteRenderer _tempRenderer;
	private GameObject _spawnObject;
	private Material _gizmoMaterial;
	
	private Camera MainCamera;

	private void Awake()
	{
		
		MainCamera = Camera.main;

		if(SpriteAfterSpawn != null && GetComponent<SpriteRenderer>() == null)
			gameObject.AddComponent<SpriteRenderer>();
		else
		{
			if(GetComponent<SpriteRenderer>() == null)
				gameObject.AddComponent<SpriteRenderer>();
	
			gameObject.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Simple;
			gameObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("SpriteDefaultMaterial");
		}
	}

	// Update is called once per frame
	public void Update () {
		
		if(!_wait) return;
		if(!(MainCamera.WorldToViewportPoint(transform.position).y < 1) || SpawnedObjects == null) return;
	
		// If not repeating, spawn and destroy now
		if(!SpawnRepeating)
		{
			_wait = false;
			StartCoroutine(DelayedSpawn());
		} 
		else
		{
			// Increment spawn wait
			_spawnWaitTime += Time.deltaTime;

			// Initial spawn
			if(_prefabIndex == -1 && _spawnWaitTime < SpawnDelay)
			{
				_spawnCount = 0;
				return;
			}

			// Waiting for next prefab in loop
			else if(_prefabIndex >= 0 && _spawnWaitTime < SpawnedObjects[_prefabIndex].DelayBeforeNext)
				return;
			
			if(SpawnRepeating && _prefabIndex == SpawnedObjects.Length - 1)
			{
				_spawnCount++;
				_prefabIndex = -1;
			}
			
			// Reset time and spawn
			_spawnWaitTime = 0;
			Spawn();
		}

	}
	
#if UNITY_EDITOR

	// Display sprite of first prefab spawn as "ghost" to help w/ layout
	private void OnDrawGizmos()
	{

		if(SpawnedObjects == null || SpawnedObjects.Length < 1 || SpawnedObjects[0] == null ||
		   SpawnedObjects[0].Prefab == null || Application.isPlaying) return;

		if(_gizmoMaterial == null)
			_gizmoMaterial = Resources.Load<Material>("GizmoGreyMaterial");

		var sprite = SpawnedObjects[0].Prefab.GetComponent<SpriteRenderer>();
		
		if(sprite == null) return;

		if(gameObject.GetComponent<SpriteRenderer>() == null)
			gameObject.AddComponent<SpriteRenderer>();

		_tempRenderer = gameObject.GetComponent<SpriteRenderer>();

		// Switch to gizmo material
		if(_tempRenderer.sharedMaterial != null)
			_tempRenderer.sharedMaterial = _gizmoMaterial;

		if(_tempRenderer.sprite == null)
			_tempRenderer.sprite = sprite.sprite;
		
	}
	
#endif

	private IEnumerator DelayedSpawn()
	{
		yield return new WaitForSeconds(SpawnDelay);
		
		Spawn();
	}

	private void Spawn()
	{

		if(SpawnedObjects == null || SpawnedObjects.Length == 0)
		{
			Debug.LogWarning("No prefabs to spawn from " + gameObject.name + "!!");
			return;
		}

		// Increment index
		if(_prefabIndex < SpawnedObjects.Length - 1)
			_prefabIndex++;
			
		if(SpawnedObjects[_prefabIndex].Prefab == null) return;
		var spawnPos = SpawnedObjects[_prefabIndex].UseSpawnerParent ? transform.localPosition : transform.position;

		_spawnObject = Instantiate(SpawnedObjects[_prefabIndex].Prefab, spawnPos, SpawnedObjects[_prefabIndex].Prefab.transform.rotation);
		_spawnObject.SetActive(true);

		// Destroy once well past camera bounds
		if (MainCamera.WorldToViewportPoint(transform.position).y < -1.2f)
			Destroy(gameObject);

		if (!MoveAfterSpawn)
		{
			Vector3 globalPos = MainCamera.transform.InverseTransformPoint(transform.position);
			globalPos.z = 0;

			transform.parent = null;
			transform.position = globalPos;
		}
		else
		{
			// Give spawn parent of spawner if enabled
			if (SpawnedObjects[_prefabIndex].UseSpawnerParent)
				_spawnObject.transform.SetParent(transform.parent, false);
		}

		if (_spawnCount >= SpawnRepeatCount || !SpawnRepeating)
		{
			// Replace sprite?
			if (SpriteAfterSpawn != null)
			{
				GetComponent<SpriteRenderer>().sprite = SpriteAfterSpawn;
				_spriteReplaced = true;
			}
		 	else
				Destroy(gameObject);
		}
		
	}
}