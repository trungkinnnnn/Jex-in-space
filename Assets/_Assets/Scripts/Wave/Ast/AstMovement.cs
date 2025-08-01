using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstMovement : MonoBehaviour
{
    [SerializeField] MovementData movementData;
    private Transform playerPostion;
    private Rigidbody2D rb;

    private float addForceRandom;
    private float addForceTowerRandom;
    private float addTorqueRandom;
    private float timeDelay;

    private float minScreen = 0.1f;
    private float maxScreen = 1.1f;
    private float minScreenDestroy = 0.4f;
    private float maxScreenDestroy = 1.4f;
    private float timeDestroy = 1f;
   

    public void SetTranformPlayer(Transform position)
    {
        playerPostion = position;
    }

    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        RandomValue(); 
        StartCoroutine(TimeDelay());
    }

    private void Update()
    {
        if (rb.velocity.magnitude < movementData.minSpeed)
        {
            if (!IsInView(transform.position))
            {
                AddForceOutScreen();
            }
        }
        IsInToFar(transform.position);
    }

    private IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(timeDelay);
    }

    private void RandomValue()
    {
        addForceRandom = Random.Range(movementData.addForceMin, movementData.addForceMax);
        addForceTowerRandom = Random.Range(movementData.addForceTorqueMin, movementData.addForceTorqueMax);
        timeDelay = movementData.timeBack;
        AddForceStart();
    }

    private void AddForceStart()
    {
        Vector2 direction = (playerPostion.position - transform.position).normalized;
        rb.AddForce(direction * addForceRandom, ForceMode2D.Impulse);
        rb.AddTorque(addForceTowerRandom, ForceMode2D.Impulse);
    }

    private void AddForceOutScreen()
    {
        Vector2 direction = (playerPostion.position - transform.position).normalized;
        rb.AddForce(direction * movementData.addForceMin, ForceMode2D.Force);
        rb.AddTorque(movementData.addForceTorqueMin, ForceMode2D.Force);
    }

    private bool IsInView(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        return viewPos.x > -minScreen && viewPos.y > -minScreen && viewPos.x < maxScreen && viewPos.y < maxScreen;
    }

    private void IsInToFar(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        if (viewPos.x < -minScreenDestroy || viewPos.y < -minScreenDestroy || viewPos.x > maxScreenDestroy || viewPos.y > maxScreenDestroy) AtsDestroy();
    }

    protected void AtsDestroy()
    {
        Destroy(gameObject, timeDestroy);
    }

}
