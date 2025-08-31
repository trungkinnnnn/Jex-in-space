using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;


public class GameStateManager : MonoBehaviour
{
    // pausecontroller
    public static Action ActionOnScreen;
    public static Func<IEnumerator> ActionOffScreen;

    [Header("Screen")]
    [SerializeField] GameObject _screenInGame;
    [SerializeField] GameObject _screenPauseGame;
    [SerializeField] GameObject _sceenEmpty;

    private CanvasGroup _canvasGroupInGame;
    private CanvasGroup _canvasGroupPauseGame;
    private CanvasGroup _canvasGroupEmpty;
    private float _alphaMax = 1f;
    private float _alphaMin = 0f;


    public float timeOffScreen = 0.4f;

   

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
    }
   

    public void ActionDownButtonPauseON()
    {
        AudioBGMManager.Instance.PauseBGM();
        AudioSystem.Instance.PlayAudioClick();

        Debug.Log("Pause Game");
        PausePhysic2D.Instance.PauseGame();

        StartCoroutine(OffScreen(_canvasGroupInGame, _screenInGame));
        StartCoroutine(OnScreen(_canvasGroupEmpty, _sceenEmpty));
        StartCoroutine(OnScreen(_canvasGroupPauseGame, _screenPauseGame));
        ActionOnScreen?.Invoke();
    }

    public void ActionUpButtonPauseOFF()
    {
        AudioSystem.Instance.PlayAudioClick();

        StartCoroutine(HandleActionPauseOFF());
        AudioBGMManager.Instance.ResumeBGM();
    }

    public IEnumerator HandleActionPauseOFF()
    {
        if(ActionOffScreen() != null)
            yield return StartCoroutine(ActionOffScreen());
        yield return StartCoroutine(OffScreen(_canvasGroupPauseGame, _screenPauseGame));
        StartCoroutine(OffScreen(_canvasGroupEmpty, _sceenEmpty));

        PausePhysic2D.Instance.ResumeGame(); 
        StartCoroutine(OnScreen(_canvasGroupInGame, _screenInGame));
    }    

    private IEnumerator OffScreen(CanvasGroup canvas, GameObject screen)
    {
        if (canvas != null)
        {
            var t = canvas.DOFade(_alphaMin, timeOffScreen).SetUpdate(true);
            yield return t.WaitForCompletion();
        }
        screen.SetActive(false);
    }

    private IEnumerator OnScreen(CanvasGroup canvas, GameObject screen)
    {
        screen.SetActive(true);
        if (canvas != null)
        {
            var t = canvas.DOFade(_alphaMax, timeOffScreen).SetUpdate(true);
            yield return t.WaitForCompletion();
        }
    }

  


}
