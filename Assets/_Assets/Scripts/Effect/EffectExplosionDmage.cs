using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosionDmage : EffectLightExplosion
{

    private const string NAME_TAG_AST = "Ast";
    private const string NAME_TAG_PLAYER = "Player";

    public int damage = 1;
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

        
        if(collision.CompareTag(NAME_TAG_AST))
        {
            Ast ast = collision.GetComponent<Ast>();
            if (ast != null)
            {
                ast.TakeDameBullet(damage);
            }
        }    
       
        if(collision.CompareTag(NAME_TAG_PLAYER))
        {
            JexHeatlh jexHeatlh = collision.GetComponent<JexHeatlh>();
            if (jexHeatlh != null)
            {
                jexHeatlh.TakeDameAst(damage);
            }    
        }    
       
    }
}
