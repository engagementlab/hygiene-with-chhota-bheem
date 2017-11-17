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
	public GameObject Chapters;
	public GameObject Levels;
	public GameObject[] Interstitials;
	public GameObject[] InterstitialScreens;

	private GameObject _interstitialScreen;
	private int _interstitialScreenCount;

	private Sprite[] _interstitialImages;
//	private List<Transform> _interstitialScreens;
 
	public AudioClip MenuMusic;
	private AudioSource _audio;

	public Sprite ToggleOn;
	public Sprite ToggleOff;

	public Levels LevelToLoad;

	private Toggle _soundToggle;
	private Toggle _musicToggle;

	private Slider _volumeSlider;

	private GameObject _settingsTitle;
	private GameObject _settingsBoard;
	private GameObject _settingsBack;
	private GameObject[] _settingsLanguages;

	private GameObject _chaptersTitle;
	private GameObject _chapterSelect;
	private GameObject _chaptersBack;
	private GameObject[] _chapterButtons;

	private GameObject objToFadeOut;
	private GameObject objToFadeIn;
		
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
		
		// Find chapters objects
		_chaptersTitle = Chapters.transform.Find("Text").gameObject;
		_chapterSelect = Chapters.transform.Find("Select").gameObject;
		_chaptersBack = Chapters.transform.Find("Buttons/Back").gameObject;

		// Set toggles and sliders to player pref settings
		_soundToggle.isOn = PlayerPrefs.GetInt("sound") == 1;
		_musicToggle.isOn = PlayerPrefs.GetInt("music") == 1;
		_volumeSlider.value = PlayerPrefs.GetFloat("volume");
	}

	public void CloseMainMenu(bool goToSettings)
	{

		iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(goToSettings ? 540 : -540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", goToSettings ? "OpenSettings" : "OpenChapterSelect", "oncompletetarget", gameObject));

	}

	void OpenSettings()
	{
		
		Settings.SetActive(true);
		iTween.MoveTo(Settings, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack));

		iTween.ScaleFrom(_settingsTitle, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .1f));
		iTween.ScaleFrom(_settingsBoard, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .2f));
		iTween.ScaleFrom(_settingsBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .3f));
		
	}

	public void CloseSettings()
	{

		iTween.MoveTo(Settings, iTween.Hash("position", new Vector3(-540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));

	}

	void OpenChapterSelect()
	{
		
		Chapters.SetActive(true);
		_chapterButtons = GameObject.FindGameObjectsWithTag("ChapterButton");

		iTween.MoveTo(Chapters, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack));

		iTween.ScaleFrom(_chaptersTitle, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", .1f));
		iTween.MoveFrom(_chapterSelect, iTween.Hash("position", new Vector3(0, -850, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));
		iTween.PunchRotation(_chapterButtons[0], iTween.Hash("z", 90, "time", 1.5f, "delay", 1.1f));
		iTween.PunchRotation(_chapterButtons[1], iTween.Hash("z", -90, "time", 1.5f, "delay", 1.15f));
		iTween.PunchRotation(_chapterButtons[2], iTween.Hash("z", 90, "time", 1.5f, "delay", 1.2f));
		iTween.ScaleFrom(_chaptersBack, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 1.2f));
		
	}
	

	public void CloseChapterSelect()
	{
		
		iTween.MoveTo(Chapters, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveTo(MainMenu, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));

	}

	public void OpenLevelSelect()
	{
		Levels.SetActive(true);
		iTween.MoveTo(Chapters, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveTo(Levels, iTween.Hash("position", new Vector3(0, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1.1f));

	}

	public void SaveLevelToLoad(string level)
	{
		LevelToLoad = (Levels) Enum.Parse(typeof(Levels), level);
	}

	public void OpenLevelInterstitial(int chapter)
	{
		
		iTween.MoveTo(Levels, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		gameObject.transform.Find("Interstitials").gameObject.SetActive(true);
		
		gameObject.transform.Find("Interstitials/PlayButton").gameObject.SetActive(false);
		gameObject.transform.Find("Interstitials/NextButton").gameObject.SetActive(true);

		_interstitialScreen = gameObject.transform.Find("Interstitials/Background/Image").gameObject;
		Debug.Log(_interstitialScreen.GetComponent<Image>().sprite.name);
		_interstitialScreenCount = 0;
		
		switch (chapter)
		{
			case 1:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapOneInterstitials");
				break;
				
			case 2:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapTwoInterstitials");
				break;
				
			case 3:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapThreeInterstitials");
				break;
		}
		
		_interstitialScreen.GetComponent<Image>().sprite = _interstitialImages[_interstitialScreenCount];
//		_interstitialScreen.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, Mathf.PingPong(Time.time, 1));
				
		iTween.ScaleFrom(gameObject.transform.Find("Interstitials/NextButton").gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.3, "easetype", iTween.EaseType.easeOutElastic, "delay", 0.2f));
				
	}

	public void NextInterstitial()
	{
//		_interstitialScreen.GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 1));

		_interstitialScreenCount++;

		if (_interstitialScreenCount == 3) // Final screen 
		{
			// Show play button
			gameObject.transform.Find("Interstitials/PlayButton").gameObject.SetActive(true);
			gameObject.transform.Find("Interstitials/NextButton").gameObject.SetActive(false);
		}
		
		_interstitialScreen.GetComponent<Image>().sprite = _interstitialImages[_interstitialScreenCount];
//		_interstitialScreen.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, Mathf.PingPong(Time.time, 1));

	}

	private void InterstitialColor()
	{
		
	}

	public void PlayLevel()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/AlphaScenes/" + LevelToLoad.ToString());
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
		int nextLang = GameConfig.CurrentLanguage == 0 ? 1 : 0;
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
