using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("BackGroundListMusic")]
    [SerializeField] List<AudioClip> _menuList;
    [SerializeField] List<AudioClip> _inGameList;
    [SerializeField] List<AudioClip> _pauseList;

    [Header("AudioSources")]
    [SerializeField] AudioSource _bgmSource;

    [Header("Setting")]
    public float bgVolume = 1.0f;
    public float bgVolumeDuckPer = 0.4f;
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;    
    public float pauseBetweenTracks = 2f;

    private Coroutine _bgmCoroutine;

    public static AudioManager Instance;

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
        StartCoroutine(WaitStartMusicMenu());
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

    private IEnumerator DuckBackGround()
    {
        float targetVol = _bgmSource.volume * bgVolumeDuckPer;
        float startVol = _bgmSource.volume;

        yield return StartCoroutine(WhileLerp(_bgmSource, startVol, targetVol, 0.5f));
        _bgmSource.volume = targetVol;

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(WhileLerp(_bgmSource, targetVol, startVol, 2f));
        _bgmSource.volume = startVol;
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

    public void PlayMenuBGM() => PlayBGMList(_menuList);
    public void PlayInGameBGM() => PlayBGMList(_inGameList);
    public void PlayPauseBGM() => PlayBGMList(_pauseList);

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

}
