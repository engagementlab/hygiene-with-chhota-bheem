using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArchetypeShooting : MonoBehaviour {
	
	public GameObject bubble;

	public Image meterImage; 
	public float bubbleSpeed;

	// public float shootInterval = 20;
	public bool isStatic;

	float intervalTime = 0;

	bool reloading;

	void Update () {

		// if(reloading)
		// 	return;

		if(intervalTime >= GameConfig.numBubblesInterval) {

			if(isStatic)
			{
				if(meterImage.fillAmount == 0)
					return;

				float yPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

				if(yPos < -3.9f)
					return;
			}

			intervalTime = 0;
			Vector2 dir;

		 	float xPos = isStatic ? Camera.main.ScreenToWorldPoint(Input.mousePosition).x : transform.position.x;

			if(isStatic)
			{
				Vector2 touchPos = new Vector2(xPos, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				dir = touchPos - (new Vector2(transform.position.x, transform.position.y));
			} 
			else
			{
				Vector2 endPos = new Vector2(transform.position.x, 0);
				dir = new Vector2(0, 1);
			}
			
			dir.Normalize();
			
			GameObject projectile = Instantiate (bubble, transform.position, Quaternion.identity) as GameObject;
			projectile.GetComponent<Rigidbody> ().velocity = dir * bubbleSpeed; 
			
		}
		else
			intervalTime += Time.deltaTime;

	}
	
  void OnMouseEnter() {

  	reloading = true;

	  if(meterImage != null) {
	  	if(meterImage.fillAmount < 1)
	  		meterImage.fillAmount += (meterImage.fillAmount / GameConfig.numBubblesFull);
		}

  }

  void OnMouseExit() {

	  if(meterImage != null) {
	  	if(meterImage.fillAmount < 1)
	  		meterImage.fillAmount += (meterImage.fillAmount / GameConfig.numBubblesFull);
		}

  	reloading = false;

  }
      
	void OnTriggerEnter(Collider other)
  {

	  if(other.gameObject.tag == "Spawner") {
	  	
	  }

  }

	void OnTriggerExit(Collider other)
  {
		reloading = false;
  }

}
