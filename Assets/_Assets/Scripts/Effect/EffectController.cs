using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(WaitTimeDestroy(_animator));
    }

    public void ApplyHexColor(string hexColor)
    {
        Color color;
        string hexString = "#" + hexColor;
        //Debug.Log(hexString);
        if(ColorUtility.TryParseHtmlString(hexString, out color))
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if( _spriteRenderer != null )
            {
                _spriteRenderer.color = color;
            }
        }
        else
        {
            Debug.Log("Hexa not true");
        }
    }    

    private IEnumerator WaitTimeDestroy(Animator ani)
    {
        RandomAngle();
        yield return new WaitForSeconds(GetTimeAni(ani));
        Destroy(gameObject);
    }    

    private void RandomAngle()
    {
        float angle = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }    

    private float GetTimeAni(Animator animator)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        float clip = ac.animationClips[0].length;
        return clip;
    }    

}
