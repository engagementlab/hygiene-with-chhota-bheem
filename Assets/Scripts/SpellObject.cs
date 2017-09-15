using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SpellObject : MonoBehaviour
{

	private Vector3[] movementPoints;
	private float percentsPerSecond = .1f;
	private float currentPathPercent;

	private SpellComponent thisComponent;
	
	private void Awake()
	{
		
		movementPoints = new Vector3[10];
			
		for(int i = 0; i < 10; i++)
			movementPoints[i] = Utilities.ClampToScreen(new Vector3(Random.Range(transform.position.x-1, transform.position.x+1), Random.Range(transform.position.y-1, transform.position.y+1), 0), Camera.main);
		
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

		GUIManager.Instance.ShowSpellComponent(thisComponent);
		Destroy(gameObject);

	}

	public void SelectComponent(SpellComponent component)
	{
		thisComponent = component;
		transform.Find(component.ToString()).GetComponent<SpriteRenderer>().enabled = true;
	}

}
