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
	private Vector3 _startingPos;
	private Vector3 _lastPoint;
	private Vector3 _toPoint;
	private Vector3[] _movementPoints;
	private Spells _type;

	public void StartMovement(Vector3 startingPos)
	{
		transform.position = startingPos;
		_startingPos = startingPos;
		Animate();
	}

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

	private void Animate()
	{

		float x;
		float y;
		
		if (transform.position.x - _startingPos.x >= 1 || transform.position.x - _startingPos.x <= -1)
			x = Random.Range(_startingPos.x - 1, _startingPos.x + 1);
		else 
			x = Random.Range(transform.position.x - 1, transform.position.x + 1);
		
		
		if (transform.position.y - _startingPos.y >= 1 || transform.position.y - _startingPos.y <= -1)
			y = Random.Range(_startingPos.y - 1, _startingPos.y - 0.5f);
		else 
			y = Random.Range(transform.position.y - 1, transform.position.y - 0.5f);
		
		// Place object at current %
		_lastPoint = transform.position;
		_toPoint = Utilities.ClampToScreen(new Vector3(x, y, 0), Camera.main);	
		
		var distance = Vector3.Distance(_toPoint, _lastPoint);
		iTween.MoveTo(gameObject, iTween.Hash("position", _toPoint, "time", distance/2, "easetype", iTween.EaseType.linear, "oncomplete", "Complete"));
	}

	private void Complete()
	{
		
		Animate();
		
	}

	private void JuiceCollected(GameObject spellObject)
	{
		// SFX
		bool soundBool = Random.value > .5f;
		Events.instance.Raise(new SoundEvent("spell_pickup_"+(soundBool?"1":"2"), SoundEvent.SoundType.SFX));
				
		// Update Spell Juice UI
		var fill = spellObject.transform.Find("Background").gameObject;
		GUIManager.Instance.AddSpellJuice(_type, fill);
		
		// Destroy this spell juice
		Destroy(gameObject);
		
	}

}