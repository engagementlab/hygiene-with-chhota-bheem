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

	private GameObject _gameEndScreen;
	private Animator _gameEndAnim;
	private Text _gameEndScore;
	private Text _gameEndVillagers;
		
	private GameObject _inventoryUi;
	private GameObject _spellText;
	private GameObject _pauseUi;
	public GameObject _spellActivatedUi;
	public GameObject[] _spellSteps;
	private GameObject _gameEndUi;
	private Animator _pauseAnimator;

	private Text _fliesCount;
	private Text _villagerCount;
	private Text _score;
	private int _stars;

	public GameObject[] SpellBars;
	public float SpellSize;
	private GameObject _bar;
	private int _spellCount;
	public Animator[] _spellStepsComponent;

	// Use this for initialization
	public void Initialize ()
	{ 
		_inventoryUi = GameObject.Find("GameUI/SpellJuiceBars");
		
		SpellBars = GameObject.FindGameObjectsWithTag("SpellBar");
		
		_gameEndScreen = GameObject.Find("GameUI/GameEndScreen");
		_gameEndAnim = _gameEndScreen.GetComponent<Animator>();
		_gameEndScore = _gameEndScreen.transform.Find("Wrapper/Board/ScoreWrap/Score").GetComponent<Text>();
		_gameEndVillagers = _gameEndScreen.transform.Find("Wrapper/Board/VillagersMultiplier/Score").GetComponent<Text>();
		
		_gameEndScreen.SetActive(false);

		_pauseUi = GameObject.Find("GameUI/PauseUI");
		_pauseAnimator = _pauseUi.GetComponent<Animator>();
		_pauseUi.SetActive(false);
		
		_score = GameObject.Find("GameUI/Score/ScoreCount").GetComponent<Text>();
		
		SpellSize = SpellBars[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta.y/5;

		for (int i = 0; i < SpellBars.Length; i++)
		{
			SpellBars[i].SetActive(false);
			
			var fill = SpellBars[i].transform.Find("Background").GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2(fill.sizeDelta.x, 0);
		}
		
		_spellActivatedUi = GameObject.Find("GameUI/SpellActivated");
//		_spellSteps = GameObject.FindGameObjectsWithTag("StepGroup");
		
		_spellActivatedUi.SetActive(false);
		_spellCount = 0;

	}
	

	public void DisplayCurrentSpell(string spellName)
	{
		
//		_spellText.GetComponent<Text>().text = "Spell: " + spellName;
//		_spellText.SetActive(true);
		
	}
	
	public void HideSpell()
	{
		
//		_spellText.SetActive(false);
		
	}

	public void NewSpell(GameObject spellBar)
	{
		_bar = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (_bar != null)
			_bar.SetActive(false);
		
		spellBar.SetActive(true);
		_spellCount = 0;
	}

	public void AddSpellJuice(Spells type, GameObject fill)
	{
		var spellFill = fill.GetComponent<RectTransform>();
		spellFill.sizeDelta = new Vector2( spellFill.sizeDelta.x, spellFill.sizeDelta.y + SpellSize);
		_spellCount++;

		if (_spellCount == GameObject.FindGameObjectWithTag("Player").GetComponent<ArchetypePlayer>().SpellStepCount)
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

	public void UpdateScore(int num) {

		_score.text = num.ToString();

	}

	public void ShowPause()
	{
		_pauseUi.SetActive(true);
		_pauseAnimator.Play("ShowPause");
	}

	public void HidePause()
	{
		
		_pauseAnimator.Play("HidePause");
	}

	public void GameEnd(bool win)
	{
		_gameEndScreen.SetActive(true);
		
		_gameEndScore.text = GameConfig.Score.ToString();
		
		_gameEndVillagers.text = GameConfig.VillagersSaved.ToString();

		_gameEndAnim.SetBool("won", win);

	}

}
