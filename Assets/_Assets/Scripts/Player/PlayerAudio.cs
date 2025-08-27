using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioPlayer _audioPlayer;
    private AudioSource _audioSource;

    private float _timeHurt = 3f;
    private float _lastTimeHurt;
    private float _timeImpactWall = 4f;
    private float _lastTimeImpactWall;

    public float volume = 1.0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClipImpactWall()
    {
        if(Time.time - _lastTimeImpactWall > _timeImpactWall)
        {
            _lastTimeImpactWall = Time.time + _timeImpactWall;
            AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioPlayer.clipsMeoWall, volume - 0.2f);
        }    
       
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioPlayer.clipsImpactWall, volume + 0.5f);
    }

    public void PlayClipTakeCoin()
    {
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioPlayer.clipsTakeCoin, volume - 0.6f);
    }

    public void PlayClipHurt()
    {
        if(Time.time - _lastTimeHurt > _timeHurt)
        {
            _lastTimeHurt = Time.time + _timeHurt;  
            AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioPlayer.clipsHurt, volume - 0.4f);
        }    
        
    }

    public void PlayClipEat()
    {
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioPlayer.clipsEat, volume + 2.5f);
    }

    public void PlayClipDie()
    {
        AudioSFX.Instance.PlayAudioOneShort(_audioPlayer.clipDie);
    }

    public void PlayClipGlassBreak()
    {
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioPlayer.clipGlassBreak, volume + 3f);
    }

}
