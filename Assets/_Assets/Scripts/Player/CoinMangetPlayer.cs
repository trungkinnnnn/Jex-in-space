
using UnityEngine;

public class CoinMangetPlayer : MonoBehaviour
{

    private const string NAME_TAG_COIN = "Coin";
    public float addforceEnter = 0.5f;
    public float addforceStay = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(NAME_TAG_COIN))
        {
            var coin = collision.GetComponent<Rigidbody2D>();
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            coin.AddForce(direction * addforceEnter, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(NAME_TAG_COIN))
        {
            var coin = collision.GetComponent<Rigidbody2D>();
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            coin.AddForce(direction * addforceStay, ForceMode2D.Force);
        }
    }

}
