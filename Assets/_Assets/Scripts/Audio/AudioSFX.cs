using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    public static AudioSFX Instance;

    public float volume;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayAudioOneShortAndVolumeDownBackGround(AudioSource audioSource, List<AudioClip> clips, float per)
    {
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        AudioBGMManager.Instance.VolumeDownBackGround();
        audioSource.PlayOneShot(clip, volume * per);
    }    

    public void PlayAudioOneShort(AudioSource audioSource, List<AudioClip> clips)
    {
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayAudioOneShortChangeVolume(AudioSource audioSource, List<AudioClip> clips, float per)
    {
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        audioSource.PlayOneShot(clip, volume * per);
    }
}
