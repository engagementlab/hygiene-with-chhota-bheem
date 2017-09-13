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

	public GameObject prefabToSpawn;
	
	[Tooltip("Should spawner object continue to move after spawning prefab?")]
	public bool moveAfterSpawn;
	
	[HideInInspector]
	public bool isDestroyed;
	[HideInInspector]
	public string spawnType;

	private Camera mainCamera;
	private bool wait = true;

	void Start()
	{
		mainCamera = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		
		if(!MoveEnabled || !wait)
			return;
		
		base.Update();

		if(!(mainCamera.WorldToViewportPoint(transform.position).y < 1) || prefabToSpawn == null) return;
		var spawn = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

		spawn.SetActive(true);
		wait = false;

		if(!moveAfterSpawn)
		{
			Vector3 globalPos = mainCamera.transform.InverseTransformPoint(transform.position);
			transform.parent = null;
			transform.position = globalPos;
			SetupWaypoints();
		}
			
		Destroy(gameObject);
	}
}
