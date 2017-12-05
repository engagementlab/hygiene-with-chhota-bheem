using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AudioControl : MonoBehaviour
{
    private Dictionary<string, AudioClip> _loadedAudio;

    private AudioSource _sound;
    private AudioSource _music;

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
        player = audioPlayer == null ? audio : audioPlayer.GetComponent<AudioSource>();
        
        if(e.SoundFileName != null)
        {
            // If sound name provided and clip not loaded, load into dictionary
            if(!_loadedAudio.ContainsKey(e.SoundFileName))
                _loadedAudio.Add(e.SoundFileName, Resources.Load<AudioClip>("Audio/" + e.Type + "/" + e.SoundFileName));

            // Play loaded clip
            player.PlayOneShot(_loadedAudio[e.SoundFileName], e.SoundVolume * GameConfig.GlobalVolume);
        }
        // Otherwise, play provided clip
        else if(e.SoundClip != null) 
            player.PlayOneShot(e.SoundClip, e.SoundVolume * GameConfig.GlobalVolume);
		
        if(audioPlayer != null)
            Destroy(audioPlayer, e.SoundClip.length);
    }

    public AudioSource WhichMusicPlayer()
    {
        if (audioPlayer != null)
            return audioPlayer.GetComponent<AudioSource>();
        else
            return _music;
    }
    

}
