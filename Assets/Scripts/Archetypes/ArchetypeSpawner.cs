﻿/* 

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

public class ArchetypeSpawner : ArchetypeMove
{

	public GameObject[] PrefabsToSpawn;
	public Sprite SpriteAfterSpawn;
	
	[Tooltip("Should spawner object continue to move after spawning prefab?")]
	public bool MoveAfterSpawn;

	public bool SpawnSelf;
	
	[HideInInspector]
	public bool UseSpawnerParent = true;

	public bool SpawnRepeating;
	[HideInInspector]
	public int SpawnRepeatCount;
	[HideInInspector]
	public float SpawnDelay;
	[HideInInspector]
	public float SpawnRepeatDelay;
	
	private float _spawnWaitTime;
	private int _prefabIndex;
	private int _spawnCount;
	private bool _spriteReplaced;
	private bool _wait = true;

	private GameObject _spawnObject;

	// Update is called once per frame
	private void Update () {
		
		if(MoveEnabled)
			base.Update();
		
		if(!_wait) return;
		if(!(MainCamera.WorldToViewportPoint(transform.position).y < 1) || PrefabToSpawn == null) return;
	
		// If not repeating, spawn and destroy now
		if(!SpawnRepeating)
			StartCoroutine(DelayedSpawn());
		else
		{
			// Don't "wait" for spawner from here on out until destroy so we invoke only once
			_wait = false;
			InvokeRepeating("Spawn", SpawnDelay, SpawnRepeatDelay);
		}

	}
	
/*	#if UNITY_EDITOR

	private void OnDrawGizmos() {
		
		if(PrefabsToSpawn == null || PrefabsToSpawn.Length < 1) return;

		Gizmos.DrawGUITexture();
	}
	
	#endif*/

	private IEnumerator DelayedSpawn()
	{
		yield return new WaitForSeconds(SpawnDelay);
		
		Spawn();
	}

	private void Spawn()
	{
		// WILL REFACTOR -- OLD		
		// Debug.Log(gameObject.name);
		// if (SpawnSelf)
		// {
		// 	_spawnObject = gameObject;
		// 	_spawnObject.GetComponent<SpriteRenderer>().enabled = true;

		// 	if (gameObject.tag == "Wizard")
		// 		gameObject.GetComponent<ArchetypeWizard>().spawned = true;
		// }
		// else
		// {
		// 	_spawnObject = Instantiate(PrefabToSpawn, transform.localPosition, PrefabToSpawn.transform.rotation);
		// 	_spawnObject.SetActive(true);
		// }
	
		var spawn = Instantiate(
															PrefabsToSpawn[_prefabIndex], 
															UseSpawnerParent ? transform.localPosition : transform.position, 
															PrefabsToSpawn[_prefabIndex].transform.rotation
														);	

		// Increment or reset index
		if(_prefabIndex < PrefabsToSpawn.Length - 1)
			_prefabIndex++;
		else
			_prefabIndex = 0;
		
		spawn.SetActive(true);

		if(!MoveAfterSpawn)
		{
			Vector3 globalPos = MainCamera.transform.InverseTransformPoint(transform.position);
			globalPos.z = 0;

			transform.parent = null;
			transform.position = globalPos;

			MoveEnabled = false;
			SetupWaypoints();
		}
		else
		{
			// Give spawn parent of spawner if enabled
			if(UseSpawnerParent)
				_spawnObject.transform.SetParent(transform.parent, false);
		}

		// If not repeating and not replacing sprite, destroy now
		if(!SpawnRepeating && SpriteAfterSpawn == null)
		{
			Destroy(gameObject);
			return;
		} 
		// Replace sprite?
		if(SpriteAfterSpawn != null && !_spriteReplaced)
		{
			GetComponent<SpriteRenderer>().sprite = SpriteAfterSpawn;
			_spriteReplaced = true;

		}
		_spawnCount++;

		if(_spawnCount >= SpawnRepeatCount && SpriteAfterSpawn == null)
		{
			CancelInvoke();
			Destroy(gameObject);
			return;
		}
		
		// Destroy once well past camera bounds
		if(MainCamera.WorldToViewportPoint(transform.position).y < -1.2f)
			Destroy(gameObject);
		
	}
}
