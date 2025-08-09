using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float timeDuration = 0.4f;
    [SerializeField] private float damping = 1.5f;
    [SerializeField] private float frequency = 20f;
    [SerializeField] Transform playerTranform;

    private Vector3 originalLocalPosition;
    private bool originalCaptured = false;
    private Coroutine currentShake;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EffectLightExplosion.OnExploed += HandleExploed;
    }

    private void OnDisable()
    {
        EffectLightExplosion.OnExploed -= HandleExploed;    
    }


    private void HandleExploed(Vector2 pos, float range, float intensity)
    {
        TriggerShake(pos, range, intensity);
        Debug.Log("camraShake");
    }    

    private void TriggerShake(Vector2 positionEx, float range, float maxIntensity)
    {
        if (playerTranform == null) return;
        if(range <= 0) return;

        float distance = Vector2.Distance(positionEx, playerTranform.position);
        if (distance > range) return;

        float intensity = Mathf.Clamp01(1f - (distance/range)) * maxIntensity;

        if(!originalCaptured)
        {
            originalCaptured = true;
            originalLocalPosition = transform.localPosition;
            _animator.enabled = false;
        }    

        if(currentShake != null)
        {
            StopCoroutine(currentShake);
            currentShake = null;

            transform.localPosition = originalLocalPosition;
        }

        currentShake = StartCoroutine(Shake(timeDuration, intensity));
       

    }

    private IEnumerator Shake(float duration, float magnitude)
    {
       
        float elapsed = 0f;
        float seed = Random.Range(0f, 1000f);

        while (elapsed < duration)
        {
            float dampingFactor = Mathf.Pow(1 - (elapsed/duration), damping);

            float nx = Mathf.PerlinNoise(seed, elapsed * frequency);
            float ny = Mathf.PerlinNoise(seed * 37.1f, elapsed * frequency);

            float offX = (nx * 2f - 1f) * magnitude * dampingFactor;
            float offY = (ny * 2f - 1f) * magnitude * dampingFactor;

            transform.localPosition = originalLocalPosition + new Vector3(offX, offY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;    
        currentShake = null;
        
    }

    public void ResetOriginalCapture()
    {
        originalCaptured = false;
    }
}
