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
	private GameObject _slowMoWrapper;
	public GameObject _spellActivatedUi;
	public GameObject[] _spellSteps;
	
	private GameObject _gameEndUi;
	private Animator _pauseAnimator;

	private Text _fliesCount;
	private Text _villagerCount;
	private Text _score;
	private int _stars;

	private int _steps;

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

		_slowMoWrapper = GameObject.Find("GameUI/SlowMoWrap");
		_slowMoWrapper.SetActive(false);
		
		_score = GameObject.Find("GameUI/Score/ScoreCount").GetComponent<Text>();

		_steps = GameObject.Find("Player").GetComponent<ArchetypePlayer>().SpellStepCount;
		
		SpellSize = SpellBars[0].GetComponent<RectTransform>().sizeDelta.y/_steps;

		for (int i = 0; i < SpellBars.Length; i++)
		{
			SpellBars[i].SetActive(false);
			
			var fill = SpellBars[i].GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2(fill.sizeDelta.x, 0);
		}
		
		_spellActivatedUi = GameObject.Find("GameUI/SpellActivated");
//		_spellSteps = GameObject.FindGameObjectsWithTag("StepGroup");
		
		_spellActivatedUi.SetActive(false);
		_spellCount = 0;

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

		iTween.ValueTo(fill, iTween.Hash(
				"from", spellFill.sizeDelta.y,
				"to", spellFill.sizeDelta.y + SpellSize,
				"time", 1,
				"easetype", iTween.EaseType.easeOutSine,
				"onupdate", "AdjustSpellLevel"));
		// Maybe someday:
		//		iTween.ShakeRotation(fill.transform.parent.gameObject, Vector3.one*10, 5);
		
		_spellCount++;

		if (_spellCount == _steps)
		{
			Events.instance.Raise (new SpellEvent(type, true));
			EmptySpells();
		}

	}

	public void EmptySpells()
	{
		
		_bar = GameObject.FindGameObjectWithTag("SpellBar");
				
		var fill = _bar.GetComponent<RectTransform>();
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

	public void ShowSloMo()
	{
		_slowMoWrapper.SetActive(true);
	}

	public void HideSloMo()
	{
		_slowMoWrapper.SetActive(false);
	}

	public void GameEnd(bool win)
	{
		_gameEndScreen.SetActive(true);
		
		_gameEndScore.text = GameConfig.Score.ToString();
		
		_gameEndVillagers.text = GameConfig.VillagersSaved.ToString();

		_gameEndAnim.SetBool("won", win);

	}

}
