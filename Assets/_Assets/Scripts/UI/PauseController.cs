using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] GameObject testButton;

    [Header("ShowHand")]
    [SerializeField] GameObject _handObj;
    [SerializeField] float _durationMoveHand = 0.4f;
    private RectTransform _handTransform;
    private Image _handImage;
    private Vector2 _hiddenPos = new Vector2(1140f, 145f);
    private Vector2 _showPos = new Vector2(182f, 145f);
    private bool _isShown = false;

    private Material _material;

    private string _para_NAME_IntensityX = "_IntensityX";
    private string _para_NAME_IntensityY = "_IntensityY";
    private float _paraIntensityStart = 0f;
    private float _paraIntensityTargetX = 0.008f;
    private float _paraIntensityTargetY = 0.008f;
    private float _matDurationON = 0.1f;
    private float _matDurationOFF = 0.2f;

    private void Start()
    {
        SetupHandObj();
    }

    private void OnEnable()
    {
        GameStateManager.ActionOnScreen += OnScreen;
        GameStateManager.ActionOffScreen = OffScreen;
    }

    private void OnDisable()
    {
        GameStateManager.ActionOnScreen -= OnScreen;
        GameStateManager.ActionOffScreen = null;
    }

    private void SetupHandObj()
    {
        _handTransform = _handObj.GetComponent<RectTransform>();
        _handImage = _handTransform.GetComponent<Image>();

        _material = Instantiate(_handImage.material);
        _handImage.material = _material;

        _handTransform.anchoredPosition = _hiddenPos;
        _material.SetFloat(_para_NAME_IntensityX, _paraIntensityStart);
        _material.SetFloat(_para_NAME_IntensityY, _paraIntensityStart);
    }

    private void OnScreen()
    {
        StartCoroutine(HandON());
    }

    private IEnumerator OffScreen()
    {
        yield return StartCoroutine(HandOFF());
    }

    private IEnumerator HandON()
    {
        testButton.SetActive(true);
        yield return StartCoroutine(MoveHand());
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityX), _paraIntensityTargetX, _para_NAME_IntensityX, _matDurationON));
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityY), _paraIntensityTargetY, _para_NAME_IntensityY, _matDurationON));

    }

    private IEnumerator HandOFF()
    {
        testButton.SetActive(false);
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityX), _paraIntensityStart, _para_NAME_IntensityX, _matDurationOFF));
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityY), _paraIntensityStart, _para_NAME_IntensityY, _matDurationOFF));
        yield return StartCoroutine(MoveHand());
    }

    private IEnumerator MoveHand()
    {
        _isShown = !_isShown;
        _handTransform.DOKill();
        Tween t = _handTransform
            .DOAnchorPos(_isShown ? _showPos : _hiddenPos, _durationMoveHand)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
        yield return t.WaitForCompletion();
    }

    private IEnumerator ChangMatHand(float start, float target, string name, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float value = Mathf.Lerp(start, target, timer / duration);
            _material.SetFloat(name, value);
            yield return null;
        }
    }
}
