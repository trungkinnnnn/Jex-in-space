using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public class AudioBGMManager : MonoBehaviour
{
    [Header("BackGroundListMusic")]
    [SerializeField] AudioBGMData _data;

    [Header("AudioSources")]
    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioSource _pauseSource;

    [Header("Setting")]
    public float bgVolume = 1.0f;
    public float bgVolumeDuckPer = 0.8f;
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;    
    public float pauseBetweenTracks = 2f;
    public float pauseTime = 1f;


    private Coroutine _bgmCoroutine;
    private Coroutine _pauseCoroutine;
    private Coroutine _duckCoroutine;
    private float _bgPauseTime = 0f;
    private float _defaultVolume;

    private bool _isActive;

    public static AudioBGMManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }         
    }

    private void Start()
    {
        _isActive = LoadingData.Instance.ActiveSoundMusic();
        _defaultVolume = bgVolume;
        _pauseSource.clip = _data.pauseClips[0];
        StartCoroutine(WaitStartMusicMenu());
    }

    public IEnumerator WaitStartMusicMenu()
    {
        yield return new WaitForSeconds(2f);
        PlayMenuBGM();
    }

    public IEnumerator WaitStartMusicInGame()
    {
        yield return new WaitForSeconds(1f);
        PlayInGameBGM();
    }

    private void PlayBGMList(List<AudioClip> list)
    {
        if (list == null || list.Count <= 0) return;

        if(_bgmCoroutine != null)
        {
            StartCoroutine(FadeOutAndChangeBGM(list));  
        }
        else
        {
            _bgmCoroutine = StartCoroutine(LoopBGM(list));
        }    
    }    

    private IEnumerator LoopBGM(List<AudioClip> list)
    {
        while(true)
        {
            AudioClip clip = list.Count <= 1 ? list[0] : list[Random.Range(0, list.Count)];

            yield return StartCoroutine(FadeInBGM(clip, fadeInTime));

            float playTime = clip.length - fadeOutTime;
            if (playTime > 0)
                yield return new WaitForSeconds(playTime);

            yield return StartCoroutine(FadeOutBGM(fadeOutTime));

            yield return new WaitForSeconds(pauseBetweenTracks);
        }
    }

    private IEnumerator FadeInBGM(AudioClip clip, float duration)
    {
        _bgmSource.clip = clip;
        _bgmSource.volume = 0f;
        _bgmSource.Play();

         yield return StartCoroutine(WhileLerp(_bgmSource, 0, bgVolume, duration));

        _bgmSource.volume = bgVolume;
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVol = _bgmSource.volume;

        yield return StartCoroutine(WhileLerp(_bgmSource, startVol, 0, duration));
       
        _bgmSource.Stop();
        _bgmSource.volume = bgVolume;
    }

    private IEnumerator FadeOutAndChangeBGM(List<AudioClip> list)
    {
        yield return StartCoroutine(FadeOutBGM(fadeOutTime));
        
        if(_bgmCoroutine != null) StopCoroutine(_bgmCoroutine);
        _bgmCoroutine = StartCoroutine(LoopBGM(list));
    }    



    private IEnumerator WhileLerp(AudioSource audio, float start, float end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            audio.volume = Mathf.Lerp(start, end, timer/duration);
            yield return null;
        }    
    }

    public void PlayMenuBGM()
    {
        if (_pauseSource.isPlaying)
        {
            StartCoroutine(FadeOutAndPause(_pauseSource, pauseTime - 1f));
        }

        PlayBGMList(_data.menuClips);
    }

    public void PlayInGameBGM()
    {
        if (_pauseSource.isPlaying)
        {
            StartCoroutine(FadeOutAndPause(_pauseSource, pauseTime - 1f));
        }

        PlayBGMList(_data.inGameClips);
    }

    public void PauseBGM()
    {
        if(_bgmSource.isPlaying)
        {
            _bgPauseTime = _bgmSource.time;
            if(_pauseCoroutine != null) StopCoroutine(_pauseCoroutine);
            _pauseCoroutine = StartCoroutine(FadeOutAndPause(_bgmSource, pauseTime));
        }

        if (_pauseSource != null)
        {
            _pauseSource.Play();
            StartCoroutine(FadeIn(_pauseSource, pauseTime + 1f));
        }

    }    

    public void ResumeBGM()
    {

        if (_pauseSource.isPlaying)
        {
            StartCoroutine(FadeOutAndPause(_pauseSource, pauseTime - 1f));
        }

        if (_bgPauseTime > 0f)
        {
            _bgmSource.time = _bgPauseTime;
            _bgmSource.Play();
            if(_pauseCoroutine != null) StopCoroutine(_pauseCoroutine);
            _pauseCoroutine = StartCoroutine(FadeIn(_bgmSource, pauseTime));
        }
    }

    private IEnumerator FadeOutAndPause(AudioSource audio, float duration)
    {
        float startVol = audio.volume;
        yield return StartCoroutine(WhileLerp(audio, startVol, 0, duration));
        audio.volume = 0f;
        audio.Pause();
    }

    private IEnumerator FadeIn(AudioSource audio, float duration)
    {
        audio.volume = 0f;
        float startVol = bgVolume;
        yield return StartCoroutine(WhileLerp(audio, 0, startVol, duration));
        audio.volume = startVol;
    }


    public void VolumeDownBackGround()
    {
        if(_duckCoroutine != null) StopCoroutine(_duckCoroutine);
        _duckCoroutine = StartCoroutine(DuckBackGround());
    }

    private IEnumerator DuckBackGround()
    {
        float targetVol = _bgmSource.volume * bgVolumeDuckPer;
        float startVol = _bgmSource.volume;

        yield return StartCoroutine(WhileLerp(_bgmSource, startVol, targetVol, 0.3f));
        _bgmSource.volume = targetVol;

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(WhileLerp(_bgmSource, targetVol, startVol, 0.3f));
        _bgmSource.volume = startVol;
    }

    public void MuteAudioPause()
    {
        StartCoroutine(MuteAudio(_pauseSource));
    }

    private IEnumerator MuteAudio(AudioSource audio)
    {
        float startVol = audio.volume;
        yield return StartCoroutine(WhileLerp(audio, startVol, 0f, 0.3f));
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        if (_isActive)
        {
            bgVolume = _defaultVolume;
            MusicOn();
        }
        else
        {
           StartCoroutine(MusicOff());
        }    
    }    

    private void MusicOn()
    {
        StartCoroutine(WhileLerp(_bgmSource, 0f, bgVolume, 1f));
        StartCoroutine(WhileLerp(_pauseSource, 0f, bgVolume, 1f));
    }  
    
    private IEnumerator MusicOff()
    {
        StartCoroutine(WhileLerp(_bgmSource, bgVolume, 0f, 1f));
        yield return StartCoroutine(WhileLerp(_pauseSource, bgVolume, 0f, 1f));
        bgVolume = 0f;
    }

}
