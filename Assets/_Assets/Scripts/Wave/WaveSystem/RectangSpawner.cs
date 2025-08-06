using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangSpawner
{
    private const float min = 0.3f;
    private const float max = 1.3f;

    private readonly Vector2 topLeft = new Vector2(-min, max);
    private readonly Vector2 bottomRight = new Vector2(max, -min);

    private enum Edge { Top, Right, Bottom, Left }

    private readonly List<Edge> clockWiseEdges = new List<Edge> { Edge.Top, Edge.Right, Edge.Bottom, Edge.Left };

    private readonly Edge start;

    public RectangSpawner()
    {
        start = (Edge)Random.Range(0, 4);
    }

    public Vector3 GetSpawnPoint(int index)
    {
        int edgeIndex = (clockWiseEdges.IndexOf(start) + index) % 4;
        Edge edge = clockWiseEdges[edgeIndex];

        float x = 0f, y = 0f;

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
        }

        Vector3 viewPortPoint = new Vector3(x, y, 0f);
        return Camera.main.ViewportToWorldPoint(viewPortPoint);
    }

}
