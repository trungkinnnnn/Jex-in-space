using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstChildren : MonoBehaviour
{ 
    private float minScreenDestroy = 0.1f;
    private float maxScreenDestroy = 1.1f;
    private float timeDestroy = 2f;

    private void Update()
    {
        if(IsInToFar(transform.position)) Destroy(gameObject, timeDestroy);
    }

    private bool IsInToFar(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        if (viewPos.x < -minScreenDestroy || viewPos.y < -minScreenDestroy || viewPos.x > maxScreenDestroy || viewPos.y > maxScreenDestroy) return true;
        return false;
    }

   
}
