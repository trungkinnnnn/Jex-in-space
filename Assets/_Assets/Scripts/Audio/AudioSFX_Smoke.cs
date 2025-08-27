using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX_Smoke : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    private AudioSource _audio;
    public float volume = 0.7f;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(AudioSFX.Instance.PlayAudioVolumeLoop(_audio, _clip, volume));
    }

    private void OnDestroy()
    {
        if (_audio != null)
            _audio.Stop();
    }
}
