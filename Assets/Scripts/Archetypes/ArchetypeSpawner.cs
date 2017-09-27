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

	public bool SpawnRepeating;
	
	[HideInInspector]
	public int SpawnRepeatCount;
	[HideInInspector]
	public float SpawnDelay;
	
	private float _spawnWaitTime;
	private int _spawnCount;
	private bool _wait = true;

	// Update is called once per frame
	private void Update () {
		
		if(!MoveEnabled || !_wait)
			return;
		
		base.Update();

		if(!(_mainCamera.WorldToViewportPoint(transform.position).y < 1) || PrefabToSpawn == null) return;

		// If not repeating, spawn and destroy now
		if(!SpawnRepeating)
		{
			Spawn();
			Destroy(gameObject);
		}
		else
			InvokeRepeating("Spawn", 0, SpawnDelay);

		_wait = false;
		
	}

	private void Spawn()
	{
	
		var spawn = Instantiate(PrefabToSpawn, transform.position, Quaternion.identity);	
		spawn.SetActive(true);
		
		if(!MoveAfterSpawn)
		{
			Vector3 globalPos = _mainCamera.transform.InverseTransformPoint(transform.position);
			globalPos.z = 0;
			
			transform.parent = null;
			transform.position = globalPos;
			
			SetupWaypoints();
		}

		if(!SpawnRepeating) return;
		_spawnCount++;

		if(_spawnCount == SpawnRepeatCount)
		{
			CancelInvoke();
			Destroy(gameObject);
		}
		
	}
}
