using System;
using UnityEngine;

public class SoundEvent : GameEvent
{

  public readonly string SoundFileName;
  public readonly SoundType Type;
  public readonly AudioClip SoundClip;
  public readonly float SoundVolume;

  public enum SoundType
  {
     SFX,
     Music
  }

  public SoundEvent (string name, SoundType type, AudioClip clip=null, float volume=1)
  {
    SoundClip = clip;
    Type = type;
    SoundFileName = name;
    SoundVolume = volume;
  }

  // Use provided audio clip instead of finding Resource
  public static SoundEvent WithClip(AudioClip clip, SoundType type=SoundType.SFX)
  {
    var evt = new SoundEvent(null, type, clip);
    return evt;
  }
}
