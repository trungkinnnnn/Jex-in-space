using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JexMovement : MonoBehaviour
{
    [SerializeField] public JexData jexData;
    [SerializeField] Transform positionForce;
    [SerializeField] Transform rotationFishZero;
    private Vector3 direction;

    private float addForceMax;
    private float addForceMin;
    private float addTorque_Max;
    private float addTorque_Min;
    private float addTorqueWind;

    private float timeRecovery;
    private float timeLastShoot;
    // component
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        addForceMax = jexData.addForceMax;
        addForceMin = jexData.addForceMin;
        addTorque_Max = jexData.addForceTorqueInput_Max;
        addTorque_Min = jexData.addForceTorqueInput_Min;
        addTorqueWind = jexData.addForceTorqueWind;
        timeRecovery = jexData.timeRecovery;

        timeLastShoot = Time.deltaTime; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 direction = transform.position - positionForce.position;
            float timeNow = Time.time;
            if (timeNow - timeLastShoot >= timeRecovery)
            {
                rb.AddTorque(-addTorque_Max, ForceMode2D.Impulse);
                rb.AddForce(direction * addForceMax, ForceMode2D.Impulse);
                Debug.Log("Sử dụng lực mạnh");
            }
            else
            {
                rb.AddForce(direction * addForceMin, ForceMode2D.Impulse);
                rb.AddTorque(-addTorque_Min, ForceMode2D.Impulse);
                Debug.Log("Sử dụng lực nhẹ");
            }

            timeLastShoot = timeNow;
        }
        else
        {
            // gió xoay nhẹ
            rb.AddTorque(-addTorqueWind, ForceMode2D.Force);
        }

        rotationFishZero.rotation = Quaternion.Euler(Vector3.zero);

    }
}
