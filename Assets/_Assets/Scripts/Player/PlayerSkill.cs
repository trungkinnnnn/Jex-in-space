using System;
using System.Collections;
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

    [Header("SkillShoot")]
    public int magSizeBulletSkill = 30;
    public float fireRateSkill = 0.2f;
    public float forceToque = 0.03f;
    public float force = 0.01f;

    private float _magSkill;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _magSkill = magSizeBulletSkill;
        _playerMovement = GetComponent<PlayerMovement>();

        if (_buttonSkillShoot != null) _buttonSkillShoot.onClick.AddListener(() => ActionSkillShootOn());
        
    }

    private void Update()
    {
        fire = Input.GetMouseButtonDown(0);
    }


    public void ActionSkillShootOn()
    {
        if (skillShootOn) return; // tránh bật nhiều lần 
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