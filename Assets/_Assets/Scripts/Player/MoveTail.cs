using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTail : MonoBehaviour
{
    [Header("TailSetting")]
    [SerializeField] Transform _tailTransform;
    public float tail_angleMin = -25f;
    public float tail_angleMax = 25f;
    public float tailSmooth = 5f;

    [Header("TailV2Setting")]
    [SerializeField] Transform _tailV2Transform;
    public float tailV2_angleMin = -10f;
    public float tailV2_angleMax = 20f;
    public float tailV2Smooth = 7f;


    [Header("Rotation Mapping")]
    [SerializeField] float minRotSpeed = -250f;
    [SerializeField] float maxRotaSpeed = -90f;

    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float rotSpeed = _rb.angularVelocity;

        float t = Mathf.InverseLerp(minRotSpeed, maxRotaSpeed, rotSpeed);

        SetUpAngle(_tailTransform, tail_angleMin, tail_angleMax, tailSmooth, t);

        SetUpAngle(_tailV2Transform, tailV2_angleMin, tailV2_angleMax, tailV2Smooth, t);


    }

    private void SetUpAngle(Transform transform, float minAngle, float maxAngle, float smooth, float t)
    {
        float target = Mathf.Lerp(maxAngle, minAngle, t);
        float currentAngle = Mathf.LerpAngle(transform.localEulerAngles.z, target, Time.deltaTime * smooth);
        transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
    }


}
