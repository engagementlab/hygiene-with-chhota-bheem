using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

	public GameObject MainMenu;
	public GameObject Settings;
	public GameObject Chapters;
	public GameObject Interstitials;
 
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

	private GameObject _chaptersTitle;
	private GameObject _chapterSelect;
	private GameObject _chaptersBack;
	private GameObject[] _chapterButtons;
	
	// Use this for initialization
	void Start () {
	
		_audio = GetComponent<AudioSource>();
		// Start menu music
		_audio.PlayOneShot(MenuMusic);
		
		// Find settings objects, toggles and sliders
		_settingsBoard = Settings.transform.Find("Board").gameObject;
		_settingsTitle = Settings.transform.Find("Title").gameObject;
		_settingsBack = Settings.transform.Find("Buttons/Back").gameObject;
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

	public void Screen(int num)
	{
		gameObject.GetComponent<Animator>().SetInteger("Screen", num);
	}

	public void Level(int num)
	{
		gameObject.GetComponent<Animator>().SetInteger("LevelSelected", num);
	}

	public void Volume(float volume)
	{
		if (volume > 1f)
			volume = 1f;
		
		GameConfig.GlobalVolume = volume;
		GameConfig.UpdatePrefs("volume", null, volume);
		Debug.Log(GameConfig.GlobalVolume);
	}

	public void Sound()
	{
		GameConfig.SoundOn = !GameConfig.SoundOn;
		GameConfig.UpdatePrefs("sound", GameConfig.SoundOn == true ? 1 : 0);
		Debug.Log(GameConfig.SoundOn);
	}

	public void Music()
	{
		GameConfig.MusicOn = !GameConfig.MusicOn;
		GameConfig.UpdatePrefs("music", GameConfig.MusicOn == true ? 1 : 0);
		Debug.Log(GameConfig.MusicOn);
	}
}
