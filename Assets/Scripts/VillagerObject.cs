using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class VillagerObject : ArchetypeSpawner {
	
	public Canvas healthCanvas;
	public RawImage healthBg;
	public RawImage healthFill;

	public int placeholderIndex = 0;
	public float health = 2;

	Vector3[] movements = new Vector3[4];

  IEnumerator RemoveVillager()
  {
      yield return new WaitForSeconds(1);
      Destroy(gameObject);
  }

	public void BubbleHitEvent(Transform t, GameObject bubble) {

	}

	// Use this for initialization
	void Awake () {
		
		base.Awake();

		// if(wizardMode) {

		// 	movements[0] = transform.localPosition;
		// 	movements[1] = new Vector3(Random.Range(transform.position.x-15, transform.position.x+15), transform.position.y, 0);
		// 	movements[2] = new Vector3(transform.position.x, Random.Range(transform.position.y-15, transform.position.y+15), 0);
		// 	movements[3] = new Vector3(Random.Range(transform.position.x-15, transform.position.x+15), transform.position.y, 0);
	   
		// 	iTween.MoveTo(gameObject, iTween.Hash("path", movements, "islocal", true, "time", Random.Range(5, 10), "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutSine));
		// }

	}
	
	// Update is called once per frame
	void Update () {
		
		base.Update();

	}

	void OnTriggerEnter(Collider collider) {

		if(healthCanvas != null && !healthCanvas.gameObject.activeSelf)
			return;

  	if(collider.gameObject.GetComponent<ArchetypeSpawner>() != null) {
	
				if(health < 5) {
					Vector2 bgSize = healthBg.rectTransform.sizeDelta;
		  		health += .5f;
					bgSize.x = health;
					healthBg.rectTransform.sizeDelta = bgSize;
				}

  		return;
  	}
		
		if(spawnType != "villager")
			return;

		if(collider.tag != "Bubble")
			return;

		placeholderIndex++;

		Events.instance.Raise (new HitEvent(HitEvent.Type.PowerUp, collider, collider.gameObject));

		Vector2 v = healthFill.rectTransform.sizeDelta;
		v.x += .5f;
		healthFill.rectTransform.sizeDelta = v;

		if(v.x == health) {

			iTween.ScaleTo(gameObject, Vector3.zero, 1.0f);
			Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Good));	
			StartCoroutine(RemoveVillager());

			isDestroyed = true;
			GameConfig.peopleSaved++;
		}

	}

}