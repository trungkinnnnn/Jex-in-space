using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JexHeatlh : MonoBehaviour
{
    [SerializeField] JexData data;
    private Rigidbody2D rb;

    private string NAME_TAG_AST = "Ast";
    private string NAME_ITEM_HEALTH = "Health";

    private int hpMax;
    private int currentHp;
    private float timeImmortal;
    private float lastTimeTakeDame = 0f;
    public float addforceImpact = 0.1f;

    private int dmage = 1;
    private int health = 1;

    public float impactSpeedMax = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hpMax = data.health;
        currentHp = hpMax;
        timeImmortal = data.timeImmortal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(NAME_TAG_AST))
        {
            float impactSpeed = collision.relativeVelocity.magnitude;

            //Debug.Log("Speed : " +  impactSpeed);   

            if(impactSpeed >= impactSpeedMax && Time.time > lastTimeTakeDame)
            {
                lastTimeTakeDame = Time.time + timeImmortal;
                TakeDameAst(dmage);

                AddForce(collision.collider.transform.position);
            }    
        }

        if (collision.collider.CompareTag(NAME_ITEM_HEALTH))
        {
            AddHealth(health);
            
            ItemHealth itemHealth = collision.collider.GetComponent<ItemHealth>();
            if (itemHealth != null) itemHealth.HandleDestroyHealth();
        }
    }

    public void TakeDameAst(int Dmage)
    {
        currentHp -= Dmage;
        Debug.Log("Hp : " + currentHp);
        if(currentHp <= 0)
        {
            Debug.Log("Jex die");
        }    
    }

    private void AddHealth(int health)
    {
        currentHp = Mathf.Min(currentHp + health, hpMax);
        Debug.Log("Hp : " + currentHp);
    }    

    private void AddForce(Vector3 position)
    {
        Vector3 direction = (transform.position - position).normalized;
        rb.AddForce(direction * addforceImpact, ForceMode2D.Impulse);
    }    

}
