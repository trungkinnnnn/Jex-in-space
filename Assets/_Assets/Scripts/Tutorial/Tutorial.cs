using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Playables;

public class Tutorial : MonoBehaviour
{
    [Header("OBJ")]
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _wave;
    [SerializeField] GameObject _ship;

    [Header("Moving")]
    [SerializeField] GameObject _camera;
    [SerializeField] List<Vector2> _cameraMove = new List<Vector2> { new Vector2(-2f, 0f), new Vector2(9.5f, 0f) };
    [SerializeField] GameObject _stars;
    [SerializeField] List<Vector2> _starMove = new List<Vector2> { new Vector2(-2f, 0f), new Vector2(9.5f, 0f) };

    [Header("Position Tutorial Gameplay")]
    [SerializeField] Vector2 _cameraPosition = new Vector2(0f, 0f);
    [SerializeField] List<RectTransform> _imageTransforms;
    [SerializeField]
    List<Vector2> _positionStartImage = new List<Vector2> 
    {
        new Vector2(-956f, 661f), new Vector2(964f, 719f),
        new Vector2(-1180f, 277f), new Vector2(905f, 72f),
        new Vector2(-953f, -97f), new Vector2(849f, -229f),
        new Vector2(-1168f, -496f)
    };
    [SerializeField]
    List<Vector2> _positionEndImage = new List<Vector2>
    {   new Vector2(-241f, 661f), new Vector2(278f, 719f),
        new Vector2(0f, 277f), new Vector2(250f, 72f),
        new Vector2(-205f, -97f), new Vector2(200f, -229f),
        new Vector2(0f, -496f)
    };
    [SerializeField] GameObject _panelBipBip;
    [SerializeField] Button _nextButton2;
    [SerializeField] Button _nextButton3;
    

    [Header("Audio")]
    [SerializeField] List<AudioClip> _clips;
    [SerializeField] AudioClip _clipFire;
    public float volume = 1f;

    [Header("Canvas")]
    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasGroup _textStory;
    [SerializeField] Button _nextButton1;

    [Header("Animation")]
    [SerializeField] Animator _cameraAnimator;
    [SerializeField] PlayableDirector _timeline;

    private Image _imageButton;
    private int _index = 1;
    private bool _playWaring = false;   

    public float durationMove = 30f;
    public float durationOn = 5f;


    private void Awake()
    {
        _player.SetActive(false);
        _wave.SetActive(false);
        _ship.SetActive(false);
    }

    private void Start()
    {
        SetUpStart();
        StartCoroutine(StartStory());
    }

    private void SetUpStart()
    {
        _canvas.gameObject.SetActive(true);
        _textStory.alpha = 0f;
        _imageButton = _nextButton1.GetComponent<Image>();
        SetAlpha(0f);
        _camera.transform.position = _cameraMove[0];
        _stars.transform.position = _starMove[0];

        _nextButton2.gameObject.SetActive(false);
        _nextButton3.gameObject.SetActive(false);
        _panelBipBip.gameObject.SetActive(false);

        _nextButton2.onClick.AddListener(() => ActionNextStoryImage());
        _nextButton3.onClick.AddListener(() => ActionNextIngameTutorial());

        _cameraAnimator.enabled = false;
    }    

    private IEnumerator StartStory()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(LoadingScreen.Instance.HideAlpha(2f));
        _camera.transform.DOMove(_cameraMove[1], durationMove).SetEase(Ease.Linear);
        _stars.transform.DOMove(_starMove[1], durationMove).SetEase(Ease.Linear);

        yield return new WaitForSeconds(3f);
        _textStory.DOFade(1f, durationOn);

        yield return new WaitForSeconds(4f);
        _imageButton.DOFade(1f, durationOn);
        _nextButton1.onClick.AddListener(() => ActionNextStory());
    }

    private void SetAlpha(float alpha)
    {
        Color color = _imageButton.color;
        color.a = alpha;    
        _imageButton.color = color;
    }   

    private void ActionNextStory()
    {
        AudioBGMManager.Instance.SetActive(false);
        StartCoroutine(NextStory());
    }

    private IEnumerator NextStory()
    {
        _imageButton.DOFade(0f, 2f);
        StartCoroutine(LoadingScreen.Instance.ShowAlpha(2f));
        yield return new WaitForSeconds(3f);
        SetUp();
        StartCoroutine(LoadingScreen.Instance.HideAlpha(1f));
        yield return new WaitForSeconds(1f);
        _imageTransforms[0].DOAnchorPos(_positionEndImage[0], 1f).SetEase(Ease.InQuad);
        AudioSFX.Instance.PlayAudioOneShortOneClip(_clips[0], volume);
    }

    private void SetUp()
    {
        DOTween.KillAll();

        _textStory.alpha = 0f;
        _camera.transform.position = _cameraPosition;
        _stars.transform.position = _starMove[1];

        _nextButton2.gameObject.SetActive(true);

        for(int i = 0; i < _imageTransforms.Count; i++)
        {
            _imageTransforms[i].anchoredPosition = _positionStartImage[i];
        }    
    }

    private void ActionNextStoryImage()
    {
        if(_index < _imageTransforms.Count)
        {
            _imageTransforms[_index].DOAnchorPos(_positionEndImage[_index], 1f).SetEase(Ease.InQuad);
            AudioSFX.Instance.PlayAudioOneShortOneClip(_clips[_index], volume);
            if(_index == 3) AudioSFX.Instance.PlayAudioOneShortOneClip(_clipFire, volume);
            _index++;
        }else
        {
            if(!_playWaring)
            {
                _panelBipBip.gameObject.SetActive(true);
                _nextButton3.gameObject.SetActive(true);
                AudioSFX.Instance.PlayAudioOneShortOneClip(_clips[_index], volume);
                _playWaring = true;
            }    
           
        }
    }   
    
    private void ActionNextIngameTutorial()
    {
        StartCoroutine(SetUpIngameTutorial());
    }

    private IEnumerator SetUpIngameTutorial()
    {
        yield return StartCoroutine(LoadingScreen.Instance.ShowCircle());
        _canvas.gameObject.SetActive(false);
        _player.gameObject.SetActive(true);
        _wave.gameObject.SetActive(true);
        _ship.gameObject.SetActive(true);
        yield return StartCoroutine(LoadingScreen.Instance.HideCircle());
        yield return new WaitForSeconds(3f);
        _cameraAnimator.enabled = true;
        _timeline.Play();
        
    }
}
