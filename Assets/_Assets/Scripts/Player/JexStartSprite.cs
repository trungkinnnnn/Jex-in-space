using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JexStartSprite : MonoBehaviour
{
    private Animator animator;
    public float timeSpin = 3;

    private string NAME_ANI_TRIGGER_SCALEUP = "isScaleUp";

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TimeStart(timeSpin));
    }

    private IEnumerator TimeStart(float timeSpin)
    {
        yield return new WaitForSeconds(timeSpin);
        animator.SetTrigger(NAME_ANI_TRIGGER_SCALEUP);
    }

}
