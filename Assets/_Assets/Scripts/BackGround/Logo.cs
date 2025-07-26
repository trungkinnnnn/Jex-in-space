using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Logo : MonoBehaviour
{
    [SerializeField] LogoScripTable logoData;

    private float timeAddForce;
    private float addForce;
    private float addTorque;
    private float angle;

    private float timeReset;
    private float timeStart;

    private Vector3 direction;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeStart = logoData.timeStart;
        ResetForce();
    }

    // Update is called once per frame
    void Update()
    {
        timeStart -= Time.deltaTime;
        if(timeStart < 0)
        {
            rb.AddForce(direction * addForce, ForceMode2D.Force);
            rb.AddTorque(addTorque, ForceMode2D.Force);
            ResetForce();
            timeStart = logoData.timeReset;
        }    
        Debug.Log("Time : " + timeStart);   
        
    }

    private void ResetForce()
    {
        addForce = Random.Range(logoData.addForce_min, logoData.addForce_max);
        addTorque = Random.Range(logoData.addForceTorque_min, logoData.addForceTorque_max);
        angle = Random.Range(0f, 360f);

        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }    
}
