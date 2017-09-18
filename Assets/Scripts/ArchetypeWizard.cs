/*

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeMove.cs
	Archetype class for which all moving non-player objects use or inherit.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/ArchetypeMove.cs

	Created by Johnny Richardson, Erica Salling.
==============

*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeWizard : MonoBehaviour
{
	public int speed;

	private GameObject player;
	private GameObject parent;

	private float playerPos;
	private Vector3 wizardPos;

	private Vector3 _velocity;
	public float SmoothTime = 0.1f;

	public void Awake() {

		player = GameObject.FindWithTag("Player");
		parent = GameObject.FindWithTag("Parent");
		
	}
	
	public void Update () {

		if (parent.transform.position.y <= 4f) {

			if (parent.GetComponent<ArchetypeMove>().MoveEnabled == true) {
				parent.GetComponent<ArchetypeMove>().MoveEnabled = false;
			}

			// Check player & wizard position, move wizard away from bounds & player
			playerPos = player.transform.position.x;
			wizardPos = gameObject.transform.position;

			var distance = Vector3.Distance(wizardPos, new Vector3(0f, wizardPos.y, wizardPos.z));

			if (wizardPos.x <= playerPos && wizardPos.x >= playerPos - 1.5f) {
				// Move Wizard
				if (distance >= 3.5f) {
					wizardPos = new Vector3(0, wizardPos.y, wizardPos.z);
				} else {
					wizardPos = new Vector3(wizardPos.x - 2.0f, wizardPos.y, wizardPos.z);
				}

			} else if (wizardPos.x >= playerPos && wizardPos.x <= playerPos + 1.5f) {
			 // Move Wizard
				if (distance >= 3.5f) {
					wizardPos = new Vector3(0, wizardPos.y, wizardPos.z);
				} else {
					wizardPos = new Vector3(wizardPos.x + 2.0f, wizardPos.y, wizardPos.z);
				}
			} 
			gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, wizardPos, ref _velocity, SmoothTime);
		}

		
	}
	
  public void OnTriggerEnter(Collider collider) {
	  
  	
  }

}