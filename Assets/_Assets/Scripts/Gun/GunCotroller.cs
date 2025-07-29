using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCotroller : MonoBehaviour
{
    private string NAME_ANI_TRIGGER_SHOOT = "isShoot";
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger(NAME_ANI_TRIGGER_SHOOT);
        }    
    }
}
