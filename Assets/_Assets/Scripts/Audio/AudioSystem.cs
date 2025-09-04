
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource _sysSource;
    [SerializeField] AudioClip _clipClick;
    [SerializeField] AudioClip _clipGameOver;
    [SerializeField] AudioClip _clipNextWave;

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

    public void PlayNextWave()
    {
        _sysSource.PlayOneShot(_clipNextWave, volume);
    }    
}
