using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletController : MonoBehaviour
{
    private float speed;
    private Vector2 direction;
    private Rigidbody2D rb;

    public void Init(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        Destroy(gameObject, 5f);
    }

}
