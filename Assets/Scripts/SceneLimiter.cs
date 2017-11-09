using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLimiter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log(other.name);
		
		var archetypeMove = other.GetComponent<ArchetypeMove>();
		if(archetypeMove != null)
			Destroy(other.gameObject);
	}
}
