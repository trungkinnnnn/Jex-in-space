using System;
using UnityEngine;

public class BounceOffWall : MonoBehaviour
{
    public static Action OnAnnoyed;

    public float bounceForceEnter = 1f;
    public string wallTag = "Wall";

    private Rigidbody2D _rb;
    private PlayerAudio _audio;

    private void Awake()
    {
        _audio = GetComponent<PlayerAudio>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(wallTag))
        {
            _audio.PlayClipImpactWall();
            BounceBreak(collision, bounceForceEnter);
            OnAnnoyed?.Invoke();
        }
    }

    private void BounceBreak(Collision2D collision, float force)
    {
        Vector2 playerPostion = _rb.position;
        Vector2 contact = collision.contacts[0].point;
       
        Vector2 bounceDir = (playerPostion - contact).normalized;

        _rb.AddForce(bounceDir * force, ForceMode2D.Impulse);

    }
}
