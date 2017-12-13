using System.Linq;
using UnityEngine;

public class PowerUpUnderlay : MonoBehaviour
{

	public GameObject BlueSpellParent;
	public GameObject RedSpellParent;
	public GameObject YellowSpellParent;

	private SpriteRenderer[] _underlayRings;
	private int _ringIndex = -1;

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
		
		_underlayRings = activeParent.GetComponentsInChildren<SpriteRenderer>().ToArray();
		
		foreach(var ring in _underlayRings)
			ring.gameObject.SetActive(false);
		
	}

	public void Add()
	{
		if(_ringIndex < _underlayRings.Length-1)
		{
			_ringIndex++;
			SpriteRenderer ring = _underlayRings[_ringIndex];
			ring.transform.localScale = Vector3.zero;
			ring.gameObject.SetActive(true);
			
			int rotAmount = _ringIndex % 2 == 0 ? 2 : -2;
			iTween.ScaleTo(ring.gameObject, iTween.Hash("name", "scale"+_ringIndex, "scale", Vector3.one * 1.42f, "time", 2, "easetype", iTween.EaseType.easeOutElastic));
			iTween.RotateBy(ring.gameObject, iTween.Hash("z", rotAmount, "time", 50, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));

		}
	}

	public void Subtract()
	{
		
		if(_ringIndex >= 0 && _ringIndex < _underlayRings.Length)
		{
			iTween.Stop(_underlayRings[_ringIndex].gameObject);
			iTween.ScaleTo(_underlayRings[_ringIndex].gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInElastic));
			
			_ringIndex--;
		}
		
	}
	
}
