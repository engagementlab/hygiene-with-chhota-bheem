using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpUnderlay : MonoBehaviour
{

	public GameObject BlueSpellParent;
	public GameObject RedSpellParent;
	public GameObject YellowSpellParent;

	private Transform[] _underlayRings;
	private int _ringIndex = -1;

	// Use this for initialization
	void Start () {
		
		
		
	}

	public void Setup(Spells type)
	{

		GameObject activeParent = BlueSpellParent;
		
			switch (type)
			{
					
				case Spells.SpeedShoot:
					activeParent = RedSpellParent;
					break;
					
				case Spells.ScatterShoot:
					activeParent = YellowSpellParent;
					break;
			}
		
		activeParent.SetActive(true);
		
		_underlayRings = activeParent.GetComponentsInChildren<Transform>().Skip(1).ToArray();
		
		foreach(var ring in _underlayRings)
			ring.gameObject.SetActive(false);
		
//		_underlayRings[0].gameObject.SetActive(true);
	}

	public void Add()
	{
		if(_ringIndex < 4)
		{
			_ringIndex++;
			_underlayRings[_ringIndex].gameObject.SetActive(true);
			int rotAmount = _ringIndex % 2 == 0 ? 2 : -2;
			iTween.ScaleFrom(_underlayRings[_ringIndex].gameObject, iTween.Hash("scale", Vector3.zero, "time", 4.4f, "easetype", iTween.EaseType.easeOutElastic));
			iTween.RotateBy(_underlayRings[_ringIndex].gameObject, iTween.Hash("z", rotAmount, "time", 50, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));

		}
	}

	public void Subtract()
	{
		
		if(_ringIndex > -1)
		{
			iTween.ScaleTo(_underlayRings[_ringIndex].gameObject, iTween.Hash("scale", Vector3.zero, "time", 3.4f, "easetype", iTween.EaseType.easeInElastic));
			_ringIndex--;
		}
	}
	
}
