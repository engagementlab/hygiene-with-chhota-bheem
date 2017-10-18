using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class VillagerObject : ArchetypeMove
{

	public ParticleSystem Particles;
	public RawImage healthFill;

	public int placeholderIndex = 0;
	public float health = 2f;

	private Camera mainCamera;
	private Vector3[] movements = new Vector3[4];

	private IEnumerator RemoveVillager()
	{
		yield return new WaitForSeconds(1);
	    Destroy(gameObject);
    }

	// Use this for initialization
	private void Awake () {
		
		base.Awake();
		mainCamera = Camera.main;

	}
	
	// Update is called once per frame
	private void Update () {
		
		base.Update();
			
		if(mainCamera.WorldToViewportPoint(transform.position).y < -.5f)
			Destroy(gameObject);
	

	}

	private void OnTriggerEnter(Collider collider) {
		
		base.OnTriggerEnter(collider);
		
		if(collider.gameObject.tag != "Bubble") return;

		placeholderIndex++;

		Vector2 v = healthFill.rectTransform.sizeDelta;
		v.x += .5f;
		healthFill.rectTransform.sizeDelta = v;

		if(!(Mathf.Abs(v.x - health) <= .1f)) return;
		
		SpawnSpellComponent();
		
		Particles.Play();
		iTween.ScaleTo(gameObject, Vector3.zero, 1f);
		Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Villager));

		StartCoroutine(RemoveVillager());

		IsDestroyed = true;
		GameConfig.peopleSaved++;


	}

}