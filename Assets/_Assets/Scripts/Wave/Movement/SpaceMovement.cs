using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMovement : MonoBehaviour
{
    [SerializeField] MovementData movementData;
    private Transform playerPostion;

    private float addForceRandom;
    private float addForceTowerRandom;

    private float addTorqueRandom;
    private float timeDelay;

    private Rigidbody2D rb;

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
                AddForce();
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
        AddForce();
    }

    private void AddForce()
    {
        Vector2 direction = (playerPostion.position - transform.position).normalized;
        rb.AddForce(direction * addForceRandom, ForceMode2D.Impulse);
        rb.AddTorque(addForceTowerRandom, ForceMode2D.Impulse);
    }

    private bool IsInView(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        return viewPos.x > -0.1 && viewPos.y > -0.1 && viewPos.x < 1.1 && viewPos.y < 1.1;
    }

    private void IsInToFar(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        if (viewPos.x < -0.4 || viewPos.y < -0.4 || viewPos.x > 1.4 || viewPos.y > 1.4) Destroy(gameObject, 3f);
    }    
   

}
