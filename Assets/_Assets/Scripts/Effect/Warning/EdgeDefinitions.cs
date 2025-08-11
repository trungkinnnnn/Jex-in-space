
using UnityEngine;

public enum Edge
{
    None,
    top,
    right,
    left,
    bottom,
}

public struct EdgeIntersection
{
    public Vector3 position;
    public Edge edge;

    public EdgeIntersection(Vector3 position, Edge edge)
    {
        this.position = position;
        this.edge = edge;
    }

}