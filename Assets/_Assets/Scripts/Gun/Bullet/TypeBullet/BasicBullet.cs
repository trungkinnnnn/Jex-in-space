using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : BulletBase
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Ast ast = other.GetComponent<Ast>();
        if(ast != null && other.CompareTag(NAME_COMPARETAG_PHYSIC))
        {
            OnhitAst?.Invoke(ast);
            Destroy(gameObject);
        }
    }
}
