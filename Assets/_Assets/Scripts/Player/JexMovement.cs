using UnityEngine;

public class JexMovement : MonoBehaviour
{
    [SerializeField] private JexData _jexData;
    [SerializeField] private Transform _positionForce;
    [SerializeField] private Transform _rotationFishZero;

    private Rigidbody2D _rb;

    private float _addForceMax;
    private float _addForceMin;
    private float _addTorqueMax;
    private float _addTorqueMin;
    private float _addTorqueWind;
    private float _timeRecovery;
    private float _timeLastShoot = -Mathf.Infinity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _addForceMax = _jexData.addForceMax;
        _addForceMin = _jexData.addForceMin;
        _addTorqueMax = _jexData.addForceTorqueInput_Max;
        _addTorqueMin = _jexData.addForceTorqueInput_Min;
        _addTorqueWind = _jexData.addForceTorqueWind;
        _timeRecovery = _jexData.timeRecovery;
    }

    private void Update()
    {
        if (_rotationFishZero != null)
            _rotationFishZero.rotation = Quaternion.identity;

        if (InputManager.isInputLocked)
        {
            _rb.AddTorque(-_addTorqueWind, ForceMode2D.Force);
            return;
        }

        if (Input.GetMouseButtonDown(0) && FireRate.canShoot)
        {
            Vector2 direction = (Vector2)transform.position - (Vector2)_positionForce.position;
            float now = Time.time;
            if (now - _timeLastShoot >= _timeRecovery)
            {
                _rb.AddTorque(-_addTorqueMax, ForceMode2D.Impulse);
                _rb.AddForce(direction * _addForceMax, ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(direction * _addForceMin, ForceMode2D.Impulse);
                _rb.AddTorque(-_addTorqueMin, ForceMode2D.Impulse);
            }
            _timeLastShoot = now;
        }
        else
        {
            _rb.AddTorque(-_addTorqueWind, ForceMode2D.Force);
        }
    }
}
