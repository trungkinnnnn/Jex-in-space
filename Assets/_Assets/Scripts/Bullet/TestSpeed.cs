using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeed : MonoBehaviour
{
    public float bulletSpeed = 1f;

    private void Update()
    {
        transform.position += Vector3.right * bulletSpeed * Time.deltaTime;
        
    }
}
