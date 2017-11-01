﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class VillagerObject : ArchetypeMove
{

	public ParticleSystem Particles;
//	public RawImage healthFill;

	private Camera _mainCamera;
	private SpriteRenderer _villagerRenderer;
	private Sprite[] _spriteFrames;
	
	private IEnumerator RemoveVillager()
	{
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	// Use this for initialization
	private void Awake () {
		
		base.Awake();
		
		_mainCamera = Camera.main;
		_villagerRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		
		// Pick random villager spritesheet and set to first frame
		_spriteFrames = Resources.LoadAll<Sprite>("Villagers/"+UnityEngine.Random.Range(1, 4));
		_villagerRenderer.sprite = _spriteFrames[0];
		
	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();
			
		if(_mainCamera.WorldToViewportPoint(transform.position).y < -.5f)
			Destroy(gameObject);
	
	}

	private void OnTriggerEnter(Collider collider) {
		
//		base.OnTriggerEnter(collider);
		
		if(collider.gameObject.tag != "Bubble") return;
		
		Destroy(collider.gameObject);
		if(_bubblesHit < HitPoints-1)
		{
			_bubblesHit += _playerScript.Strength;
			
			if(_bubblesHit <= _spriteFrames.Length)
				_villagerRenderer.sprite = _spriteFrames[_bubblesHit];
			return;
		}

		SpawnSpellComponent();
		
		Particles.Play();
		iTween.ScaleTo(gameObject, Vector3.zero, 1f);
		Events.instance.Raise (new ScoreEvent(pointsWorth, ScoreEvent.Type.Villager));

		StartCoroutine(RemoveVillager());

		IsDestroyed = true;
		GameConfig.PeopleSaved++;


	}

}