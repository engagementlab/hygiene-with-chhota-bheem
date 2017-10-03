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
using UnityEngine;

public class ArchetypeSpawner : ArchetypeMove
{

	public GameObject PrefabToSpawn;
	
	[Tooltip("Should spawner object continue to move after spawning prefab?")]
	public bool MoveAfterSpawn;
	public bool UseSpawnerParent = true;

	public bool SpawnRepeating;
	
	[HideInInspector]
	public int SpawnRepeatCount;
	[HideInInspector]
	public float SpawnDelay;
	[HideInInspector]
	public float SpawnRepeatDelay;
	
	private float _spawnWaitTime;
	private int _spawnCount;
	private bool _wait = true;

	// Update is called once per frame
	private void Update () {
		
		if(MoveEnabled)
			base.Update();
		
		if(!_wait) return;
		if(!(_mainCamera.WorldToViewportPoint(transform.position).y < 1) || PrefabToSpawn == null) return;

		// If not repeating, spawn and destroy now
		if(!SpawnRepeating)
			Spawn();
		else {
			// Don't "wait" for spawner from here on out until destroy so we invoke only once
			_wait = false;
			InvokeRepeating("Spawn", SpawnDelay, SpawnRepeatDelay);
		}
		
	}

	private void Spawn()
	{
	
		var spawn = Instantiate(PrefabToSpawn, transform.localPosition, PrefabToSpawn.transform.rotation);	
		spawn.SetActive(true);
		
		// Give spawn parent of spawner if enabled
		if(UseSpawnerParent)
			spawn.transform.SetParent(transform.parent, false);

		if(!MoveAfterSpawn)
		{
			Vector3 globalPos = _mainCamera.transform.InverseTransformPoint(transform.position);
			globalPos.z = 0;
			
			transform.parent = null;
			transform.position = globalPos;
			
			SetupWaypoints();
		}

		// If not repeating, destroy now
		if(!SpawnRepeating)
		{
			Destroy(gameObject);
			return;
		}
		_spawnCount++;

		if(_spawnCount >= SpawnRepeatCount)
		{
			CancelInvoke();
			Destroy(gameObject);
		}
		
	}
}
