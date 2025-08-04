using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosionDmage : EffectLightExplosion
{
    public int dmage = 1;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null)
        {

            Vector2 dir = rb.transform.position - transform.position;
            float distance = dir.magnitude;

            if (distance > 0 && distance <= explosionRadius)
            {
                float fallof = 1f - (distance / explosionRadius);
                rb.AddForce(dir.normalized * forceEnter * fallof, ForceMode2D.Impulse);
            }
        }

        Ast ast = collision.GetComponent<Ast>();
        if (ast != null)
        {
            ast.TakeDameBullet(dmage);
        }    
    }
}
