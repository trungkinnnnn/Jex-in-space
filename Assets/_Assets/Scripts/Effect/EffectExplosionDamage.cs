using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosionDamage : EffectLightExplosion
{

    private const string NAME_TAG_AST = "Ast";
    private const string NAME_TAG_ITEMHEALTH = "Health";
    private const string NAME_TAG_PLAYER = "Player";
    private bool _playerCanTakeDamage = true;

    public int damage = 1;


    public void InitSetUpTakeDamagePlayer(bool playerCanTakeDamage)
    {
        _playerCanTakeDamage = playerCanTakeDamage;
    }    

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

        
        if(collision.CompareTag(NAME_TAG_AST) || collision.CompareTag(NAME_TAG_ITEMHEALTH))
        {
            Ast ast = collision.GetComponent<Ast>();
            if (ast != null)
            {
                ast.TakeDameBullet(damage);
            }
        }    
       
        if(collision.CompareTag(NAME_TAG_PLAYER) && _playerCanTakeDamage)
        {
            PlayerHealth jexHeatlh = collision.GetComponent<PlayerHealth>();
            if (jexHeatlh != null)
            {
                jexHeatlh.TakeDamage(damage);
            }    
        }    
       
    }
}
