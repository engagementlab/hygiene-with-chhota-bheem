using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private Dictionary<string, AudioClip> _loadedAudio;

    private AudioSource _sound;
    private AudioSource _music;
    private DoubleAudioSource _doubleAudioSource;

    [HideInInspector] 
    public GameObject audioPlayer;
    [HideInInspector] 
    public AudioSource player;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        Setup();
    }
    
    private void Setup()
    {        
        _music = transform.Find("Music").GetComponent<AudioSource>();
        _sound = transform.Find("Sound").GetComponent<AudioSource>();
        _loadedAudio = new Dictionary<string, AudioClip>();

        _doubleAudioSource = GetComponentInChildren<DoubleAudioSource>();
		
        Events.instance.AddListener<SoundEvent> (OnSoundEvent);
    }

    void OnSceneLoaded()
    {
        _music.Stop();
    }

    private void OnSoundEvent(SoundEvent e)
    {
        audioPlayer = null;
		
        if(_music == null || _sound == null) 
            return;
		
        // If this is SFX and the sound is OFF, stop here
        if (e.Type == SoundEvent.SoundType.SFX && !GameConfig.SoundOn)
            return;
		
        // If this is Music and the music is OFF, stop here
        if (e.Type == SoundEvent.SoundType.Music && !GameConfig.MusicOn)
            return;
		
        // If pitch is altered, play sound on temp audiosource
        if(e.SoundPitch != 1)
        {
            audioPlayer = new GameObject();
			
            audioPlayer.AddComponent<AudioSource>();
            audioPlayer.GetComponent<AudioSource>().pitch = e.SoundPitch;
        }

        bool isMusic = e.Type == SoundEvent.SoundType.Music;
        AudioSource audio = isMusic ? _music : _sound;
        audio.volume = e.SoundVolume * GameConfig.GlobalVolume;
        
        player = audioPlayer == null ? audio : audioPlayer.GetComponent<AudioSource>();
        
        if(e.SoundFileName != null)
        {
            // If sound name provided and clip not loaded, load into dictionary
            if(!_loadedAudio.ContainsKey(e.SoundFileName))
                _loadedAudio.Add(e.SoundFileName, Resources.Load<AudioClip>("Audio/" + e.Type + "/" + e.SoundFileName));

            if(!e.FadeClip)
            {
                // Play loaded clip
                if(isMusic)
                {
                    player.clip = _loadedAudio[e.SoundFileName]; 
                    player.Play();
                }
                else
                    player.PlayOneShot(_loadedAudio[e.SoundFileName]);
            } 
            else
                Fade(_loadedAudio[e.SoundFileName], isMusic, e.SoundVolume * GameConfig.GlobalVolume);
        }
        // Otherwise, play provided clip
        else if(e.SoundClip != null)
        {
            if(!e.FadeClip)
                player.PlayOneShot(e.SoundClip, e.SoundVolume * GameConfig.GlobalVolume);
            else
                Fade(e.SoundClip, isMusic, e.SoundVolume * GameConfig.GlobalVolume);
        }

        if(audioPlayer != null)
            Destroy(audioPlayer, e.SoundClip.length);
    }

    public void UpdateVolume(float newVolume)
    {
        _music.volume = newVolume;
        _sound.volume = newVolume;
        _doubleAudioSource.UpdateVolume(newVolume);
    }

    public void MuteMusicOnOff(bool mute)
    {
        _doubleAudioSource.MuteOnOff(mute);
    }

    public AudioSource WhichMusicPlayer()
    {
        if (audioPlayer != null)
            return audioPlayer.GetComponent<AudioSource>();
        else
            return _music;
    }

    public void Fade(AudioClip newClip, bool loop=false, float volume=1)
    {
        _doubleAudioSource.CrossFade(newClip, volume, 1f, 0, loop);
    }
    

}
