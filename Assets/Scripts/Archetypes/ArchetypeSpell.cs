﻿using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class ArchetypeSpell : ArchetypeMove {

	public Spells type;

	// Use this for initialization
	private void Start ()
	{
		

	}

	public IEnumerator Timer(int time, string power) {

		while(time>0){
			Debug.Log(time--);
			yield return new WaitForSeconds(1);
		}
		Debug.Log("Countdown Complete!");

		if (power == "SpellMatrix") {
			// Reset game speed
		} else if (power == "SpellSpeedShoot") {
			// Reset shooting speed
		} else if (power == "SpellScatterShoot") {
			// Reset shooting
		}

	}

}
