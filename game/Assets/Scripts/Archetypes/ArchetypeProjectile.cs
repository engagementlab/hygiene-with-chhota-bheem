﻿using UnityEngine;

public class ArchetypeProjectile : MonoBehaviour {

	private Camera _mainCamera;

	public void Initialize(Vector3 scale, Vector3 velocity)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, Utilities.GetZPosition(gameObject));
		transform.localScale = scale;
		
		GetComponent<Rigidbody>().velocity = velocity;
		
		if (gameObject.GetComponent<SphereCollider>() == null)
			gameObject.AddComponent<SphereCollider>().isTrigger = true;
		else 
			gameObject.GetComponent<SphereCollider>().isTrigger = true;
		
		iTween.ScaleFrom(gameObject, iTween.Hash("time", .3f, "scale", Vector3.zero, "easetype", iTween.EaseType.easeOutElastic));
	}

	private void Start ()
	{

		_mainCamera = Camera.main;
		
	}
	
	// Update is called once per frame
	private void Update () {

		if(_mainCamera.WorldToViewportPoint(transform.position).y > 1)
			Destroy(gameObject);
		
	}

}
