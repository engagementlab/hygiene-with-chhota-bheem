using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine;

public class ArchetypeSpawner : ArchetypeMove
{

	public bool moveWithParent = true;

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
		
		if(mainCamera.WorldToViewportPoint(transform.position).y < 1) {
//			GameObject spawn = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

//			gameObject.SetActive(true);
			wait = false;

			if(!moveWithParent)
			{
				Vector3 globalPos = mainCamera.transform.InverseTransformPoint(transform.position);
				transform.parent = null;
				transform.position = globalPos;
				SetupWaypoints();
			}
			
//			Destroy(gameObject);
		}
		
	}
}
