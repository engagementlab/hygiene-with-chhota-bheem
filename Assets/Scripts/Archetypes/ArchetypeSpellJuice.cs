using UnityEngine;
using Random = UnityEngine.Random;

public class ArchetypeSpellJuice : MonoBehaviour
{

	public Spells Type
	{
		get { return _type; }
		set { 
			_type = value;
			foreach(Transform child in transform)
				child.gameObject.SetActive(false);
			
			transform.Find(_type.ToString()).gameObject.SetActive(true);
		}
	}

	public GameObject CurrentSpell;
	private float _targetAnimSpeed;
	private float _timeElapsed;
	private int _nextPoint;
	private Vector3 _lastPoint;
	private Vector3 _toPoint;
	private Vector3[] _movementPoints;
	private Spells _type;

	private void Awake()
	{
		// Pick the spell item
		var spells = transform.GetComponentsInChildren(typeof(SpriteRenderer), true);
		int index = Random.Range(0, spells.Length);
		CurrentSpell = spells[index].gameObject;
		CurrentSpell.SetActive(true);

	}

	private void Update()
	{
	
		if(!GameConfig.GamePaused)
			_timeElapsed += Time.deltaTime;
		
		// Destroy this object after 5 seconds
		if(_timeElapsed >= 5)
			Destroy(gameObject);
		
	}

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Player") return;
		
		var currentSpellObject = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (currentSpellObject == null || currentSpellObject.GetComponent<ArchetypeSpell>().Type != _type)
		{
			var spellBars = GUIManager.Instance.SpellBars;
			
			for (int i = 0; i < spellBars.Length; i++)
			{
				if (spellBars[i].GetComponent<ArchetypeSpell>().Type == _type)
				{
					currentSpellObject = spellBars[i];
					GUIManager.Instance.NewSpell(spellBars[i]);

					JuiceCollected(currentSpellObject);
				}
			}
		}
		else if (currentSpellObject.GetComponent<ArchetypeSpell>().Type == _type)
			JuiceCollected(currentSpellObject);

	}

	public void StartMovement(Vector3 startingPos)
	{
		transform.position = startingPos;
	}
	
	private void JuiceCollected(GameObject spellObject)
	{
		// SFX
		bool soundBool = Random.value > .5f;
		Events.instance.Raise(new SoundEvent("spell_pickup_"+(soundBool?"1":"2"), SoundEvent.SoundType.SFX));
				
		// Update Spell Juice UI
		var fill = spellObject;
		GUIManager.Instance.AddSpellJuice(_type, fill);
		
		// Destroy this spell juice
		Destroy(gameObject);
		
	}

}