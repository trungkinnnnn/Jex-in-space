using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_InGame : MonoBehaviour
{
    [Header("CanvasGroup")] 
    [SerializeField] CanvasGroup _canvasTutorial_1;
    [SerializeField] CanvasGroup _canvasTutorial_2;
    [SerializeField] CanvasGroup _canvasChecking;

    [Header("Button")]
    [SerializeField] Button _confirmButton;

    [Header("Rotation")]
    [SerializeField] Transform _playerTransform;

    [Header("Audio")]
    [SerializeField] AudioClip _clipClick;

  
    private float _rotationZ = 44f;

    private bool _showTutorial_1 = false;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (_showTutorial_1)
            ShowTutorial_1();

    }

    private void SetUp()
    {
        _canvasTutorial_1.gameObject.SetActive(false);
        _canvasTutorial_2.gameObject.SetActive(false);
        _canvasChecking.gameObject.SetActive(false);

        _canvasTutorial_1.alpha = 0f;
        _canvasTutorial_2.alpha = 0f;
        _canvasChecking.alpha = 0f;

        _confirmButton.onClick.AddListener(() => ActionNextTutorial());
    }    

    private void OnEnable()
    {
        Tutorial.OnTutorialInGame += HandleOpenTutorial;
    }

    private void OnDisable()
    {
        Tutorial.OnTutorialInGame -= HandleOpenTutorial;
    }

    private void HandleOpenTutorial()
    {
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("Checking");
        _showTutorial_1 = true;
    }    

    private void ShowTutorial_1()
    {
        float zAngle = _playerTransform.localEulerAngles.z;
        if(zAngle > _rotationZ - 5f && zAngle < _rotationZ + 5f)
        {
            InputManager.isInputLocked = true;
            PausePhysic2D.Instance.PauseGame();
            _canvasTutorial_1.gameObject.SetActive(true);
            _canvasTutorial_1.DOFade(1f, 2f).SetEase(Ease.OutQuad).SetUpdate(true);
            _showTutorial_1 = false;
        }
    }

    private void ActionNextTutorial()
    {
        AudioSFX.Instance.PlayAudioOneShortOneClip(_clipClick, 1.5f);
        StartCoroutine(SetUpTutorial());
    }    

    private IEnumerator SetUpTutorial()
    {
        InputManager.isInputLocked = false;
        PausePhysic2D.Instance.ResumeGame();
        _canvasTutorial_1.DOFade(0f, 1f).SetEase(Ease.Linear).SetUpdate(true);
        yield return new WaitForSeconds(1.5f);
        _canvasTutorial_1.gameObject.SetActive(false);
        _canvasTutorial_2.gameObject.SetActive(true);
        _canvasTutorial_2.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(5f);
        _canvasTutorial_2.DOFade(0f, 2f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(3f);
        _canvasTutorial_1.gameObject.SetActive(false);
        _canvasTutorial_2.gameObject.SetActive(false);
        _canvasChecking.gameObject.SetActive(true);
        _canvasChecking.DOFade(1f, 3f).SetEase(Ease.OutQuad);
    }



}
