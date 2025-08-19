using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float _timeDuration = 0.4f;
    [SerializeField] float _damping = 1.5f;
    [SerializeField] float _frequency = 20f;
    [SerializeField] private float _seedRange = 500f;
    [SerializeField] private float _seedMultiplier = 37.1f;
    [SerializeField] Transform _playerTranform;

    private Vector3 _originalLocalPosition;
    private bool _originalCaptured = false;
    private Coroutine _currentShake;

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
    }    

    private void TriggerShake(Vector2 positionEx, float range, float maxIntensity)
    {
        if (_playerTranform == null) return;
        if(range <= 0) return;

        float distance = Vector2.Distance(positionEx, _playerTranform.position);
        if (distance > range) return;

        float intensity = Mathf.Clamp01(1f - (distance/range)) * maxIntensity;

        if(!_originalCaptured)
        {
            _originalCaptured = true;
            _originalLocalPosition = transform.localPosition;
            _animator.enabled = false;
        }    

        if(_currentShake != null)
        {
            StopCoroutine(_currentShake);
            _currentShake = null;

            transform.localPosition = _originalLocalPosition;
        }

        _currentShake = StartCoroutine(Shake(_timeDuration, intensity));
       

    }

    private IEnumerator Shake(float duration, float magnitude)
    {
       
        float elapsed = 0f;
        float seed = Random.Range(0f, _seedRange);

        while (elapsed < duration)
        {
            float dampingFactor = Mathf.Pow(1 - (elapsed/duration), _damping);

            float nx = Mathf.PerlinNoise(seed, elapsed * _frequency);
            float ny = Mathf.PerlinNoise(seed * _seedMultiplier, elapsed * _frequency);

            float offX = (nx * 2f - 1f) * magnitude * dampingFactor;
            float offY = (ny * 2f - 1f) * magnitude * dampingFactor;

            transform.localPosition = _originalLocalPosition + new Vector3(offX, offY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalLocalPosition;    
        _currentShake = null;
        
    }

    public void ResetOriginalCapture()
    {
        _originalCaptured = false;
    }
}
