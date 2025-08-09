
using UnityEngine;

public class RocketBullet : BulletBase
{
    protected override void HandleHitAst(Collider2D other)
    {
        CreateEffectHit();
        Destroy(gameObject);
    }

    protected override void CreateEffectLight()
    {
        if (_lightEffect == null) return;
        GameObject effect = Instantiate(_lightEffect, transform.position, Quaternion.identity);
        var effectLight = effect.GetComponent<EffectExplosionDamage>();
        effectLight?.InitRadius(radiusEffectLight);
        effectLight?.InitForce(forceEnter, forceStay);
        effectLight?.InitSetUpTakeDamagePlayer(false);
    }
}
