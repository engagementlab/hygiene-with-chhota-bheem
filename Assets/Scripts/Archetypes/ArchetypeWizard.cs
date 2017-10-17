/*

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeWizard.cs
	Archetype class for wizard.

	Created by Johnny Richardson, Erica Salling.
==============

*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeWizard : MonoBehaviour
{

	[Range(0, 40)]
	[Tooltip("Time before tiled background stops moving")]
	public int BackgroundDelay;
	public RawImage HealthFill;
	public Movements MovementType;
	public float Health = 2f;
	public float SmoothTime = .1f;
	
	[HideInInspector]
	public bool spawned;

	private GameObject _player;
	private GameObject _parent;

	private float _playerPos;
	private Vector3 _wizardPos;

	private Vector3 _velocity;
	
	public enum Movements
	{
		Avoid, 
		Follow, 
		Dodge
	}

	public void Awake() {

		_player = GameObject.FindWithTag("Player");
		_parent = GameObject.FindWithTag("Parent");
		
	}

	private IEnumerator Spawned()
	{
		yield return new WaitForSeconds(BackgroundDelay);
		_parent.GetComponent<ArchetypeMove>().MoveEnabled = false;

	}

	public void Update()
	{

		if(!spawned) return;

		StartCoroutine(Spawned());

		float height = 2f * Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		// Check player & wizard position
		_playerPos = _player.transform.position.x;
		_wizardPos = gameObject.transform.position;

		var distance = Vector3.Distance(_wizardPos, new Vector3(0f, _wizardPos.y, _wizardPos.z));

		switch(MovementType)
		{
			case Movements.Avoid:
				
				// move wizard away from bounds & player
				if(_wizardPos.x <= _playerPos && _wizardPos.x >= _playerPos - 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					else
						_wizardPos = new Vector3(_wizardPos.x - 2.0f, _wizardPos.y, _wizardPos.z);

				}
				else if(_wizardPos.x >= _playerPos && _wizardPos.x <= _playerPos + 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					else
						_wizardPos = new Vector3(_wizardPos.x + 2.0f, _wizardPos.y, _wizardPos.z);
				}

				break;

			case Movements.Follow:

				// move wizard away from bounds & towards player
				if(_wizardPos.x <= _playerPos && _wizardPos.x <= _playerPos - 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x + 2.0f, _wizardPos.y, _wizardPos.z);
					}

				} else if(_wizardPos.x >= _playerPos && _wizardPos.x >= _playerPos + 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x - 2.0f, _wizardPos.y, _wizardPos.z);
					}
				}

				break;

			case Movements.Dodge:

				// move wizard away from bounds & towards player

				if(_wizardPos.x <= _playerPos && _wizardPos.x <= _playerPos - 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x + 2.0f, _wizardPos.y, _wizardPos.z);
					}

				} else if(_wizardPos.x >= _playerPos && _wizardPos.x >= _playerPos + 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x - 2.0f, _wizardPos.y, _wizardPos.z);
					}
				}

				break;
		}

		gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, _wizardPos, ref _velocity, SmoothTime);


	}


	public void OnTriggerEnter(Collider collider)
	{

		if(collider.gameObject.tag != "Bubble") return;

		Events.instance.Raise(new HitEvent(HitEvent.Type.Spawn, collider, collider.gameObject));

		Vector2 v = HealthFill.rectTransform.sizeDelta;
		v.x += .5f;
		HealthFill.rectTransform.sizeDelta = v;

		if(!(Mathf.Abs(v.x - Health) <= .1f)) return;

		// Destroy Wizard
		iTween.ScaleTo(gameObject, Vector3.zero, 1.0f);
		StartCoroutine(DestroyWizard());

		// You won the game
		Events.instance.Raise(new ScoreEvent(1, ScoreEvent.Type.Wizard));
		Events.instance.Raise(new DeathEvent(true));

	}


	private IEnumerator DestroyWizard()
	{
		
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
		
	}

}