using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuclideanGraphNode
{
    public readonly IEuclidean Data;
    public readonly Vector2 Position;
    public readonly bool HasData;

    public EuclideanGraphNode(IEuclidean data)
    {
        Data = data;
        Position = Data.GetPosition();
        HasData = true;
    }

    public EuclideanGraphNode(Vector2 position)
    {
        Data = null;
        Position = position;
        HasData = false;
    }
}
