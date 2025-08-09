using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Logo : MonoBehaviour
{
    [SerializeField] LogoScripTable _logoData;

    private float _timeAddForce;
    private float _addForce;
    private float _addTorque;
    private float _angle;

    private float _timeReset;
    private float _timeStart;

    private Vector3 _direction;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _timeStart = _logoData.timeStart;
        ResetForce();
    }

    // Update is called once per frame
    void Update()
    {
        _timeStart -= Time.deltaTime;
        if(_timeStart < 0)
        {
            _rb.AddForce(_direction * _addForce, ForceMode2D.Force);
            _rb.AddTorque(_addTorque, ForceMode2D.Force);
            ResetForce();
            _timeStart = _logoData.timeReset;
        }    
        Debug.Log("Time : " + _timeStart);   
        
    }

    private void ResetForce()
    {
        _addForce = Random.Range(_logoData.addForce_min, _logoData.addForce_max);
        _addTorque = Random.Range(_logoData.addForceTorque_min, _logoData.addForceTorque_max);
        _angle = Random.Range(0f, 360f);

        _direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
    }    
}
