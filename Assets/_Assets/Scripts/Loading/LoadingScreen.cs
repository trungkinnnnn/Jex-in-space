using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Material _material;
    [SerializeField] AnimationCurve _animationCurveShow = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve _animationCurveHide = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private static string _para_MAT_RADIUS = "_Radius";
    private static string _para_MAT_ALPHA = "_Alpha";

    public static LoadingScreen Instance;
    public float _timeDuration = 0.5f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    public IEnumerator ShowCircle()
    {
        _material.SetFloat(_para_MAT_RADIUS, 1f);
        _material.SetFloat(_para_MAT_ALPHA, 1f);
        yield return StartCoroutine(SetValue(_para_MAT_RADIUS, 1f, 0f, _animationCurveShow, _timeDuration));
    }

    public IEnumerator HideCircle()
    {
        _material.SetFloat(_para_MAT_ALPHA, 1f);
        _material.SetFloat(_para_MAT_RADIUS, 0f);
        yield return StartCoroutine(SetValue(_para_MAT_RADIUS, 0f, 1f, _animationCurveHide, _timeDuration)); 
    }

    public IEnumerator ShowAlpha(float duration)
    {
        _material.SetFloat(_para_MAT_RADIUS, 0f);
        _material.SetFloat(_para_MAT_ALPHA, 0f);
        yield return StartCoroutine(SetValue(_para_MAT_ALPHA, 0f, 1f, _animationCurveShow, duration));
    }

    public IEnumerator HideAlpha(float duration)
    {
        _material.SetFloat(_para_MAT_RADIUS, 0f);
        _material.SetFloat(_para_MAT_ALPHA, 1f);
        yield return StartCoroutine(SetValue(_para_MAT_ALPHA, 1f, 0f, _animationCurveHide, duration));
    }

    private IEnumerator SetValue(string namePara,float start, float end, AnimationCurve curveLoad, float duration = 0.5f)
    {
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float curve = curveLoad.Evaluate(timer/ duration);

            float value = Mathf.Lerp(start, end, curve);
            _material.SetFloat(namePara, value);
            yield return null;
        }
    }
    
}
