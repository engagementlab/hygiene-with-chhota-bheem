using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
  	
	}

	public IEnumerator Timer(int time, string power) {

		while(time>0){
        Debug.Log(time--);
        yield return new WaitForSeconds(1);
    }
    Debug.Log("Countdown Complete!");

    if (power == "PowerUpMatrix") {
    	// Reset game speed
    } else if (power == "PowerUpSpeedShoot") {
    	// Reset shooting speed
    } else if (power == "PowerUpScatterShoot") {
    	// Reset shooting
    }

	}

	private void OnTriggerEnter(Collider collider) {

		if (collider.gameObject.tag == "Player") {

			Events.instance.Raise (new PowerUpEvent(PowerUps.SpeedShoot));

		}

	}


}
