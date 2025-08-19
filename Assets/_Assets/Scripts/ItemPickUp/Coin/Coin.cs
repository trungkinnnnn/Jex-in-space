using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour, IPickUp
{

    public const string NAME_TAG_PLAYER = "Player";
    public float timeLifeMax = 10f;
    public float timeLifeMin = 7f;
    public float timeAlphaDown = 3f;
    public float scaleMax = 2f;
    public float scaleMin = 1f;
    public float addforceMin = 0.1f;
    public float addforceMax = 0.5f;
    public float angleMin = 0f;
    public float angleMax = 360f;

    private int _quanity = 1;
    private float _timelife;
    private float _timeAlphaToggePlayer = 0.5f;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        SetUp();
        StartCoroutine(LifeCycle());
    }

    private void SetUp()
    {
        _timelife = Random.Range(timeLifeMin, timeLifeMax);
        float scale = Random.Range(scaleMin, scaleMax);
        float force = Random.Range(addforceMin, addforceMax);
        float angle = Random.Range(angleMin, angleMax);

        transform.localScale = new Vector3(scale, scale, 1f);

        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin( angle * Mathf.Deg2Rad));

        _rb.AddForce(force * direction, ForceMode2D.Impulse);
    }    
   
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(_timelife);
        yield return DestroyOfAlphaZero(timeAlphaDown);
    }

    private IEnumerator DestroyOfAlphaZero(float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            ChangeAlpha(Mathf.Lerp(1f, 0f, elapsed / time));
            yield return null;
        }
        Destroy(gameObject);
    }    

    private void ChangeAlpha(float alpha)
    {
        Color color = _sprite.color;
        color.a = alpha;
        _sprite.color = color;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(NAME_TAG_PLAYER))
        {
            StopAllCoroutines();
            _rb.velocity = Vector2.zero;
            OnPickup(collision.gameObject);
            StartCoroutine(DestroyOfAlphaZero(_timeAlphaToggePlayer));
        }    
    }

    public void OnPickup(GameObject collector)
    {
        var inventory = collector.GetComponent<PlayerInventory>();
        if (inventory == null) return;
        inventory.HandleAddCoin(_quanity);
    }

}
