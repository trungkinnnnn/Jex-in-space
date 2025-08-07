using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : BulletBase
{
    protected override void HandleHitAst(Collider2D other)
    {
        CreateEffectHit();
        Destroy(gameObject);
    }
}
