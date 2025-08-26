using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource _sysSource;
    [SerializeField] AudioClip _clipClick;
    [SerializeField] AudioClip _clipGameOver;

    public static AudioSystem Instance;
    public float volume = 0.4f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }    
    }

    public void PlayAudioClick()
    {
        _sysSource.PlayOneShot(_clipClick, volume);
    }

    public void PlayAudioGameOver()
    {
        _sysSource.PlayOneShot(_clipGameOver, volume);
    }
}
