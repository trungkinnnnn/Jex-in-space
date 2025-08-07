
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    //Data
    [SerializeField] protected List<GameObject> effectHits;
    [SerializeField] protected GameObject lightEffect;


    //Para
    private Vector2 direction;
    private float speed;
    private float timeLife = 3f;
    private int damage = 1;
    public string COLORHEXA = "FFC015";

    [Header("PARA EXPLOSION LIGHT")]
    public float radiusEffectLight = 0.4f;
    public float forceStay = 0.8f;
    public float forceEnter = 0.5f;
   
    protected string NAME_COMPARETAG_PHYSIC = "Ast";
    protected string NAME_COMPARETAG_PHYSIC_ITEM = "Health";

    //Component
    private Rigidbody2D rb;

    public void Init(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        Destroy(gameObject, timeLife);
    }

    // Hiệu ứng va chạm
    protected virtual void CreateEffectHit()
    {
        GameObject prefabEffect = RandomEffect();
        var objEffect = Instantiate(prefabEffect, transform.position, Quaternion.identity);
        EffectController effect = objEffect.GetComponent<EffectController>();
        effect.ApplyHexColor(COLORHEXA);

        CreateEffectLight();

    }    


    // Hiệu ứng ánh sáng
    private void CreateEffectLight()
    {
        if (lightEffect == null) return;
        GameObject effect = Instantiate(lightEffect, transform.position, Quaternion.identity);
        EffectLightExplosion effectLight = effect.GetComponent<EffectLightExplosion>();
        effectLight.InitRadius(radiusEffectLight);
        effectLight.InitForce(forceEnter, forceStay);
    }    

    protected GameObject RandomEffect()
    {
        if(effectHits.Count == 1) return effectHits[0];
        int random = Random.Range(0, effectHits.Count);
        return effectHits[random];
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(NAME_COMPARETAG_PHYSIC) || other.CompareTag(NAME_COMPARETAG_PHYSIC_ITEM))
        {
            HandleHitAst(other);
            AstHitBullet(other);
        }
    }

    protected abstract void HandleHitAst(Collider2D other);

    private void AstHitBullet(Collider2D other)
    {
        Ast obj = other.GetComponent<Ast>();
        if (obj != null)
        {
            obj.TakeDameBullet(damage);
        }
    }

}
