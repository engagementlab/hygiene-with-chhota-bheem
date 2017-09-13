using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchetypePowerUp : ArchetypeMove {

	public Sprite[] spells;

	public Sprite currentSpell;

	// Use this for initialization
	void Awake ()
	{
		// Pick the spell item
    int index = Random.Range(0, spells.Length);
    currentSpell = spells[index];

    gameObject.GetComponent<SpriteRenderer>().sprite = currentSpell;
    Debug.Log(currentSpell);
		
	}
	
	// Update is called once per frame
	void Update () {
      

		
		
	}


}
