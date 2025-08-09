using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D), typeof(CircleCollider2D))]
public class EffectLightExplosion : MonoBehaviour
{
    public static Action<Vector2, float, float> OnExploed;

    //CameraShake
    private float _range = 0f;
    private float _intensity = 0f;

    [Header("Info explosion light")]
    public float explosionRadius = 1f;
    public float forceEnter = 1f;
    public float forceStay = 0.3f;
    public float lifeTime = 1f;

    // Thời gian nở
    [Range(0.1f, 0.9f)]
    public float growRatio = 0.3f;

    private Light2D _light;
    private CircleCollider2D _circleCollider;
    private float _timer = 0f;
    private Color _alpha;

    // Using for light
    public void InitRadius(float radius)
    {
        explosionRadius = radius;
    }

    // Using for Bullet
    public void InitForce(float forceEnter, float forceStay)
    {
        this.forceEnter = forceEnter;
        this.forceStay = forceStay;
    }    

    // Using for CameraShake
    public void InitCameraShake(float range, float intensity)
    {
        _range = range;
        _intensity = intensity;
    }    

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _circleCollider = GetComponent<CircleCollider2D>();

        _light.pointLightOuterRadius = 0f;
        _circleCollider.radius = 0f;

        _alpha = _light.color;

        OnExploed?.Invoke(new Vector2(transform.position.x, transform.position.y), _range, _intensity);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        float t = Mathf.Clamp01(_timer/lifeTime);
        if(t < growRatio)
        {
            float growT = t / growRatio;
            float radius = Mathf.Lerp(0f, explosionRadius, growT);

            _light.pointLightOuterRadius = radius;  
            _circleCollider.radius = radius;
        }
        else
        {
            float shinkT = (t - growRatio) / (1f - growRatio);   
            float radius = Mathf.Lerp(explosionRadius, 0f, shinkT);

            _light.pointLightOuterRadius = radius;
            _circleCollider.radius = radius;

            SetAlpha(Mathf.Lerp(_alpha.a, 0f, shinkT));

        }

        if(_timer >= lifeTime)
        {
            Destroy(gameObject);
        }    
    }
    
    private void SetAlpha(float alpha)
    {
        Color color = _light.color; 
        color.a = alpha;
        _light.color = color;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if(rb != null)
        {
          
            Vector2 dir = rb.transform.position - transform.position;
            float distance = dir.magnitude; 

            if(distance > 0 && distance <= explosionRadius)
            {
                float fallof = 1f - (distance/explosionRadius);
                rb.AddForce(dir.normalized * forceEnter *  fallof, ForceMode2D.Impulse);
            }    
        }    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null)
        {
            Vector2 dir = rb.transform.position - transform.position;
            float distance = dir.magnitude;

            if (distance > 0 && distance <= explosionRadius)
            {
                float fallof = 1f - (distance / explosionRadius);
                rb.AddForce(dir.normalized * forceStay * fallof, ForceMode2D.Force);
            }
        }
    }



}
