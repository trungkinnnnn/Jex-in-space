using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public static class InputManager
{
    public static bool isInputLocked = false;
}

public static class FireRate
{
    public static bool canShoot = true;
}

public class PlayerSkill : MonoBehaviour
{
    public static bool fire = false;
    public static bool skillShootOn = false;

    public static Action OnSkillShoot;

    [Header("Button")]
    [SerializeField] Button _buttonSkillShoot;
    [SerializeField] Button _buttonShockWave;

    [Header("UI")]
    [SerializeField] HUDController _hudController;

    [Header("SkillShoot")]
    [SerializeField] SkillController _skillController;
    public int magSizeBulletSkill = 30;
    public float fireRateSkill = 0.2f;
    public float forceToque = 0.03f;
    public float force = 0.01f;

    [Header("ShockWave")]
    [SerializeField] List<ShockWave> _shockWave;
    private bool _isShock = false;
    public float shockTime = 0.5f;

    [Header("Audio")]
    [SerializeField] List<AudioClip> _audioClips;
    public float volume = 1.5f;

    private List<int> _counts;
    private PlayerMovement _playerMovement;
    private float _magSkill;
    private void Start()
    {
        _magSkill = magSizeBulletSkill;
        _playerMovement = GetComponent<PlayerMovement>();
        _counts = _skillController.GetListNumberSkill();

        if (_buttonSkillShoot != null) _buttonSkillShoot.onClick.AddListener(() => ActionSkillShootOn());
        if (_buttonShockWave != null) _buttonShockWave.onClick.AddListener(() => ActionOnShockWave());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }    

            fire = true;
        }    
    }

    private void ActionOnShockWave()
    {
        if (_counts[0] < 1 || _isShock) return;
        _isShock = true;

        _skillController.SetNumberIndexList(0, _skillController.GetListNumberSkill()[0] - 1);
        _hudController.SetTextNumberSkill(_skillController.GetListNumberSkill());

        StartCoroutine(OnShockWaveCoroutine());
    }    

    private IEnumerator OnShockWaveCoroutine()
    {
        for (int i = 0; i < _shockWave.Count; i++)
        {
            _shockWave[i].OnShockWave();
            AudioSFX.Instance.PlayAudioOneShortAndVolumeDownBackGround(_audioClips, volume);
            yield return new WaitForSeconds(shockTime);
        }
        _isShock = false;
    }


    private void ActionSkillShootOn()
    {
        if (_counts[1] < 1 || skillShootOn) return; // tránh bật nhiều lần 
        skillShootOn = true;

        _skillController.SetNumberIndexList(1, _skillController.GetListNumberSkill()[1] - 1);
        _hudController.SetTextNumberSkill(_skillController.GetListNumberSkill());

        StartCoroutine(FireSkillCoroutine());
    }

    private IEnumerator FireSkillCoroutine()
    {
        InputManager.isInputLocked = true;
        while (_magSkill > 0)
        {
            _magSkill--;
            OnSkillShoot?.Invoke();
            _playerMovement.AddForceMin(force, forceToque);
            yield return new WaitForSeconds(fireRateSkill); 
        }
        skillShootOn = false;
        InputManager.isInputLocked = false;
        _magSkill = magSizeBulletSkill;
    }


}