using System;
using System.Collections.Generic;
using UnityEngine;

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
    

    private Rigidbody2D _rb;
    private int _hpMax;
    private int _currentHp;
    private float _timeImmortal;
    private float _lastTimeTakenDamage = -Mathf.Infinity;

    public float addForceImpact = 0.1f;
    public float addForceStay = 0.1f;
    public float impactSpeedMax = 0.2f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _hpMax = _data.health;
        _currentHp = _hpMax;
        _timeImmortal = _data.timeImmortal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_asteroidTag))
        {
            float impactSpeed = collision.relativeVelocity.magnitude;

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
        //Debug.Log("Hp" + _currentHp);


        OnCatCrack();

        //Event
        OnActionHp?.Invoke(_currentHp); 

        if (_currentHp <= 0)
        {
            Die?.Invoke();
            Debug.Log("Jex die");
        }
    }

    private void AddHealth(int heal)
    {
        _currentHp = Mathf.Min(_currentHp + heal, _hpMax);

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
        _catCracks[0].gameObject.SetActive(true);
        _catCracks[1].gameObject.SetActive(_currentHp <= 1);
    }
    }    
}
