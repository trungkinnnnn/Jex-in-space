
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioSFXPlayer")]
public class AudioPlayer : ScriptableObject
{
    public List<AudioClip> clipsImpactWall;
    public List<AudioClip> clipsMeoWall;
    public List<AudioClip> clipsTakeCoin;
    public List<AudioClip> clipsEat;
    public List<AudioClip> clipsHurt;
    public List<AudioClip> clipDie;
    public List<AudioClip> clipGlassBreak;
}
