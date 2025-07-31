using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangSpawner
{
    private Vector2 topLeft = new Vector2(-0.3f, 1.3f);
    private Vector2 bottomRight = new Vector2(1.3f, -0.3f);

    private enum Edge { Top, Right, Bottom, Left }

    private List<Edge> clockWiseEdges = new List<Edge> { Edge.Top, Edge.Right, Edge.Bottom, Edge.Left };

    private Edge start;

    public RectangSpawner()
    {
        start = (Edge)Random.Range(0, 4);
    }

    public Vector3 GetSpawnPoint(int index)
    {
        int edgeIndex = (clockWiseEdges.IndexOf(start) + index) % 4;

        Edge edge = clockWiseEdges[edgeIndex];

        float x, y;

        switch (edge)
        {
            case Edge.Top:
                x = Random.Range(topLeft.x, bottomRight.x);
                y = topLeft.y;
                break;
            case Edge.Right:
                x = bottomRight.x;
                y = Random.Range(bottomRight.y, topLeft.y);
                break;
            case Edge.Bottom:
                x = Random.Range(topLeft.x, bottomRight.x);
                y = bottomRight.y;
                break;
            case Edge.Left:
                x = topLeft.x;
                y = Random.Range(bottomRight.y, topLeft.y);
                break;
            default:
                x = 0;
                y = 0;
                break;
        }

     

        Vector3 viewPortPoint = new Vector3(x, y, 0f);
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(viewPortPoint);
        return worldPoint;
    }

}
