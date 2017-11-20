using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;

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

	private Transform[] _interstitialScreens;
 
	public AudioClip MenuMusic;
	private AudioSource _audio;

	public Sprite ToggleOn;
	public Sprite ToggleOff;

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
	private Button[] _levelButtons;
	
	private GameObject _interstitialsBack;
	
	private GameObject objToFadeOut;
	private GameObject objToFadeIn;

	private bool _levelsOpen;
	private bool _interstitialsOpen;
	private int _selectedChapter;
	private int _selectedLevel;
	
	// Use this for initialization
	void Start () {
	
		_audio = GetComponent<AudioSource>();
		// Start menu music
		_audio.PlayOneShot(MenuMusic);
		
		// Find settings objects, toggles and sliders
		_settingsBoard = Settings.transform.Find("Board").gameObject;
		_settingsTitle = Settings.transform.Find("Title").gameObject;
		_settingsBack = Settings.transform.Find("Buttons/Back").gameObject;
		_settingsLanguages = new[]
		{
			_settingsBoard.transform.Find("Language/Selector/Mask/English").gameObject,
			_settingsBoard.transform.Find("Language/Selector/Mask/Tamil").gameObject
		};
		
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
		_levelButtons = Levels.transform.Find("Select").GetComponentsInChildren<Button>();
//		_interstitialsBack = InterstitialsParent.transform.Find("Buttons/Back").gameObject;
		
		// Set toggles and sliders to player pref settings
		_soundToggle.isOn = PlayerPrefs.GetInt("sound") == 1;
		_musicToggle.isOn = PlayerPrefs.GetInt("music") == 1;
		_volumeSlider.value = PlayerPrefs.GetFloat("volume");
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
		iTween.PunchRotation(_chapterButtons[2], iTween.Hash("z", 90, "time", 1.5f, "delay", 1.2f));
		
		if(!_levelsOpen)
			iTween.ScaleFrom(_chaptersBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.2f));
		
		_levelsOpen = false;
		
	}
	
	public void OpenLevelSelect(int chapter=0)
	{

		if(chapter > 0)
			GameConfig.CurrentChapter = chapter;		
		
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
		iTween.PunchRotation(_levelButtons[1].gameObject, iTween.Hash("z", -90, "time", 1.5f, "delay", levelsDelay + .4f));
		
		if(_interstitialsOpen)
			iTween.ScaleFrom(_chaptersBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.2f));

		_interstitialsOpen = false;
		_levelsOpen = true;
	}

	void OpenSelectedChapter()
	{
		OpenLevelSelect(_selectedLevel);
	}

	public void ChaptersGoBack()
	{
		
		if(_levelsOpen)
			iTween.MoveTo(Levels, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "OpenChapters", "oncompletetarget", gameObject));
		else
		{
			iTween.MoveTo(ChaptersParent, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
			iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));
		}

	}
	
	public void OpenLevelInterstitial(int level)
	{
		
		_levelsOpen = false;
		_interstitialsOpen = true;
		GameConfig.CurrentLevel = level;
		
		iTween.ScaleTo(_chaptersBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInElastic));
//		iTween.ScaleFrom(_interstitialsBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .6f));
		
		iTween.MoveTo(Levels, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		gameObject.transform.Find("Interstitials").gameObject.SetActive(true);

		foreach (var screen in Interstitials)
			screen.SetActive(false);
		
		switch (level)
		{
			case 1:
				Interstitials[0].gameObject.SetActive(true);
				iTween.MoveTo(Interstitials[0], iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack) );
				_interstitialScreens = Interstitials[0].GetComponentsInChildren<Transform>();

				break;
				
			case 2:
				Interstitials[1].gameObject.SetActive(true);
				iTween.MoveTo(Interstitials[1], iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack) );
				_interstitialScreens = Interstitials[1].GetComponentsInChildren<Transform>();

				break;
				
			case 3:
				Interstitials[2].gameObject.SetActive(true);
				iTween.MoveTo(Interstitials[2], iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack) );
				_interstitialScreens = Interstitials[2].GetComponentsInChildren<Transform>();

				break;
		}
				
	}

	public void NextInterstitial(Transform current)
	{
		iTween.MoveTo(current.gameObject, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
	}

	public void CloseInterstitials()
	{
		iTween.MoveTo(InterstitialsParent, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "OpenSelectedChapter", "oncompletetarget", gameObject));
	}

	public void OpenLevel()
	{
		GameConfig.LoadLevel();
	}

	public void Volume(float volume)
	{
		if (volume > 1f)
			volume = 1f;
		
		GameConfig.GlobalVolume = volume;
		GameConfig.UpdatePrefs("volume", null, volume);
	}

	public void Sound()
	{
		GameConfig.SoundOn = !GameConfig.SoundOn;
		GameConfig.UpdatePrefs("sound", GameConfig.SoundOn ? 1 : 0);
	}

	public void Music()
	{
		GameConfig.MusicOn = !GameConfig.MusicOn;
		GameConfig.UpdatePrefs("music", GameConfig.MusicOn ? 1 : 0);
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
	
}
