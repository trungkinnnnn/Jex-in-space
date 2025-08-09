using System;
using UnityEngine;

public class JexHealth : MonoBehaviour
{
    public static Action Eat;
    public static Action Hurt;
    public static Action Die;

    [SerializeField] private JexData _data;
    [SerializeField] private string _asteroidTag = "Ast";
    [SerializeField] private string _healthItemTag = "Health";
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _healAmount = 1;
    [SerializeField] public float addForceImpact = 0.1f;

    private Rigidbody2D _rb;
    private int _hpMax;
    private int _currentHp;
    private float _timeImmortal;
    private float _lastTimeTakenDamage = -Mathf.Infinity;

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
                Hurt?.Invoke();
                AddForceFrom(collision.collider.transform.position);
            }
        }

        if (collision.collider.CompareTag(_healthItemTag))
        {
            AddHealth(_healAmount);
            Eat?.Invoke();

            var item = collision.collider.GetComponent<ItemHealth>();
            if (item != null) item.HandleDestroyHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        Debug.Log("Hp" + _currentHp);
        if (_currentHp <= 0)
        {
            Die?.Invoke();
            Debug.Log("Jex die");
        }
    }

    private void AddHealth(int heal)
    {
        _currentHp = Mathf.Min(_currentHp + heal, _hpMax);
    }

    private void AddForceFrom(Vector3 sourcePosition)
    {
        Vector3 direction = (transform.position - sourcePosition).normalized;
        _rb.AddForce(direction * addForceImpact, ForceMode2D.Impulse);
    }
}
