using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstBackGround : MonoBehaviour
{
    public float speedMax = 0.4f;
    public float speedMin = 0.2f;
    private float speedStart;
    private float currentAnge = 0f;

    private float angleMove;
    private Vector3 direction;

    public float speedRotation = 0.5f;
    public float minPosition = 0.1f;
    public float maxPosition = 1.1f;

    void Start()
    {
        RandomStar();
    }

    void Update()
    {
        transform.position += direction * speedStart * Time.deltaTime;
        currentAnge += speedRotation * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, currentAnge);
        CheckOutScreen(transform.position);
    }

    private void CheckOutScreen(Vector3 position)
    {
        Vector3 viewPort = Camera.main.WorldToViewportPoint(position);

        if (viewPort.x < -minPosition || viewPort.x > maxPosition) direction.x *= -1;
        if (viewPort.y < -minPosition || viewPort.y > maxPosition) direction.y *= -1;
    }

    private void RandomStar()
    {
        speedStart = Random.Range(speedMin, speedMax);
        angleMove = Random.Range(0, 360);
        direction = new Vector2(Mathf.Cos(angleMove * Mathf.Deg2Rad), Mathf.Sin(angleMove * Mathf.Deg2Rad));
    }

}
