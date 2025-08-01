using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRespon : MonoBehaviour
{
    public float zDepth = 10f; 

    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        // Màu xanh để hiển thị vùng viewport
        Gizmos.color = Color.green;

        // Tính toán điểm ở khoảng cách zDepth so với camera
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.4f, -0.4f, zDepth));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1.4f, -0.4f, zDepth));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1.4f, 1.4f, zDepth));
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.4f, 1.4f, zDepth));

        // Vẽ khung hình chữ nhật
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
