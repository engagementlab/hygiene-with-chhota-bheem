using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Archetypes
{
	public class ArchetypeSpell : ArchetypeMove {

		public Sprite[] spells;

		public Sprite currentSpell;

		// Use this for initialization
		private void Awake ()
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

			if (power == "SpellMatrix") {
				// Reset game speed
			} else if (power == "SpellSpeedShoot") {
				// Reset shooting speed
			} else if (power == "SpellScatterShoot") {
				// Reset shooting
			}

		}

		private void OnTriggerEnter(Collider collider) {

			if (collider.gameObject.tag == "Player") {

				Events.instance.Raise (new SpellEvent(Spells.SpeedShoot));
				Destroy(gameObject);

			}

		}


	}
}
