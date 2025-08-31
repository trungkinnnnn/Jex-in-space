using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Hud controller
    public static Action<int> OnActionHp;

    // AnimationLister, DieScreenUI
    public static Action Eat;
    public static Action Hurt;
    public static Action Die;

    [SerializeField] JexData _data;
    [SerializeField] List<GameObject> _catCracks;
    [SerializeField] string _asteroidTag = "Ast";
    [SerializeField] string _healthItemTag = "Health";
    [SerializeField] int _damage = 1;
    [SerializeField] int _healAmount = 1;

    [Header("Animation Break Bowl")]
    [SerializeField] Animator _animatorBreak;
    private static int _para_NAME_BREAK = Animator.StringToHash("isBreak");

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup _canvasGroupHit;
    [SerializeField] Button _keepPlayButton;
    
    private Rigidbody2D _rb;
    private PlayerAudio _audio;
    private int _hpMax;
    private int _currentHp;
    private float _timeImmortal;
    private float _lastTimeTakenDamage = -Mathf.Infinity;

    private int _firstPlay = 0;

    [Header("Setting")]
    public float addForceImpact = 0.1f;
    public float addForceStay = 0.1f;
    public float impactSpeedMax = 0.25f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<PlayerAudio>();
    }

    private void Start()
    {
        _hpMax = _data.health;
        _currentHp = _hpMax;
        _timeImmortal = _data.timeImmortal;
        CheckFirstPlay();
    }

    private void CheckFirstPlay()
    {
        _firstPlay = PlayerPrefs.GetInt(DataPlayerPrefs.fistPlay, 0);
        if (_firstPlay == 0)
            _currentHp = 10000;
        if (_firstPlay == 1)
        {
            _canvasGroupHit.alpha = 0f;
            _canvasGroupHit.gameObject.SetActive(false);
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_asteroidTag))
        {
            float impactSpeed = collision.relativeVelocity.magnitude;

            var impact = collision.collider.GetComponent<Ast>();
            float perSpeed = Mathf.Max(0.01f, impactSpeed / impactSpeedMax);
            impact?.PlayAudioImpact(perSpeed);

            if (impactSpeed >= impactSpeedMax && Time.time - _lastTimeTakenDamage >= _timeImmortal)
            {
                _lastTimeTakenDamage = Time.time;
                TakeDamage(_damage);

                //Event
                Hurt?.Invoke();
                AddForceFrom(collision.collider.transform.position, addForceImpact);
            }
        }

        if (collision.collider.CompareTag(_healthItemTag))
        {
            AddHealth(_healAmount);
            //Event
            Eat?.Invoke();

            var item = collision.collider.GetComponent<ItemHealth>();
            if (item != null) item.HandleDestroyHealth();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_asteroidTag))
        {
            AddForceFrom(collision.collider.transform.position, addForceStay);
        }    
    }

    public void TakeDamage(int damage)
    {
        _currentHp = Math.Max(0, _currentHp - damage);
        _audio.PlayClipHurt();
        OnCatCrack();
        CheckFirstHit();


        //Event
        OnActionHp?.Invoke(_currentHp); 

        if (_currentHp <= 0)
        {
            IsDie();
        }
    }

    private void CheckFirstHit()
    {
        if (_firstPlay == 1)
        {
            StartCoroutine(ShowWarningFirstHit());
        }
    }

    private IEnumerator ShowWarningFirstHit()
    {
        yield return new WaitForSeconds(1f);
        InputManager.isInputLocked = true;
        PausePhysic2D.Instance.PauseGame();
        _canvasGroupHit.gameObject.SetActive(true);
        _canvasGroupHit.DOFade(1f, 2f).SetEase(Ease.OutQuad).SetUpdate(true);
        _firstPlay = 2;
        PlayerPrefs.SetInt(DataPlayerPrefs.fistPlay, _firstPlay);
        PlayerPrefs.Save();

        _keepPlayButton.onClick.AddListener(()  => ActionKeepPlay());   
    }    

    private void ActionKeepPlay()
    {
        StartCoroutine(HideWarningFirstHit());
    }

    private IEnumerator HideWarningFirstHit()
    {
        _canvasGroupHit.DOFade(0f, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.5f);
        InputManager.isInputLocked = false;
        PausePhysic2D.Instance.ResumeGame();
        _canvasGroupHit.gameObject.SetActive(false);
    }

    private void IsDie()
    {
        _rb.angularDrag = 3f;
        _animatorBreak.SetTrigger(_para_NAME_BREAK);
        _audio.PlayClipDie();
        Die?.Invoke();
        Debug.Log("Jex die");
    }    

    private void AddHealth(int heal)
    {
        _currentHp = Mathf.Min(_currentHp + heal, _hpMax);

        _audio.PlayClipEat();
        //Event
        OnActionHp?.Invoke(_currentHp);
        OnCatCrack();
    }



    private void AddForceFrom(Vector3 sourcePosition, float force)
    {
        Vector3 direction = (transform.position - sourcePosition).normalized;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCatCrack()
    {

        if (_catCracks.Count < 2) return;

    if (_currentHp > 2)
    {
        _catCracks.ForEach(c => c.gameObject.SetActive(false));
    }
    else
    {
        _audio.PlayClipGlassBreak();
        _catCracks[0].gameObject.SetActive(true);
        _catCracks[1].gameObject.SetActive(_currentHp <= 1);
    }
    }    
}
