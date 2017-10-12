using UnityEngine;
using UnityEngine.UI;

public class GUIManager
{
	static GUIManager _instanceInternal;
	public static GUIManager Instance
	{
		get
		{
			if(_instanceInternal == null)
				_instanceInternal = new GUIManager();

			return _instanceInternal;
		}
	}
	
	private GameObject _inventoryUi;
	private GameObject _spellText;
	private GameObject _pauseUi;
	public GameObject _spellStepsUi;
	public GameObject[] _spellSteps;
	private GameObject _gameEndUi;
	private Animator _pauseAnimator;

	private Text _fliesCount;
	private Text _villagerCount;
	private Text _score;

	public GameObject[] SpellBars;
	public float SpellSize;
	private GameObject _bar;
	private int _spellCount;
	public Animator[] _spellStepsComponent;

	// Use this for initialization
	public void Initialiaze ()
	{ 
		_inventoryUi = GameObject.Find("GameUI/SpellJuiceBars");
		
		SpellBars = GameObject.FindGameObjectsWithTag("SpellBar");
		_spellText = GameObject.Find("GameUI/SpellJuiceBars/SpellText");
		_spellText.SetActive(false);
		
		_pauseUi = GameObject.Find("GameUI/PauseUI");
		_pauseUi.SetActive(false);
		

		_fliesCount = GameObject.Find("GameUI/Score/FlyCount").GetComponent<Text>();
		_villagerCount = GameObject.Find("GameUI/Score/VillagerCount").GetComponent<Text>();
		_score = GameObject.Find("GameUI/Score/ScoreCount").GetComponent<Text>();
		
		SpellSize = SpellBars[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta.y/5;

		for (int i = 0; i < SpellBars.Length; i++)
		{
			SpellBars[i].SetActive(false);
			
			var fill = SpellBars[i].transform.Find("Background").GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2(fill.sizeDelta.x, 0);
		}
		
		_spellStepsUi = GameObject.Find("GameUI/SpellSteps");
		_spellSteps = GameObject.FindGameObjectsWithTag("StepGroup");
		
		_gameEndUi = GameObject.Find("GameUI/GameEndScreen");
		_gameEndUi.SetActive(false);
		
		_spellStepsUi.SetActive(false);
		_spellCount = 0;
		
		foreach (GameObject group in _spellSteps)
		{
			group.SetActive(false);
		}

	}
	

	public void DisplayCurrentSpell(string spellName)
	{
		
		_spellText.GetComponent<Text>().text = "Spell: " + spellName;
		_spellText.SetActive(true);
		
	}
	
	public void HideSpell()
	{
		
		_spellText.SetActive(false);
		
	}

	public void NewSpell(GameObject spellBar)
	{
		_bar = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (_bar != null)
			_bar.SetActive(false);
		
		spellBar.SetActive(true);
		_spellCount = 1;
	}

	public void AddSpellJuice(Spells type, GameObject fill)
	{
		Debug.Log("Adding juice for spell '" + type + "'");

		var spellFill = fill.GetComponent<RectTransform>();
		spellFill.sizeDelta = new Vector2( spellFill.sizeDelta.x, spellFill.sizeDelta.y + SpellSize);
		_spellCount++;

		if (_spellCount == 5)
		{
			Events.instance.Raise (new SpellEvent(type, true));
			EmptySpells();
		}

	}


	public void EmptySpells()
	{
		
		_bar = GameObject.FindGameObjectWithTag("SpellBar");
				
		var fill = _bar.transform.Find("Background").GetComponent<RectTransform>();
		fill.sizeDelta = new Vector2( fill.sizeDelta.x, 0);

		_spellCount = 0;
		
		if (_bar != null)
			_bar.SetActive(false);

	}

	public void UpdateScore(float num, string type) {

		if (type == "Villager") {
			_villagerCount.text = GameConfig.peopleSaved.ToString();
		} else if (type == "Fly") {
			_fliesCount.text = GameConfig.fliesCaught.ToString();
		}

		_score.text = (float.Parse(_score.text) + num).ToString();

	}

	public void ShowPause()
	{
		_pauseAnimator.Play("ShowPause");
	}

	public void HidePause()
	{
//		pauseUI.SetActive(true);
		_pauseAnimator.Play("HidePause");
	}
}
