using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

	public AudioClip MenuMusic;
	private AudioSource _audio;

	public Sprite ToggleOn;
	public Sprite ToggleOff;

	private Toggle SoundToggle;
	private Toggle MusicToggle;

	private Slider VolumeSlider;
	
	// Use this for initialization
	void Start () {
	
		_audio = GetComponent<AudioSource>();
		// Start menu music
		_audio.PlayOneShot(MenuMusic);
		
		// Find toggles and sliders
		SoundToggle = gameObject.transform.Find("Settings/Board/Sound/Toggle").GetComponent<Toggle>();
		MusicToggle = gameObject.transform.Find("Settings/Board/Music/Toggle").GetComponent<Toggle>();
		VolumeSlider = gameObject.transform.Find("Settings/Board/Volume/Slider").GetComponent<Slider>();

		// Set toggles and sliders to player pref settings
		SoundToggle.isOn = PlayerPrefs.GetInt("sound") == 1;
		MusicToggle.isOn = PlayerPrefs.GetInt("music") == 1;
		VolumeSlider.value = PlayerPrefs.GetFloat("volume");
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
