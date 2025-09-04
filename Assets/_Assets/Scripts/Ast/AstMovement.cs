using System.Collections;
using UnityEngine;

public class AstMovement : MonoBehaviour
{
    [SerializeField] MovementData _movementData;
    private Transform _playerPostion;
    private Rigidbody2D _rb;

    private float addForceRandom;
    private float _addForceTowerRandom;
    private float _addTorqueRandom;
    private float _timeDelay;

    private float _minScreen = 0.05f;
    private float _maxScreen = 1.05f;
    private float _minScreenDestroy = 0.4f;
    private float _maxScreenDestroy = 1.4f;
    private float _timeDestroy = 0f;
   

    public void SetTranformPlayer(Transform position)
    {
        _playerPostion = position;
    }

    private void Start()
    { 
        _rb = GetComponent<Rigidbody2D>();
        RandomValue(); 
        StartCoroutine(TimeDelay());
    }

    private void Update()
    {
        if (_rb.velocity.magnitude < _movementData.minSpeed)
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
        yield return new WaitForSeconds(_timeDelay);
    }

    private void RandomValue()
    {
        addForceRandom = Random.Range(_movementData.addForceMin, _movementData.addForceMax);
        _addForceTowerRandom = Random.Range(_movementData.addForceTorqueMin, _movementData.addForceTorqueMax);
        _timeDelay = _movementData.timeBack;
        AddForceStart();
    }

    private void AddForceStart()
    {
        Vector2 direction = (_playerPostion.position - transform.position).normalized;
        _rb.AddForce(direction * addForceRandom, ForceMode2D.Impulse);
        _rb.AddTorque(_addForceTowerRandom, ForceMode2D.Impulse);
    }

    private void AddForceOutScreen()
    {
        Vector2 direction = (_playerPostion.position - transform.position).normalized;
        _rb.AddForce(direction * _movementData.addForceOutScreen, ForceMode2D.Force);
        _rb.AddTorque(_movementData.addForceTorqueMin, ForceMode2D.Force);
    }

    private bool IsInView(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        return viewPos.x > -_minScreen && viewPos.y > -_minScreen && viewPos.x < _maxScreen && viewPos.y < _maxScreen;
    }

    private void IsInToFar(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        if (viewPos.x < -_minScreenDestroy || viewPos.y < -_minScreenDestroy || viewPos.x > _maxScreenDestroy || viewPos.y > _maxScreenDestroy) AtsDestroy();
    }

    protected void AtsDestroy()
    {
        Destroy(gameObject, _timeDestroy);
    }

}
