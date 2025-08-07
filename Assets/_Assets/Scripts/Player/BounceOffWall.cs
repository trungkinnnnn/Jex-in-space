using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffWall : MonoBehaviour
{
    public static Action OnAnnoyed;

    public float bounceForceEnter = 1f;
    public string wallTag = "Wall";

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(wallTag))
        {
            BounceBreak(collision, bounceForceEnter);
            OnAnnoyed?.Invoke();
        }
    }

    private void BounceBreak(Collision2D collision, float force)
    {
        Vector2 playerPostion = rb.position;
        Vector2 contact = collision.contacts[0].point;
       
        Vector2 bounceDir = (playerPostion - contact).normalized;

        rb.AddForce(bounceDir * force, ForceMode2D.Impulse);

    }
}
