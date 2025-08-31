using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadingAmor : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Image _imageFill;
    [SerializeField] List<AudioClip> _clipReloads;

    private float _volume = 1f;
    private float _timeReloading;

    private void Start()
    {
        _canvasGroup.alpha = 0f;
        _imageFill.fillAmount = 0f;
    }

    private void OnEnable()
    {
        GunController.OnActionReloading += HandleShowReloading;
    }

    private void OnDisable()
    {
        GunController.OnActionReloading -= HandleShowReloading;
    }

    private void HandleShowReloading(float timeReload)
    {
        _timeReloading = timeReload;
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_clipReloads, _volume + 1.5f);
        StartCoroutine(Reload());
    }    

    private IEnumerator Reload()
    {
        _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InQuad);
        StartCoroutine(WhileLerp());
        yield return new WaitForSeconds(_timeReloading);
        _canvasGroup.DOFade(0f, 0.3f).SetEase(Ease.InQuad);
    }    

    private IEnumerator WhileLerp()
    {
        float time = 0f;
        while(time < _timeReloading)
        {
            time += Time.deltaTime;
            _imageFill.fillAmount = Mathf.Lerp(0f, 1f, time / _timeReloading);
            yield return null;
        }    
    }    

 }
