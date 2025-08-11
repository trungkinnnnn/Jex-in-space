using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani_Warning : MonoBehaviour
{
    private readonly int HASH_ANI_WARING = Animator.StringToHash("isWarning");

    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger(HASH_ANI_WARING);
    }
}
