using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
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
		
		if(mainCamera.WorldToViewportPoint(transform.position).y < 1 && prefabToSpawn != null) {
			GameObject spawn = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

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
}
