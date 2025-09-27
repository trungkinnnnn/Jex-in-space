using System.Collections;
using UnityEngine;

public class TrashGun : MonoBehaviour
{
    public float timeLife = 5f;
    public float addForceMin = 0.4f;
    public float addForceMax = 0.7f;

    public float addForceMinToque = 0.05f;
    public float addForceMaxToque = 0.1f;

    private float _addForce;
    private float _addForceToque;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
        }
        
        StartCoroutine(WaitToNextFrame());
        StartCoroutine(TrashEnd());
        SetAlpha(1f);
    }

    public void Init(Vector2 direction)
    {
        _direction = direction;
    }

    private void SetUp()
    {
        _addForce = Random.Range(addForceMin, addForceMax);
        _addForceToque = Random.Range(addForceMinToque, addForceMaxToque);

        AddForce();
    }

    private void AddForce()
    {
        _rb.AddForce(_direction * _addForce, ForceMode2D.Impulse);
        _rb.AddTorque(_addForceToque, ForceMode2D.Impulse);
    }

    private IEnumerator TrashEnd()
    {
        float time = 0;
        while(time < timeLife)
        {
            float alpha = Mathf.Lerp(1f, 0f, time/timeLife);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }
        PoolManager.Instance.Despawner(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _sprite.color;
        color.a = alpha;
        _sprite.color = color;
    }

    private IEnumerator WaitToNextFrame()
    {
        yield return null;
        SetUp();
    }    

}
