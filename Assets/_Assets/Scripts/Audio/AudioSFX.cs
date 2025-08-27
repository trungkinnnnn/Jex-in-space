using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    public static AudioSFX Instance;
    public float volume = 1f;
    public float duration = 3f;

    [SerializeField] AudioSource _audio;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
       
    }

    public void PlayAudioOneShortAndVolumeDownBackGround(List<AudioClip> clips, float per)
    {
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        AudioBGMManager.Instance.VolumeDownBackGround();
        _audio.PlayOneShot(clip, volume * per);
    }    

    public void PlayAudioOneShort(List<AudioClip> clips)
    {
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        _audio.PlayOneShot(clip, volume);
    }

    public void PlayAudioOneShortChangeVolume(List<AudioClip> clips, float per)
    {
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        _audio.PlayOneShot(clip, volume * per);
    }

    public IEnumerator PlayAudioVolumeLoop(AudioSource audio, AudioClip clip, float per)
    {
        audio.clip = clip;
        float targetVolume = audio.volume * per;
        audio.volume = 0f;
        audio.Play();

        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;    
            audio.volume = Mathf.Lerp(0f, targetVolume, timer/duration);
            yield return null;
        }    
         
        audio.volume = targetVolume;
    }    
}
