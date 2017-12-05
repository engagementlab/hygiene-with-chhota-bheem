using System.Collections;
using UnityEngine;

public class ArchetypeSpell : ArchetypeMove {

	public Spells Type;

	private RectTransform spellFill;
	private bool isFilling;
	private float queueAmount;
	
	private void Awake()
	{
		spellFill = GetComponent<RectTransform>();
	}

	public IEnumerator Timer(int time, string power) {

		while(time>0){
			yield return new WaitForSeconds(1);
		}

		if (power == "SpellMatrix") {
			// Reset game speed
		} else if (power == "SpellSpeedShoot") {
			// Reset shooting speed
		} else if (power == "SpellScatterShoot") {
			// Reset shooting
		}

	}

	public void Fill(float amount, bool isFull)
	{
		if(isFilling)
		{
			queueAmount = amount;
			return;
		}
		isFilling = true;
		
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", spellFill.sizeDelta.y,
			"to", spellFill.sizeDelta.y + amount,
			"time", 1,
			"delay", 1,
			"easetype", iTween.EaseType.easeOutSine,
			"onupdate", "AdjustSpellLevel",
			"oncomplete", "SpellLevelAdjusted", 
			"oncompletetarget", gameObject,
			"oncompleteparams", isFull));
		iTween.PunchRotation(transform.parent.gameObject, iTween.Hash("amount", Vector3.one*50, "time", 5, "delay", 1, "oncomplete", "RotateDone", "oncompletetarget", gameObject));

	}
	
	public void AdjustSpellLevel(float spellSize){
		
		spellFill.sizeDelta = new Vector2(spellFill.sizeDelta.x, spellSize);

	}

	private void RotateDone()
	{
		iTween.RotateTo(transform.parent.gameObject, iTween.Hash("rotation", Vector3.zero, "time", .1f, "easetype", iTween.EaseType.easeOutSine));
	}

	public void SpellLevelAdjusted(bool full)
	{
		isFilling = false;
		if(!full)
		{
			if(queueAmount > 0)
			{
				iTween.ValueTo(gameObject, iTween.Hash(
					"from", spellFill.sizeDelta.y,
					"to", spellFill.sizeDelta.y + queueAmount,
					"time", 1,
					"easetype", iTween.EaseType.easeOutSine,
					"onupdate", "AdjustSpellLevel",
					"oncomplete", "SpellLevelAdjusted", 
					"oncompletetarget", gameObject,
					"oncompleteparams", true));
				queueAmount = 0;
			}
				
			return;
		}
		// Reset potion
		spellFill.sizeDelta = new Vector2(spellFill.sizeDelta.x, 0);
		
		// If spell is full, dispatch event
		Events.instance.Raise (new SpellEvent(Type, true));
		GUIManager.Instance.EmptySpells();	
	}

}
