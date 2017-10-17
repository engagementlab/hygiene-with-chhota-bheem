using System;
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

	private Vector3[] _movementPoints;
	private Spells _type;
	private float _percentsPerSecond = .1f;
	private float _currentPathPercent;

	public void Animate(Vector3 startingPos)
	{
		
		_movementPoints = new Vector3[10];
		_movementPoints[0] = Utilities.ClampToScreen(startingPos, Camera.main);

		for (int i = 1; i < 10; i++)
			_movementPoints[i] =
				Utilities.ClampToScreen(
					new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1),
						Random.Range(transform.position.y - 1, transform.position.y + 1), 0), Camera.main);
		
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
		if(_movementPoints == null) return;
	
		if(_currentPathPercent >= 1)
			Destroy(gameObject);
		
		_currentPathPercent += _percentsPerSecond * Time.deltaTime;
		iTween.PutOnPath(transform, _movementPoints, _currentPathPercent);
		
	}

	private void JuiceCollected(GameObject spellObject)
	{
		var fill = spellObject.transform.Find("Background").gameObject;
		// Update Spell Juice UI
		GUIManager.Instance.AddSpellJuice(_type, fill);
		// Add Spell Juice to Inventory
		Inventory.instance.AddSpellComponent(_type);
		
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