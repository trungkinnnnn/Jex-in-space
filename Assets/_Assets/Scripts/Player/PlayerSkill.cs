using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("UI")]
    [SerializeField] HUDController _hudController;

    [Header("SkillShoot")]
    [SerializeField] SkillController _skillController;
    public int magSizeBulletSkill = 30;
    public float fireRateSkill = 0.2f;
    public float forceToque = 0.03f;
    public float force = 0.01f;

    private List<int> _counts;
    private PlayerMovement _playerMovement;
    private float _magSkill;
    private void Start()
    {
        _magSkill = magSizeBulletSkill;
        _playerMovement = GetComponent<PlayerMovement>();
        _counts = _skillController.GetListNumberSkill();

        if (_buttonSkillShoot != null) _buttonSkillShoot.onClick.AddListener(() => ActionSkillShootOn());
        
    }

    private void Update()
    {
        fire = Input.GetMouseButtonDown(0);
    }


    public void ActionSkillShootOn()
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