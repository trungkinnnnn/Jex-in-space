using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class CheckingAst : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textDestroysCount;
    [SerializeField] TextMeshProUGUI _textDestroysMax;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup _canvasChecking;
    [SerializeField] CanvasGroup _canvasDoneTutorial;

    [Header("Button")]
    [SerializeField] Button _playReal;

    [Header("Audio")]
    [SerializeField] AudioClip _clipClick;

    private static string _para_Name_Scene = "InGameScreen";
    private int _countDestroy = 0;
    private int _countDestroyMax = 15;

    private void Start()
    {
        _textDestroysMax.text = "/" + _countDestroyMax.ToString();
        _canvasChecking.alpha = 0f;
        _canvasDoneTutorial.alpha = 0f;
        _playReal.onClick.AddListener(() => ActionPlayReal());  
    }

    private void OnEnable()
    {
        Ast.AddScoreOnDie += HandleAddCount;
    }

    private void OnDisable()
    {
        Ast.AddScoreOnDie -= HandleAddCount;
    }

    private void HandleAddCount(int count)
    {
        _countDestroy += count;
        UpdateTextCount();
        Checking();
    }    

    private void UpdateTextCount() => _textDestroysCount.text = _countDestroy.ToString();

    private void Checking()
    {
        if(_countDestroy >= _countDestroyMax)
            StartCoroutine(DoneTutorial());
    }

    private IEnumerator DoneTutorial()
    {
        InputManager.isInputLocked = true;
        _canvasChecking.DOFade(0f, 3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);
        _canvasDoneTutorial.DOFade(1f, 3f).SetEase(Ease.Linear);
        PlayerPrefs.SetInt(DataPlayerPrefs.fistPlay, 1);
        PlayerPrefs.Save();
    }

    private void ActionPlayReal()
    {
        AudioSFX.Instance.PlayAudioOneShortOneClip(_clipClick, 1.5f);
        InputManager.isInputLocked = false;
        AudioBGMManager.Instance.SetActive(true);
        LoadingScene.Instance.LoadingScence(_para_Name_Scene);
    }    

}
