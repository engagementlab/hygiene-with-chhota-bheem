﻿using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArchetypeSpellJuice : MonoBehaviour
{

	public Spells Type
	{
		get { return _type; }
		set { 
			_type = value;
			foreach(Transform child in transform)
				child.gameObject.SetActive(false);
			
			transform.Find(_type.ToString()).gameObject.SetActive(true);
		}
	}

	public GameObject CurrentSpell;
	private float _targetAnimSpeed;
	private int _nextPoint;
	private Vector3 _startingPos;
	private Vector3 _lastPoint;
	private Vector3 _toPoint;
	private Vector3[] _movementPoints;
	private Spells _type;
	private float _percentsPerSecond = .1f;
	private float _currentPathPercent;

	public void StartMovement(Vector3 startingPos)
	{
		transform.position = startingPos;
		_startingPos = startingPos;
		Animate();
	}

	private void Awake()
	{
		// Pick the spell item
		var spells = transform.GetComponentsInChildren(typeof(SpriteRenderer), true);
		int index = Random.Range(0, spells.Length);
		CurrentSpell = spells[index].gameObject;
		CurrentSpell.SetActive(true);

	}

	private void Update()
	{
	
		if(_currentPathPercent >= 1)
			Destroy(gameObject);
		
		_currentPathPercent += _percentsPerSecond * Time.deltaTime;
				
	}
	
	internal void Animate()
	{

		float x;
		float y;
		
		if (transform.position.x - _startingPos.x >= 1 || transform.position.x - _startingPos.x <= -1)
			x = Random.Range(_startingPos.x - 1, _startingPos.x + 1);
		else 
			x = Random.Range(transform.position.x - 1, transform.position.x + 1);
		
		
		if (transform.position.y - _startingPos.y >= 1 || transform.position.y - _startingPos.y <= -1)
			y = Random.Range(_startingPos.y - 1, _startingPos.y - 0.5f);
		else 
			y = Random.Range(transform.position.y - 1, transform.position.y - 0.5f);
		
		// Place object at current %
		_lastPoint = transform.position;
		_toPoint = Utilities.ClampToScreen(new Vector3(x, y, 0), Camera.main);	
		
		var distance = Vector3.Distance(_toPoint, _lastPoint);
		iTween.MoveTo(gameObject, iTween.Hash("position", _toPoint, "time", distance/2, "easetype", iTween.EaseType.linear, "oncomplete", "Complete"));
	}

	void Complete()
	{
		
		Animate();
		
	}

	private void JuiceCollected(GameObject spellObject)
	{
		var fill = spellObject.transform.Find("Background").gameObject;
		// Update Spell Juice UI
		GUIManager.Instance.AddSpellJuice(_type, fill);
		
		// Destroy this spell juice
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Player") return;
		
		var currentSpellObject = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (currentSpellObject == null || currentSpellObject.GetComponent<ArchetypeSpell>().Type != _type)
		{
			var spellBars = GUIManager.Instance.SpellBars;
			
			for (int i = 0; i < spellBars.Length; i++)
			{
				if (spellBars[i].GetComponent<ArchetypeSpell>().Type == _type)
				{
					currentSpellObject = spellBars[i];
					GUIManager.Instance.NewSpell(spellBars[i]);
					
					JuiceCollected(currentSpellObject);
				}
			}
		}
		else if (currentSpellObject.GetComponent<ArchetypeSpell>().Type == _type)
		{
//			Debug.Log("Continuing to work towards spell '" + type + "'!");
			JuiceCollected(currentSpellObject);

		}	

	}

}