using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArchetypeSpellJuice : MonoBehaviour
{

	private Vector3[] movementPoints;
	private float percentsPerSecond = .1f;
	private float currentPathPercent;
	
	public Sprite[] spells;

	public Sprite currentSpell;

	private SpellComponent thisComponent;

	public Spells type;

	private void Awake()
	{
		
		// Pick the spell item
		int index = Random.Range(0, spells.Length);
		currentSpell = spells[index];

		gameObject.GetComponent<SpriteRenderer>().sprite = currentSpell;

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

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Player") return;
		
		Debug.Log("We triggered a spell component");

		var currentSpell = GameObject.FindGameObjectWithTag("SpellBar");

		if (currentSpell != null)
		{
			var spellBars = GUIManager.Instance.spellBars;
			for (int i = 0; i < spellBars.Length; i++)
			{
				if (spellBars[i].GetComponent<ArchetypeSpell>().type == type)
				{
					currentSpell = spellBars[i];
				}
			}
		}
			
		Debug.Log(currentSpell);
		
		if (currentSpell.GetComponent<ArchetypeSpell>().type == type)
			Debug.Log(type);
		
		GUIManager.Instance.AddSpellJuice(thisComponent);
		Destroy(gameObject);

	}

	public void SelectComponent(SpellComponent component)
	{
		thisComponent = component;
		transform.Find(component.ToString()).GetComponent<SpriteRenderer>().enabled = true;
	}
}