using System.Collections;
using DefaultNamespace;
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
		
	private GameObject _spellText;
	private GameObject _pauseUi;
	private GameObject _slowMoWrapper;
	
	private GameObject _spellActivatedUi;
	private GameObject _spellActivatedBanner;
	
	private GameObject _gameEndUi;
	private Animator _pauseAnimator;

	private Text _fliesCount;
	private Text _villagerCount;
	private Text _score;
	
	private int _stars;
	private int _steps;

	public GameObject[] SpellBars;
	private GameObject _bar;
	private float _spellSize;
	private int _spellCount;
	
	// Use this for initialization
	public void Initialize ()
	{ 
		var playerObj = GameObject.Find("Player");
		if(playerObj != null)
			_steps = playerObj.GetComponent<ArchetypePlayer>().SpellStepCount;
		
		SpellBars = GameObject.FindGameObjectsWithTag("SpellBar");
		
		_gameEndScreen = GameObject.Find("GameUI/GameEndScreen");
//		_gameEndAnim = _gameEndScreen.GetComponent<Animator>();
		
		_gameEndScreen.SetActive(false);

		_pauseUi = GameObject.Find("GameUI/PauseUI");
		_pauseAnimator = _pauseUi.GetComponent<Animator>();
		_pauseUi.SetActive(false);

		_slowMoWrapper = GameObject.Find("GameUI/SlowMoWrap");
		_slowMoWrapper.SetActive(false);
		
		_score = GameObject.Find("GameUI/Score/ScoreCount").GetComponent<Text>();
		_spellSize = SpellBars[0].GetComponent<RectTransform>().sizeDelta.y/_steps;

		for (int i = 0; i < SpellBars.Length; i++)
		{
			SpellBars[i].SetActive(false);
			
			var fill = SpellBars[i].GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2(fill.sizeDelta.x, 0);
		}
		
		_spellActivatedUi = GameObject.Find("GameUI/SpellActivated");		
		_spellActivatedBanner = _spellActivatedUi.transform.Find("BannerImage").gameObject;		
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
				"to", spellFill.sizeDelta.y + _spellSize,
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

	public IEnumerator ShowSpellActivated()
	{
		_spellActivatedUi.SetActive(true);
		iTween.MoveFrom(_spellActivatedUi, iTween.Hash("position", new Vector3(0, 200, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutElastic));
		iTween.PunchScale(_spellActivatedBanner, iTween.Hash("amount", Vector3.one*1.2f, "time", 1.3f, "easetype", iTween.EaseType.easeInOutBounce));
		
		yield return new WaitForSeconds(1.5f);

		iTween.MoveTo(_spellActivatedUi, iTween.Hash("position", new Vector3(0, 200, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
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
		_pauseAnimator.Play("FingerMove");

	}

	public void HideSloMo()
	{
		_slowMoWrapper.SetActive(false);
	}

	public void GameEnd(bool win)
	{
		
		_gameEndScreen.SetActive(true);
		_gameEndScreen.GetComponent<GameEndUI>().SetContent(win);

	}
	
	

}
