using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public static Action<Ast> OnhitAst;

    private float speed;
    private Vector2 direction;
    private Rigidbody2D rb;

    protected string NAME_COMPARETAG_PHYSIC = "Ast";

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

    protected abstract void OnTriggerEnter2D(Collider2D other);

}
