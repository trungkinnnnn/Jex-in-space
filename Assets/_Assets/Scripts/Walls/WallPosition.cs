using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPosition : MonoBehaviour
{
    [SerializeField] Transform top;
    [SerializeField] Transform right;
    [SerializeField] Transform bottom;
    [SerializeField] Transform left;

    public float timeStart = 7f;

    void Start()
    {
        StartCoroutine(UpdateWallPosition());
    }

    private IEnumerator UpdateWallPosition()
    {
        yield return new WaitForSeconds(timeStart);
        Camera cam = Camera.main;
        Vector3 camePos = cam.transform.position;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float halfWidth = camWidth / 2;
        float halfHeight = camHeight / 2;

        top.position = new Vector3(camePos.x, camePos.y + halfHeight + top.localScale.y/2f, 0f);
        bottom.position = new Vector3(camePos.x, camePos.y -halfHeight - bottom.localScale.y / 2f, 0f);
        right.position = new Vector3(camePos.x + halfWidth + right.localScale.x / 2f, camePos.y, 0f);
        left.position = new Vector3(camePos.x - halfWidth - left.localScale.x / 2f, camePos.y, 0f);

        top.localScale = new Vector3(camWidth, top.localScale.y, 1f);
        bottom.localScale = new Vector3(camWidth, bottom.localScale.y, 1f);
        right.localScale = new Vector3(right.localScale.x, camHeight, 1f);
        left.localScale = new Vector3(left.localScale.x, camHeight, 1f);
    }
}
