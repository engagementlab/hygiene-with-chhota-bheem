using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class VillagerObject : ArchetypeMove {
	
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

	}
	
	// Update is called once per frame
	void Update () {
		
		base.Update();

	}

	void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Bubble") return;
		
		Debug.Log("The Player shot a Villager! It should lose life!");

		placeholderIndex++;

		Events.instance.Raise (new HitEvent(HitEvent.Type.Spawn, collider, collider.gameObject));

		Vector2 v = healthFill.rectTransform.sizeDelta;
		v.x += .5f;
		healthFill.rectTransform.sizeDelta = v;
		
		if(Mathf.Abs(v.x - health) < .1f) {

			iTween.ScaleTo(collider.gameObject, Vector3.zero, 1.0f);
			Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Good));	
			StartCoroutine(RemoveVillager());

			IsDestroyed = true;
			GameConfig.peopleSaved++;

			Events.instance.Raise(new PowerUpEvent(powerUpGiven));

		}
	}

}