using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangSpawner
{
    private const float _min = 0.3f;
    private const float _max = 1.3f;

    private readonly Vector2 _topLeft = new Vector2(-_min, _max);
    private readonly Vector2 _bottomRight = new Vector2(_max, -_min);

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
                x = Random.Range(_topLeft.x, _bottomRight.x);
                y = _topLeft.y;
                break;
            case Edge.Right:
                x = _bottomRight.x;
                y = Random.Range(_bottomRight.y, _topLeft.y);
                break;
            case Edge.Bottom:
                x = Random.Range(_topLeft.x, _bottomRight.x);
                y = _bottomRight.y;
                break;
            case Edge.Left:
                x = _topLeft.x;
                y = Random.Range(_bottomRight.y, _topLeft.y);
                break;
        }

        Vector3 viewPortPoint = new Vector3(x, y, 0f);
        return Camera.main.ViewportToWorldPoint(viewPortPoint);
    }

}
