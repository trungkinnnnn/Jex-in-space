using System.Collections;
using UnityEngine;

public class Warning : MonoBehaviour
{
    private readonly int HASH_ANI_WARING = Animator.StringToHash("isWarning");
    private Animator _animator;
    private SpriteRenderer _sprite;
    private float alphaStart = 1f;
    private float alphaEnd = 0f;

    public float timeOffAlphaStart = 0.5f;
    public float timeOffAlphaDestroy = 2f;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger(HASH_ANI_WARING);
        StartCoroutine(SmoothAlphaStart());
    }

    private IEnumerator SmoothAlphaStart()
    {
        float elapsed = 0f;
        while( elapsed < timeOffAlphaStart)
        {
            elapsed += Time.deltaTime;  
            float alpha = Mathf.Lerp(alphaEnd, alphaStart, elapsed / timeOffAlphaStart);
            SetAlpha(alpha);
            yield return null;
        }    
    }


    public void SmoothDestroy()
    {
        StartCoroutine(SmoothAlphaDestroy());
    }
    private IEnumerator SmoothAlphaDestroy()
    {
        float elapsed = 0f;
        while (elapsed < timeOffAlphaDestroy)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(alphaStart, alphaEnd, elapsed / timeOffAlphaDestroy);
            SetAlpha(alpha);
            yield return null;
        }    
        Destroy(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _sprite.color;
        color.a = alpha;    
        _sprite.color = color;  
    }    
}
