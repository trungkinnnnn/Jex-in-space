
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    //Data
    [SerializeField] protected List<GameObject> effectHits;

    // Even
    public static System.Action<Ast> OnhitAst;

    //Para
    private float speed;
    private Vector2 direction;
   
    protected string NAME_COMPARETAG_PHYSIC = "Ast";
    public string COLORHEXA = "FFC015";

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
        Destroy(gameObject, 5f);
    }

    protected virtual void CreateEffectHit()
    {
        GameObject prefabEffect = RandomEffect();
        var objEffect = Instantiate(prefabEffect, transform.position, Quaternion.identity);
        EffectController effect = objEffect.GetComponent<EffectController>();
        effect.ApplyHexColor(COLORHEXA);
    }    

    protected GameObject RandomEffect()
    {
        int random = Random.Range(0, effectHits.Count);
        return effectHits[random];
    }    

    protected abstract void OnTriggerEnter2D(Collider2D other);

}
