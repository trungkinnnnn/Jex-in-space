using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] GameObject _screenInGame;
    [SerializeField] GameObject _screenPauseGame;
    [SerializeField] GameObject _sceenEmpty;

    [Header("ShowHand")]
    [SerializeField] GameObject _handObj;
    [SerializeField] float duration = 0.4f;
    private RectTransform _handTransform;
    private Image _handImage;
    private Vector2 _hiddenPos = new Vector2(1140f, 0f);
    private Vector2 _showPos = new Vector2(245f, 0f);
    private bool _isShown = false;

    private Material _material;

    private CanvasGroup _canvasGroupInGame;
    private CanvasGroup _canvasGroupPauseGame;
    private CanvasGroup _canvasGroupEmpty;
    private float _alphaMax = 1f;
    private float _alphaMin = 0f;

    // Change material
    private string _para_NAME_IntensityX = "_IntensityX";
    private string _para_NAME_IntensityY = "_IntensityY";
    private float _paraIntensityStart = 0f;
    private float _paraIntensityTargetX = 0.005f;
    private float _paraIntensityTargetY = 0.04f;
    private float _matDurationON = 0.1f;
    private float _matDurationOFF = 0.2f;


    public float timeOffScreen = 0.3f;

    private void Awake()
    {
        _screenInGame.SetActive(true);
        _screenPauseGame.SetActive(false);
        _sceenEmpty.SetActive(false);
    }

    private void Start()
    {
        _canvasGroupInGame = _screenInGame.GetComponent<CanvasGroup>();
        _canvasGroupPauseGame = _screenPauseGame.GetComponent<CanvasGroup>();
        _canvasGroupEmpty = _sceenEmpty.GetComponent<CanvasGroup>();

        SetupHandObj();
    }

    private  void SetupHandObj()
    {
        _handTransform = _handObj.GetComponent<RectTransform>();    
        _handImage = _handTransform.GetComponent<Image>();

        _material = Instantiate(_handImage.material);
        _handImage.material = _material;

        _handTransform.anchoredPosition = _hiddenPos;
        _material.SetFloat(_para_NAME_IntensityX, _paraIntensityStart);
        _material.SetFloat(_para_NAME_IntensityY, _paraIntensityStart);
    }    

    public void ActionDownButtonPauseON()
    {
        Debug.Log("Pause Game");
        Time.timeScale = 0;
        StartCoroutine(OffScreen(_canvasGroupInGame, _screenInGame));
        StartCoroutine(OnScreen(_canvasGroupEmpty, _sceenEmpty));
        StartCoroutine(OnScreen(_canvasGroupPauseGame, _screenPauseGame));
        StartCoroutine(HandON());
    }

    public void ActionUpButtonPauseOFF()
    {
        StartCoroutine(HandleActionPauseOFF()); 
    }

    public IEnumerator HandleActionPauseOFF()
    {
        yield return StartCoroutine(HandOFF());
        StartCoroutine(OffScreen(_canvasGroupPauseGame, _screenPauseGame));
        StartCoroutine(OffScreen(_canvasGroupEmpty, _sceenEmpty));
        Time.timeScale = 1;
        StartCoroutine(OnScreen(_canvasGroupInGame, _screenInGame));
    }    

    private IEnumerator OffScreen(CanvasGroup canvas, GameObject screen)
    {
        float timer = 0;
        while (timer < timeOffScreen)
        {
            timer += Time.unscaledDeltaTime;
            canvas.alpha = Mathf.Lerp(_alphaMax, _alphaMin, timer/timeOffScreen);
            yield return null;  
        }
        screen.SetActive(false);
    }

    private IEnumerator OnScreen(CanvasGroup canvas, GameObject screen)
    {
        screen.SetActive(true);
        float timer = 0;
        while (timer < timeOffScreen)
        {
            timer += Time.unscaledDeltaTime;
            canvas.alpha = Mathf.Lerp(_alphaMin, _alphaMax, timer / timeOffScreen);
            yield return null;
        }
    }

    private IEnumerator HandON()
    {
        yield return StartCoroutine(MoveHand());
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityX), _paraIntensityTargetX, _para_NAME_IntensityX, _matDurationON));
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityY), _paraIntensityTargetY, _para_NAME_IntensityY, _matDurationON));

    }

    private IEnumerator HandOFF()
    {
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityX), _paraIntensityStart, _para_NAME_IntensityX, _matDurationOFF))      ;
        yield return StartCoroutine(ChangMatHand(_material.GetFloat(_para_NAME_IntensityY), _paraIntensityStart, _para_NAME_IntensityY, _matDurationOFF));
        yield return StartCoroutine(MoveHand());
    }

    private IEnumerator MoveHand()
    {
        _isShown = !_isShown;
        _handTransform.DOKill();
        Tween t =  _handTransform
            .DOAnchorPos(_isShown ? _showPos : _hiddenPos, duration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
        yield return t.WaitForCompletion();
    }

    private IEnumerator ChangMatHand(float start, float target, string name, float duration)
    {
        float timer = 0f;
        while(timer< duration)
        {
            timer += Time.unscaledDeltaTime;
            float value = Mathf.Lerp(start, target, timer / duration);
            _material.SetFloat(name, value);
            yield return null;
        }
    }


}
