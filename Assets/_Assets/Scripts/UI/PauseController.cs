using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] GameObject _sceen;
    private RectTransform _rectTransformScreen;
    private Vector2 _hiddenPos = new Vector2(1010f, 0f);
    private Vector2 _showPos = new Vector2(0f, 0f);
    private bool _isShown = false;
    private float _durationSceen = 0.4f;

    [Header("Button")]
    [SerializeField] GameObject _buttonResume;

    [Header("ShowHand")]
    [SerializeField] GameObject _handObj;
    private Image _handImage;
    private Material _materialHand;
  
    private float _matDurationON = 0.1f;
    private float _matDurationOFF = 0.2f;

    [Header("ShowTablet")]
    [SerializeField] GameObject _borderObj;
    private Image _imageBorder;
    private Material _materialImageBorder;

    [Header("ShowContent")]
    [SerializeField] GameObject _contentTablet;
    public int _blinkCount = 2;
    public float _blinkInterval = 0.1f;
    
    // IntensityX
    private string _para_NAME_IntensityX = "_IntensityX";
    private string _para_NAME_IntensityY = "_IntensityY";
    private float _paraIntensityStart = 0f;
    private float _paraIntensityTargetX = 0.005f;
    private float _paraIntensityTargetY = 0.005f;

    // Reveal
    private float _para_RevealStart = 1f;
    private float _para_RevealEnd = 0f;
    private static string _para_NAME_REVEAL = "_Reveal";
    private float _revelDuration = 0.1f;
    private void Start()
    {
        _contentTablet.SetActive(false);
        SetupHandObj();
        SetupReveal();
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
        _rectTransformScreen = _sceen.GetComponent<RectTransform>();
        _rectTransformScreen.anchoredPosition = _hiddenPos;
        // Hand
        _handImage = _handObj.GetComponent<Image>();

        _materialHand = Instantiate(_handImage.material);
        _handImage.material = _materialHand;

        
        _materialHand.SetFloat(_para_NAME_IntensityX, _paraIntensityStart);
        _materialHand.SetFloat(_para_NAME_IntensityY, _paraIntensityStart);
    }

    private void SetupReveal()
    {
        _imageBorder = _borderObj.GetComponent<Image>();
        _materialImageBorder = Instantiate(_imageBorder.material);
        _imageBorder.material = _materialImageBorder;
        _materialImageBorder.SetFloat(_para_NAME_REVEAL, _para_RevealStart);
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
        _buttonResume.SetActive(true);
        
        // ChangePosition
        yield return StartCoroutine(MoveHand());

        // effect 3D
        StartCoroutine(ChangeMat( _materialHand, _materialHand.GetFloat(_para_NAME_IntensityX), _paraIntensityTargetX, _para_NAME_IntensityX, _matDurationON));
        StartCoroutine(ChangeMat( _materialHand, _materialHand.GetFloat(_para_NAME_IntensityY), _paraIntensityTargetY, _para_NAME_IntensityY, _matDurationON));

        // effect Open tablet 
        yield return StartCoroutine(ChangeMat( _materialImageBorder, _materialImageBorder.GetFloat(_para_NAME_REVEAL), _para_RevealEnd, _para_NAME_REVEAL, _revelDuration));
        StartCoroutine(ChangeMat(_materialImageBorder, _materialImageBorder.GetFloat(_para_NAME_IntensityX), _paraIntensityTargetX, _para_NAME_IntensityX, _matDurationON));
        StartCoroutine(ChangeMat(_materialImageBorder, _materialImageBorder.GetFloat(_para_NAME_IntensityY), _paraIntensityTargetY, _para_NAME_IntensityY, _matDurationON));

        // effect blink tablet
        StartCoroutine(BlinkBeforeOn(_contentTablet, _blinkCount, _blinkInterval));
    }

    private IEnumerator HandOFF()
    {
        _buttonResume.SetActive(false);
        _contentTablet.SetActive(false);
        yield return StartCoroutine(ChangeMat(_materialImageBorder, _materialImageBorder.GetFloat(_para_NAME_REVEAL), _para_RevealStart, _para_NAME_REVEAL, _revelDuration));
        yield return StartCoroutine(ChangeMat(_materialHand, _materialHand.GetFloat(_para_NAME_IntensityX), _paraIntensityStart, _para_NAME_IntensityX, _matDurationOFF));
        yield return StartCoroutine(ChangeMat(_materialHand, _materialHand.GetFloat(_para_NAME_IntensityY), _paraIntensityStart, _para_NAME_IntensityY, _matDurationOFF));
        yield return StartCoroutine(MoveHand());
        MatReset(_materialImageBorder, _para_NAME_IntensityX, _paraIntensityStart);
        MatReset(_materialImageBorder, _para_NAME_IntensityY, _paraIntensityStart);
    }

    private IEnumerator MoveHand()
    {
        _isShown = !_isShown;
        _rectTransformScreen.DOKill();
        Tween t = _rectTransformScreen
            .DOAnchorPos(_isShown ? _showPos : _hiddenPos, _durationSceen)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
        yield return t.WaitForCompletion();
    }

    private IEnumerator ChangeMat( Material material, float start, float target, string name, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float value = Mathf.Lerp(start, target, timer / duration);
            material.SetFloat(name, value);
            yield return null;
        }
    }

    private IEnumerator BlinkBeforeOn(GameObject obj, int blinkCount, float blinkTime)
    {
        for(int i = 0; i < blinkCount; i++)
        {
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSecondsRealtime(blinkTime);
        }    

        obj.SetActive(true);
    }    

    private void MatReset( Material material, string name, float value)
    {
        material.SetFloat(name, value);
    }
}
