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
			var spellTransform = transform.Find(_type.ToString());
			if(spellTransform == null)
			{
				Debug.LogError("Spell transform for " + _type.ToString() + " not found!");
				return;
			}
			
			spellTransform.gameObject.SetActive(true);
		}
	}

	public GameObject CurrentSpell;
	private Vector3 _lastPoint;
	private Vector3 _toPoint;
	private Vector3[] _movementPoints;
	private Spells _type;

	private GameObject _glow;
	
	private float _targetAnimSpeed;
	private float _timeElapsed;
	private int _nextPoint;
	private bool _triggered;

	private ArchetypePlayer _playerScript;

	private void Awake()
	{
		// Pick the spell item
		var spells = transform.GetComponentsInChildren(typeof(SpriteRenderer), true);
		int index = Random.Range(0, spells.Length);
		CurrentSpell = spells[index].gameObject;
		CurrentSpell.SetActive(true);

		_glow = transform.Find("Glow").gameObject;

		_playerScript = GameObject.FindWithTag("Player").GetComponent<ArchetypePlayer>();

	}

	private void Start()
	{
		GlowControl(_type);
	}

	private void OnTriggerEnter(Collider collider) {
		
		if(collider.gameObject.tag != "Player" || _triggered || _playerScript.LifeLossRunning) return;
		
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

					_triggered = true;
					JuiceCollected(currentSpellObject);
				}
			}
		}
		else if(currentSpellObject.GetComponent<ArchetypeSpell>().Type == _type)
		{
			_triggered = true;
			JuiceCollected(currentSpellObject);
		}

	}

	private void GlowControl(Spells type)
	{

		switch (type)
		{
			case Spells.BigShoot:

				_glow.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f, 0.5f);
				break;
				
			case Spells.SpeedShoot: 
				_glow.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
				break;
				
			case Spells.ScatterShoot: 
				_glow.GetComponent<SpriteRenderer>().color = new Color(1f, 0.92f, 0.016f, 0.5f);
				break;
		}
		
		_glow.SetActive(true);
		
	}

	public void StartMovement(Vector3 startingPos)
	{
		transform.position = startingPos;
	}
	
	private void JuiceCollected(GameObject spellObject)
	{
		// Update Spell Juice UI, and load in particles
		var fill = spellObject;
		var spellParticles = Instantiate(Resources.Load<GameObject>("SpellParticles" + _type), transform.position, Quaternion.identity);
		
		// Destroy particles soon
		Destroy(spellParticles, 3.1f);
		GUIManager.Instance.AddSpellJuice(_type, fill, spellParticles);
		
		// Destroy this spell juice
		Destroy(gameObject);
		
	}
	
	

}