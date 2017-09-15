using System;
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

	private Vector3[] movements = new Vector3[4];

	private void RemoveVillager()
  {
      Destroy(gameObject);
  }

	public void BubbleHitEvent(Transform t, GameObject bubble) {

	}

	// Use this for initialization
	private void Awake () {
		
		base.Awake();

	}
	
	// Update is called once per frame
	private void Update () {
		
		base.Update();

	}

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Bubble") return;

		placeholderIndex++;

		Events.instance.Raise (new HitEvent(HitEvent.Type.Spawn, collider, collider.gameObject));

		Vector2 v = healthFill.rectTransform.sizeDelta;
		v.x += .5f;
		healthFill.rectTransform.sizeDelta = v;

		if(!(Mathf.Abs(v.x - health) <= .1f)) return;
		
		iTween.ScaleTo(collider.gameObject, Vector3.zero, 1.0f);
		Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Good));
		
		RemoveVillager();

		IsDestroyed = true;
		GameConfig.peopleSaved++;

		SpawnSpellComponent();


	}

}