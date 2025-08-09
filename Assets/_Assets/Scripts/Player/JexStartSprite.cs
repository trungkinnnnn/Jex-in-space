using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JexStartSprite : MonoBehaviour
{
    private Animator _animator;
    public float timeSpin = 3;

    private static readonly int HashIsScaleUp = Animator.StringToHash("isScaleUp");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(TimeStart(timeSpin));
    }

    private IEnumerator TimeStart(float timeSpin)
    {
        yield return new WaitForSeconds(timeSpin);
        _animator.SetTrigger(HashIsScaleUp);
    }

}
