using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SpellObject : MonoBehaviour {

	public void SelectComponent(SpellComponent component)
	{
		transform.Find(component.ToString()).GetComponent<SpriteRenderer>().enabled = true;
	}

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Player") return;
		
		
			
	}
}
