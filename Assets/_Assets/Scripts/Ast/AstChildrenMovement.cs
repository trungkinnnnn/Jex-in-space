
using UnityEngine;

public class AstChildrenMovement : MonoBehaviour
{ 
    private float _minScreenDestroy = 0.1f;
    private float _maxScreenDestroy = 1.1f;
    private float _timeDestroy = 2f;

    private void Update()
    {
        if(IsInToFar(transform.position)) Destroy(gameObject, _timeDestroy);
    }

    private bool IsInToFar(Vector3 worldPos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        if (viewPos.x < -_minScreenDestroy || viewPos.y < -_minScreenDestroy || viewPos.x > _maxScreenDestroy || viewPos.y > _maxScreenDestroy) return true;
        return false;
    }

   
}
