
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Escape : MonoBehaviour
{
    [SerializeField] Transform _pointDropTransform;
    [SerializeField] GameObject _powerPrefab;

    public Vector2 positionStart = new Vector2(4.35f, 0f);
    public Vector3 rotationStart = new Vector3(0f, 0f, 0f);

    [Header("Thời gian giai đoạn")]
    [SerializeField] private float timePhase1 = 0.5f;
    [SerializeField] private float timePhase2 = 2f;

    [Header("Speed giai đoạn")]
    [SerializeField] private float speedPhase1 = 3f;
    [SerializeField] private float speedPhase2 = 1f;
    [SerializeField] private float speedPhase3 = 3f;

    

    public float turnAfter = 0.5f;
    public float turnBefor = 2f;
    public float angulerSpeed = 90f;
    public float timeActive = 5f;


    public float timeRespawn = 0.1f;

    private float _lastRespawn = 0f;
    private float _timeOnAngle = 0f;
    private Vector2 _direction;

    private void Start()
    {
        PlayEscape();
    }

    private void Update()
    {
        TimeCreate();
        _timeOnAngle += Time.unscaledTime;
    }

    private void PlayEscape()
    {
        transform.localPosition = positionStart;
        transform.rotation = Quaternion.Euler(rotationStart);

        float angleRad = rotationStart.z * Mathf.Deg2Rad;
        _direction = new Vector2(-Mathf.Sin(angleRad), Mathf.Cos(angleRad));

        gameObject.SetActive(true);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float time = 0f;
        while (time < timeActive)
        {
            // Xác định tốc độ theo giai đoạn
            float currentSpeed = speedPhase1;
            if (time <= timePhase1)
                currentSpeed = speedPhase1;
            else if (time <= timePhase1 + timePhase2)
                currentSpeed = speedPhase2;
            else
                currentSpeed = speedPhase3;

            // Xoay vòng nếu trong khoảng turnAfter → turnBefor
            if (time > turnAfter && time < turnBefor)
            {
                float angle = angulerSpeed * Time.unscaledDeltaTime;
                _direction = Quaternion.Euler(0, 0, angle) * _direction;
            }

            // Di chuyển
            transform.localPosition += (Vector3)(_direction.normalized * currentSpeed * Time.unscaledDeltaTime);

            // Cập nhật rotation
            float angleZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, angleZ - 90f);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }


    private void TimeCreate()
    {
        if (Time.unscaledTime - _lastRespawn > timeRespawn)
        {
            CreatePrefabPower();
            _lastRespawn = Time.unscaledTime;
        }
    }

    private void CreatePrefabPower()
    {
        Vector2 direction = _pointDropTransform.position - transform.position;
        var powerlow = Instantiate(_powerPrefab, _pointDropTransform.position, transform.rotation);
        PowerLow power = powerlow.GetComponent<PowerLow>();
        power.Init();
        power.AddForce(direction);
    }

}