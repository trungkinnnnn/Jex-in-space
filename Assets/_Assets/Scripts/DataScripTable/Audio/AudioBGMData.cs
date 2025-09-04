
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Audio/DataBGM"))]
public class AudioBGMData : ScriptableObject
{
    public List<AudioClip> menuClips;
    public List<AudioClip> inGameClips;
    public List<AudioClip> pauseClips;
}
