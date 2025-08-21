using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PowerLow : MonoBehaviour
{
    public float timeLifeMin = 5f;
    public float timeLifeMax = 7f;
    public float scaleMin = 0.5f;
    public float scaleMax = 1.0f;
    public float addForceMin = 0.5f;
    public float addForceMax = 1.0f;

    private float _timeLife;
    private float _scale;
    private float _addForce;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(DestroyOnLife());
    }

    public void Init()
    {
        _timeLife = Random.Range(timeLifeMin, timeLifeMax);
        _scale = Random.Range(scaleMin, scaleMax);
        _addForce = Random.Range(addForceMin, addForceMax);

        transform.localScale = new Vector3(_scale, _scale, 1f);
    }

    public void AddForce(Vector2 direction, float angle = 20f)
    {
        float angleRandom = Random.Range(-angle, angle);
        Vector2 rotadir = Quaternion.Euler(0, 0, angleRandom) * direction.normalized;
        _rb.AddForce(rotadir * _addForce, ForceMode2D.Impulse);
    }    

  
    private IEnumerator DestroyOnLife()
    {
        yield return StartCoroutine(LifeCircle());
        Destroy(gameObject);
    }    

    private IEnumerator LifeCircle()
    {
        float timer = 0;
        while (timer < _timeLife)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer/_timeLife);
            SetAlpha(alpha);
            yield return null;
        }    
    }    

    private void SetAlpha(float alpha)
    {
        Color color = _sprite.color;
        color.a = alpha;
        _sprite.color = color;
    }    

}
