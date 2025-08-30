using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioSFX : MonoBehaviour
{
    public static AudioSFX Instance;
    public float volume = 1f;
    public float duration = 3f;

    [SerializeField] AudioSource _audio;

    private List<(AudioSource, float)> _listAudio = new();
    private bool _isActive;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _isActive = LoadingData.Instance.ActiveSoundFX();
    }

    public void PlayAudioOneShortAndVolumeDownBackGround(List<AudioClip> clips, float per)
    {
        if (!_isActive) return;
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        AudioBGMManager.Instance.VolumeDownBackGround();
        _audio.PlayOneShot(clip, volume * per);
    }    

    public void PlayAudioOneShort(List<AudioClip> clips)
    {
        if (!_isActive) return;
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        _audio.PlayOneShot(clip, volume);
    }

    public void PlayAudioOneShortChangeVolume(List<AudioClip> clips, float per)
    {
        if (!_isActive) return;
        //Debug.Log("Log : " + "Volume" + volume + "Target" + volume * per);
        AudioClip clip = clips.Count <= 1 ? clips[0] : clips[Random.Range(0, clips.Count)];
        _audio.PlayOneShot(clip, volume * per);
    }

    public void PlayAudioOneShortOneClip(AudioClip clip, float per)
    {
        _audio.PlayOneShot(clip, volume * per);
    }

    public IEnumerator PlayAudioVolumeLoop(AudioSource audio, AudioClip clip, float per)
    {
        _listAudio.Add((audio, per));
        RemoveItemList();

        if (!_isActive) per = 0f;

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
    
    public void SetActive(bool active)
    {
        _isActive = active;
        RemoveItemList();

        if (_isActive) OnMusic();
        else OffMusic();
    }    

    private void OnMusic()
    {
        _audio.volume = volume;
        if (_listAudio.Count <= 0) return;

        for(int i = 0;i < _listAudio.Count;i++)
        {
            _listAudio[i].Item1.volume = volume * _listAudio[i].Item2;
        }    
    }

    private void OffMusic()
    {
        _audio.volume = 0f;
        if (_listAudio.Count <= 0) return;

        for (int i = 0; i < _listAudio.Count; i++)
        {
            _listAudio[i].Item1.volume = 0f;
        }
    }

    private void RemoveItemList()
    {
        _listAudio.RemoveAll(s => s.Item1 == null);
    }    

}
