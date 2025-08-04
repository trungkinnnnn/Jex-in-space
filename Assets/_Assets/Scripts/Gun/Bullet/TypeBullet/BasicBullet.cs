using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : BulletBase
{
    private int dmage = 1;
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(NAME_COMPARETAG_PHYSIC))
        {
            CreateEffectHit();

            AstHitBullet(other);

            Destroy(gameObject);
        }
    }

    private void AstHitBullet(Collider2D other)
    {
        Ast obj = other.GetComponent<Ast>();
        if (obj != null)
        {
            obj.TakeDameBullet(dmage);
        }
    }

}
