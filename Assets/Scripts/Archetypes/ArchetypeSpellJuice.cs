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
		get { return type; }
		set { 
			type = value;
			foreach(Transform child in transform)
				child.gameObject.SetActive(false);
			
			transform.Find(type.ToString()).gameObject.SetActive(true);
		}
	}

	public GameObject currentSpell;

	private Vector3[] movementPoints;
	private Spells type;
	private float percentsPerSecond = .1f;
	private float currentPathPercent;

	private void Awake()
	{
		
		// Pick the spell item
		var spells = transform.GetComponentsInChildren(typeof(SpriteRenderer), true);
		int index = Random.Range(0, spells.Length);
		currentSpell = spells[index].gameObject;
		currentSpell.SetActive(true);

		movementPoints = new Vector3[10];

		for (int i = 0; i < 10; i++)
			movementPoints[i] =
				Utilities.ClampToScreen(
					new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1),
						Random.Range(transform.position.y - 1, transform.position.y + 1), 0), Camera.main);

	}

	private void Update()
	{
		if(currentPathPercent >= 1)
			Destroy(gameObject);
		
		currentPathPercent += percentsPerSecond * Time.deltaTime;
		iTween.PutOnPath(transform, movementPoints, currentPathPercent);
		
	}

	private void JuiceCollected(GameObject spellObject)
	{
		var fill = spellObject.transform.Find("Background").gameObject;
		// Update Spell Juice UI
		GuiManager.Instance.AddSpellJuice(type, fill);
		// Add Spell Juice to Inventory
		Inventory.instance.AddSpellComponent(type);
		
		// Destroy this spell juice
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Player") return;
		
		var currentSpellObject = GameObject.FindGameObjectWithTag("SpellBar");

		if (currentSpellObject == null || currentSpellObject.GetComponent<ArchetypeSpell>().type != type)
		{
			var spellBars = GuiManager.Instance.SpellBars;
			
			for (int i = 0; i < spellBars.Length; i++)
			{
				if (spellBars[i].GetComponent<ArchetypeSpell>().type == type)
				{
					currentSpellObject = spellBars[i];
					GuiManager.Instance.NewSpell(spellBars[i]);
					
					JuiceCollected(currentSpellObject);
				}
			}
		}
		else if (currentSpellObject.GetComponent<ArchetypeSpell>().type == type)
		{
//			Debug.Log("Continuing to work towards spell '" + type + "'!");
			JuiceCollected(currentSpellObject);

		}	

	}

}