using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class SuptitleNextWave : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] List<Vector2> _iconWavePosition = new List<Vector2> { new Vector2(0f, 700f), new Vector2 (0f, 639f) };
    [SerializeField] List<Vector2> _supTitlePosition = new List<Vector2> { new Vector2(1111f, -43f), new Vector2(0f, -43f) };
    [SerializeField] List<Vector2> _waveCurrentPosition = new List<Vector2> { new Vector2(-1111f, 0f), new Vector2(0f, 0f) };
    [SerializeField] List<Vector2> _waveBestPosition = new List<Vector2> { new Vector2(0f, -1605f), new Vector2(0f, -125f) };

    [Header("Object")]
    [SerializeField] GameObject _iconWave;
    [SerializeField] GameObject _supTitle;
    [SerializeField] GameObject _waveCurrent;
    [SerializeField] GameObject _waveBest;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textWaveCurrent;
    [SerializeField] TextMeshProUGUI _textWaveBest;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup _canvas;

    private int _waveHigh;

    public float duration = 1f;

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        _iconWave.SetActive(false);
        _supTitle.SetActive(false);
        _waveCurrent.SetActive(false);  
        _waveBest.SetActive(false);

        _waveHigh = PlayerPrefs.GetInt(DataPlayerPrefs.para_HIGHWAVE, 0);
        _textWaveBest.text = _waveHigh.ToString();

        _canvas.alpha = 0f;
    }

    private void OnEnable()
    {
        WaveManager.GetWave += HandleOnSuptitle;
    }

    private void OnDisable()
    {
        WaveManager.GetWave -= HandleOnSuptitle;
    }    

    private void HandleOnSuptitle(int currentWave)
    {
        CheckWave(currentWave);
        StartTitle();
        MoveSuptitle();
        StartCoroutine(ShowOff());
    }    

    private void CheckWave(int currentWave)
    {
        if(currentWave > _waveHigh)
        {
            currentWave = _waveHigh;
            _textWaveCurrent.text = currentWave.ToString();
            _textWaveBest.text = currentWave.ToString();
        }
        else
        {
            _textWaveCurrent.text = currentWave.ToString();
        }
    }    

    private void StartTitle()
    {
        _iconWave.SetActive(true);
        _supTitle.SetActive(true);
        _waveCurrent.SetActive(true);
        _waveBest.SetActive(true);

        _iconWave.transform.localPosition = _iconWavePosition[0];
        _supTitle.transform.localPosition = _supTitlePosition[0];
        _waveCurrent.transform.localPosition = _waveCurrentPosition[0];
        _waveBest.transform.localPosition = -_waveBestPosition[0];

        _canvas.alpha = 1f;
    }    

    private void MoveSuptitle()
    {
        AudioSystem.Instance.PlayNextWave();
        _iconWave.transform.DOLocalMove(_iconWavePosition[1], duration).SetEase(Ease.OutQuad);
        _supTitle.transform.DOLocalMove(_supTitlePosition[1], duration).SetEase(Ease.OutQuad);
        _waveCurrent.transform.DOLocalMove(_waveCurrentPosition[1], duration).SetEase(Ease.OutQuad);
        _waveBest.transform.DOLocalMove(_waveBestPosition[1], duration).SetEase(Ease.OutQuad);
    }    

    private IEnumerator ShowOff()
    {
        yield return new WaitForSeconds(3f);
        _canvas.DOFade(0f, duration);
    }

}
