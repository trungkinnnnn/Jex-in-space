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


    public static LoadingScreen Instance;
    public float _timeDuration = 0.5f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        _material.SetFloat(_para_MAT_RADIUS, 1f);
    }

    public IEnumerator Show()
    {
        _material.SetFloat(_para_MAT_RADIUS, 1f);
        yield return StartCoroutine(SetRaidus(1f, 0f, _animationCurveShow));
    }

    public IEnumerator Hide()
    {
        _material.SetFloat(_para_MAT_RADIUS, 0f);
        yield return StartCoroutine(SetRaidus(0f, 1f, _animationCurveHide)); 
    }

    private IEnumerator SetRaidus(float start, float end, AnimationCurve curveLoad)
    {
        float timer = 0f;
        while(timer < _timeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float curve = curveLoad.Evaluate(timer/_timeDuration);

            float radius = Mathf.Lerp(start, end, curve);
            _material.SetFloat(_para_MAT_RADIUS, radius);
            yield return null;
        }
    }
    
}
