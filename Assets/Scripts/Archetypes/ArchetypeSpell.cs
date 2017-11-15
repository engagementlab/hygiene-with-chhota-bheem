using System.Collections;
using UnityEngine;

public class ArchetypeSpell : ArchetypeMove {

	public Spells Type;

	private RectTransform spellFill;
	
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
	
	public void AdjustSpellLevel(float spellSize){
		spellFill.sizeDelta = new Vector2( spellFill.sizeDelta.x, spellSize);

	}

}
