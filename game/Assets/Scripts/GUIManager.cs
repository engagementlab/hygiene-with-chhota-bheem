using System.Collections;
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
	public GameObject[] SpellBars;

	private GameObject _gameUI;

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

	private GameObject _bar;
	private float _spellSize;
	private int _spellCount;

	private ArchetypeSpell[] _pauseSpells;
	private float _pauseSpellSize;
	
	// Use this for initialization
	public void Initialize ()
	{ 
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if(playerObj != null)
			_steps = playerObj.GetComponent<ArchetypePlayer>().SpellStepCount;
		
		SpellBars = GameObject.FindGameObjectsWithTag("SpellBar");
		
		_gameUI = GameObject.Find("GameUI");

		_gameEndScreen = _gameUI.GetComponentInChildren<GameEndUI>(true).gameObject;
		_gameEndScreen.SetActive(false);

		_pauseUi = GameObject.Find("GameUI/PauseUI");
		_pauseUi.SetActive(false);
		_pauseAnimator = _pauseUi.GetComponent<Animator>();
		
		_pauseSpells = _pauseUi.GetComponentsInChildren<ArchetypeSpell>();
		_pauseSpellSize = _pauseSpells[0].GetComponent<RectTransform>().sizeDelta.y/_steps;

		foreach (ArchetypeSpell spell in _pauseSpells)
		{
			spell.gameObject.SetActive(false);
			
			var fill = spell.GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2(fill.sizeDelta.x, 0);
		}

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

		_score.text = GameConfig.Score + "";
	}

	public void NewSpell(GameObject spellBar)
	{
		_bar = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (_bar != null)
			_bar.SetActive(false);
		
		spellBar.SetActive(true);
		
		_spellCount = 0;
	}

	public void AddSpellJuice(Spells type, GameObject fill, GameObject particlesObj)
	{
		var spellFill = fill.GetComponent<RectTransform>();

		iTween.MoveTo(particlesObj, iTween.Hash("position", Camera.main.ScreenToWorldPoint(spellFill.transform.position), "time", 2, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
		
		_spellCount++;
		
		Events.instance.Raise(new SoundEvent("soap-pickup-"+(_spellCount==1?"1":"2"), SoundEvent.SoundType.SFX));
		fill.GetComponent<ArchetypeSpell>().Fill(_spellSize, _spellCount == _steps);
		
	}

	public void EmptySpells()
	{

		_spellCount = 0;
		
	}

	public void UpdateScore(int num) {

		_score.text = num.ToString();

	}

	public IEnumerator ShowSpellActivated()
	{
		Events.instance.Raise(new SoundEvent("spell-activate", SoundEvent.SoundType.SFX, null, .4f));
		
		_spellActivatedUi.SetActive(true);
		iTween.MoveTo(_spellActivatedUi, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutElastic));
		iTween.PunchScale(_spellActivatedBanner, iTween.Hash("amount", Vector3.one*1.2f, "time", 1.3f, "easetype", iTween.EaseType.easeInOutBounce));
		
		yield return new WaitForSeconds(1.5f);

		iTween.MoveTo(_spellActivatedUi, iTween.Hash("position", new Vector3(0, 200, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
	}

	public void UpdatePauseMenu(Spells type, float amount)
	{
		
		foreach (ArchetypeSpell spell in _pauseSpells)
		{
			if (spell.Type == type)
			{
				spell.gameObject.SetActive(true);

				var size = spell.GetComponent<RectTransform>();
				
				size.sizeDelta = new Vector2(size.sizeDelta.x, _pauseSpellSize * _spellCount);
			}
		}
		
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
		_gameEndScreen.GetComponent<GameEndUI>().SetContent(win);

	}
	
	

}
