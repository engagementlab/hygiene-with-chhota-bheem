using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

	public GameObject MainMenu;
	public GameObject Settings;
	public GameObject Info;
	public GameObject ChaptersParent;
	public GameObject Chapters;
	public GameObject Levels;
	public GameObject InterstitialsParent;
	public GameObject[] Interstitials;
	public GameObject[] InterstitialScreens;
 
	public AudioClip MenuMusic;
	public AudioSource GameSound;
	public AudioSource GameMusic;

	public Levels LevelToLoad;

	private Toggle _soundToggle;
	private Toggle _musicToggle;

	private Slider _volumeSlider;

	private GameObject _settingsTitle;
	private GameObject _settingsBoard;
	private GameObject _settingsBack;
	private GameObject[] _settingsLanguages;

	private GameObject _infoBoard;
	private GameObject _infoVersion;
	private GameObject _infoBack;

	private GameObject _chaptersTitle;
	private GameObject _chapterSelect;
	private GameObject _chaptersBack;
	private GameObject[] _chapterButtons;

	private GameObject _levelsTitle;
	private Image[] _levelsChapterTitles;
	private Button[] _levelButtons;

	private GameObject _interstitialsBack;
	
	private GameObject objToFadeOut;
	private GameObject objToFadeIn;
	
	private bool _levelsOpen;
	private bool _interstitialsOpen;
	private int _selectedChapter;
	private int _selectedLevel;

	private bool _animating;
	private bool _buttonsDisabled;

	// Use this for initialization
	void Start ()
	{
		// Start menu music
		GameObject.Find("AudioController").GetComponent<AudioControl>().Fade(MenuMusic);
		
		// Find settings objects, toggles and sliders
		_settingsBoard = Settings.transform.Find("Board").gameObject;
		_settingsTitle = Settings.transform.Find("Title").gameObject;
		_settingsBack = Settings.transform.Find("Buttons/Back").gameObject;
		_settingsLanguages = new[]
		{
			_settingsBoard.transform.Find("Language/Selector/Mask/English").gameObject,
			_settingsBoard.transform.Find("Language/Selector/Mask/Tamil").gameObject
		};

		if(GameConfig.CurrentLanguage == 1)
		{
			iTween.MoveTo(_settingsLanguages[0], iTween.Hash("position", new Vector3(0, 35, 0), "time", .001f, "islocal", true));
			iTween.MoveTo(_settingsLanguages[1], iTween.Hash("position", new Vector3(0, 0, 0), "time", .001f, "islocal", true));
		}

		_soundToggle = _settingsBoard.transform.Find("Sound/Toggle").GetComponent<Toggle>();
		_musicToggle = _settingsBoard.transform.Find("Music/Toggle").GetComponent<Toggle>();
		_volumeSlider = _settingsBoard.transform.Find("Volume/Slider").GetComponent<Slider>();

		_infoBoard = Info.transform.Find("Board").gameObject;
		_infoVersion = _infoBoard.transform.Find("Version").gameObject;
		_infoVersion.GetComponent<Text>().text = "v"+Application.version;
		_infoBack = Info.transform.Find("Buttons/Back").gameObject;
		
		// Find chapters objects
		_chaptersTitle = Chapters.transform.Find("Text").gameObject;
		_chapterSelect = Chapters.transform.Find("Select").gameObject;
		_chaptersBack = ChaptersParent.transform.Find("Buttons/Back").gameObject;
		
		// Find levels objects
		_levelsTitle = Levels.transform.Find("Header").gameObject;
		_levelsChapterTitles = _levelsTitle.GetComponentsInChildren<Image>(true);
		_levelButtons = Levels.transform.Find("Select").GetComponentsInChildren<Button>();
		_interstitialsBack = InterstitialsParent.transform.Find("BackButton").gameObject;
		
		// Set toggles and sliders to player pref settings
		_soundToggle.isOn = PlayerPrefs.GetInt("sound") == 1;
		_musicToggle.isOn = PlayerPrefs.GetInt("music") == 1;
		_volumeSlider.value = PlayerPrefs.GetFloat("volume");

		_buttonsDisabled = false;
		
	}

	public void Update()
	{
		if (_animating && !_buttonsDisabled)
		{
			foreach (Button button in _levelButtons)
				button.interactable = false;

			_chaptersBack.GetComponent<Button>().interactable = false;
			_infoBack.GetComponent<Button>().interactable = false;
			_settingsBack.GetComponent<Button>().interactable = false;
			_interstitialsBack.GetComponent<Button>().interactable = false;

			_buttonsDisabled = true;
		}
		else if (!_animating && _buttonsDisabled)
		{
			foreach (Button button in _levelButtons)
				button.interactable = true;

			_chaptersBack.GetComponent<Button>().interactable = true;
			_infoBack.GetComponent<Button>().interactable = true;
			_settingsBack.GetComponent<Button>().interactable = true;
			_interstitialsBack.GetComponent<Button>().interactable = true;

			_buttonsDisabled = false;
		}
	}

	public void CloseMainMenu(string uiToLoad)
	{

		var moveLeft = uiToLoad == "Settings";
		iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(moveLeft ? 540 : -540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "Open" + uiToLoad, "oncompletetarget", gameObject));

	}

	void OpenSettings()
	{
		
		Settings.SetActive(true);
		iTween.MoveTo(Settings, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack));

		iTween.ScaleFrom(_settingsTitle, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .1f));
		iTween.ScaleFrom(_settingsBoard, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .2f));
		iTween.ScaleFrom(_settingsBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .3f));
		
	}

	void OpenInfo()
	{
		
		Info.SetActive(true);
		iTween.MoveTo(Info, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack));

		iTween.ScaleFrom(_infoBoard, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .2f));
		iTween.ScaleFrom(_infoBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .3f));
		
	}

	public void CloseSettings()
	{

		iTween.MoveTo(Settings, iTween.Hash("position", new Vector3(-540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));

	}

	public void CloseInfo()
	{

		iTween.MoveTo(Info, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));

	}

	void OpenChapters()
	{
		ChaptersParent.SetActive(true);
		Chapters.SetActive(true);
		_levelsTitle.SetActive(false);
		_chapterButtons = GameObject.FindGameObjectsWithTag("ChapterButton");

		iTween.MoveTo(!_levelsOpen ? ChaptersParent : Chapters, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack));

		iTween.ScaleFrom(_chaptersTitle, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .1f));
		iTween.MoveTo(_chapterSelect, iTween.Hash("position", new Vector3(0, -50, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));
		iTween.PunchRotation(_chapterButtons[0], iTween.Hash("z", 90, "time", 1.5f, "delay", 1.1f));
		iTween.PunchRotation(_chapterButtons[1], iTween.Hash("z", -90, "time", 1.5f, "delay", 1.15f));
		iTween.PunchRotation(_chapterButtons[2], iTween.Hash("z", 90, "time", 1.5f, "delay", 1.2f, "oncomplete", "DoneAnimating", "oncompletetarget", gameObject));
		
		if(!_levelsOpen)
			iTween.ScaleFrom(_chaptersBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.2f));
		
		_levelsOpen = false;
		
	}
	
	public void OpenLevelSelect(int chapter=-1)
	{

		if (_animating)
			return;

		_animating = true;

		if (chapter > -1)
		{
			GameConfig.CurrentChapter = chapter;

			for ( int i = 0; i < _levelsChapterTitles.Length; i++ )
			{
				if (i == chapter)
					_levelsChapterTitles[i].gameObject.SetActive(true);
				else 
					_levelsChapterTitles[i].gameObject.SetActive(false);
			}
		}

		
		Levels.SetActive(true);
		_levelsTitle.SetActive(true);
		iTween.MoveTo(_chapterSelect, iTween.Hash("position", new Vector3(0, -850, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));

		var levelsDelay = 2.1f;
		if(!_interstitialsOpen)
		{
			iTween.PunchRotation(_chapterButtons[0], iTween.Hash("z", -90, "time", 1.5f, "delay", .5f));
			iTween.PunchRotation(_chapterButtons[1], iTween.Hash("z", 90, "time", 1.5f, "delay", .55f));
			iTween.PunchRotation(_chapterButtons[2], iTween.Hash("z", -90, "time", 1.5f, "delay", .6f));
			iTween.MoveTo(Chapters, iTween.Hash("position", new Vector3(-540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "delay", .7f));
		}
		else
			levelsDelay = 0;
		
		iTween.MoveTo(Levels, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.ScaleFrom(_levelsTitle, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", levelsDelay));
		iTween.MoveFrom(_levelButtons[0].gameObject, iTween.Hash("position", new Vector3(0, 850, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", levelsDelay));
		iTween.MoveFrom(_levelButtons[1].gameObject, iTween.Hash("position", new Vector3(0, 850, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", levelsDelay + .4f));
		iTween.PunchRotation(_levelButtons[0].gameObject, iTween.Hash("z", 90, "time", 1.5f, "delay", levelsDelay));
		iTween.PunchRotation(_levelButtons[1].gameObject, iTween.Hash("z", -90, "time", 1.5f, "delay", levelsDelay + .4f, "oncomplete", "DoneAnimating", "oncompletetarget", gameObject));

		if (_interstitialsOpen)
		{
			_chaptersBack.GetComponent<Transform>().localScale = Vector3.zero;
			iTween.ScaleTo(_chaptersBack, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.2f));
		}

		_interstitialsOpen = false;
		_levelsOpen = true;
	}

	void OpenSelectedChapter()
	{
		InterstitialsParent.SetActive(false);
		InterstitialsParent.transform.position = new Vector3(270.4f, 473.5f, 0.0f);
		
		_interstitialsOpen = true;
		OpenLevelSelect(_selectedLevel);
	}

	
	public void SaveLevelToLoad(string level)
	{
		LevelToLoad = (Levels) Enum.Parse(typeof(Levels), level);
	}

	public void ChaptersGoBack()
	{

		if (_animating)
			return;

		_animating = true;
		
		if(_levelsOpen)
			iTween.MoveTo(Levels, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "OpenChapters", "oncompletetarget", gameObject));
		else
		{
			iTween.MoveTo(ChaptersParent, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
			iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f, "oncomplete", "DoneAnimating", "oncompletetarget", gameObject));
		}

	}

	public void CloseInterstitials()
	{
		iTween.MoveTo(InterstitialsParent, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "OpenSelectedChapter", "oncompletetarget", gameObject));
	}


	public void OpenLevel()
	{
		GameConfig.Reset();
		GameConfig.LoadLevel();
	}

	public void Volume(float volume)
	{
		if (volume > 1f)
			volume = 1f;
		
		GameConfig.GlobalVolume = volume;
		GameConfig.UpdatePrefs("volume", null, volume);
		GameSound.volume = volume;
		GameMusic.volume = volume;
	}

	public void Sound()
	{
		GameConfig.SoundOn = _soundToggle.isOn;
		GameConfig.UpdatePrefs("sound", GameConfig.SoundOn ? 1 : 0);
		GameSound.mute = !GameConfig.SoundOn;
	}

	public void Music()
	{
		GameConfig.MusicOn = _musicToggle.isOn;
		GameConfig.UpdatePrefs("music", GameConfig.MusicOn ? 1 : 0);
		GameMusic.mute = !GameConfig.MusicOn;
	}

	public void ChangeLanguage()
	{
		var nextLang = GameConfig.CurrentLanguage == 0 ? 1 : 0;
		objToFadeOut = _settingsLanguages[GameConfig.CurrentLanguage];
		objToFadeIn = _settingsLanguages[nextLang];
		
		iTween.MoveTo(objToFadeOut, iTween.Hash("position", new Vector3(0, nextLang == 0 ? 35 : -35, 0), "time", .5f, "islocal", true));
		iTween.MoveTo(objToFadeIn, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true));
		
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", .2f, "onupdate", "FadeTextOut"));
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .2f, "onupdate", "FadeTextIn"));
		
		GameConfig.CurrentLanguage = nextLang;
		Events.instance.Raise (new LanguageChangeEvent());
	}

	private void FadeTextOut(float alpha)
	{
		objToFadeOut.GetComponent<CanvasRenderer>().SetAlpha(alpha);
	}
	private void FadeTextIn(float alpha)
	{
		objToFadeIn.GetComponent<CanvasRenderer>().SetAlpha(alpha);
	}

	private void DoneAnimating()
	{
		_animating = false;
	}
	
}
