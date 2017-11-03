using UnityEngine;

public class MenuUI : MonoBehaviour
{

	public Animator SettingsAnimator;
	public AudioClip MenuMusic;
	private AudioSource _audio;
	
	
	// Use this for initialization
	void Start () {
	
		_audio = GetComponent<AudioSource>();
		// Start menu music
		_audio.PlayOneShot(MenuMusic);
		
	}

	public void SettingsButtonDown()
	{
		
	}

	public void OpenSettings()
	{
		SettingsAnimator.gameObject.SetActive(true);
		SettingsAnimator.Play("SettingsOpen");
	}
}
