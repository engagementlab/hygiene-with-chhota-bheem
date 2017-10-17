using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchetypeProjectile : MonoBehaviour {

	private Camera _mainCamera;

	// Use this for initialization
	private void Awake ()
	{

		_mainCamera = Camera.main;
		iTween.ScaleFrom(gameObject, iTween.Hash("time", .3f, "scale", Vector3.zero, "easetype", iTween.EaseType.easeOutElastic));
		
	}
	
	// Update is called once per frame
	private void Update () {
      

		if(_mainCamera.WorldToViewportPoint(transform.position).y > 1)
			Destroy(gameObject);
		
	}

}
