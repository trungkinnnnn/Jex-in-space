
using UnityEngine;

public class LazerBullet : BulletBase
{
    [Header("PARA LAZEBULLET")]
    public int hpAmor = 2;
    public int hpAmorDown = 1;
    protected override void HandleHitAst(Collider2D other)
    {
        CreateEffectHit();

        hpAmor -= hpAmorDown;
        if (hpAmor <= 0)
        {
            Destroy(gameObject);
        }    

    }
}
