using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchetypeProjectile : MonoBehaviour {

	public Sprite[] bubbleSprites;

	private Camera mainCamera;

	// Use this for initialization
	void Awake ()
	{

		mainCamera = Camera.main;

		GetComponent<SpriteRenderer>().sprite = bubbleSprites[Random.Range(0, 3)];
		iTween.ScaleFrom(gameObject, iTween.Hash("time", .3f, "scale", Vector3.zero, "easetype", iTween.EaseType.easeOutElastic));
		
	}
	
	// Update is called once per frame
	void Update () {
      

		if(mainCamera.WorldToViewportPoint(transform.position).y > 1)
			Destroy(gameObject);
		
	}

	void OnTriggerEnter(Collider other)
  {

  	Debug.Log(other);

	  if(other.gameObject.tag == "Spawner") {

	  	Destroy(other.gameObject);
	  	Destroy(gameObject);
	  	
	  }
	}

}
