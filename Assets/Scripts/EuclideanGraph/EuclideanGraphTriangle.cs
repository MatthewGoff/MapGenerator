using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuclideanGraphTriangle
{

    private EuclideanGraphNode[] Corners;
    
    public EuclideanGraphTriangle(EuclideanGraphNode corner1, EuclideanGraphNode corner2, EuclideanGraphNode corner3)
    {
        Corners = new EuclideanGraphNode[] { corner1, corner2, corner3 };
    }

    public EuclideanGraphTriangle(EuclideanGraphNode[] corners)
    {
        Corners = corners;
    }

    public bool InTriangle(IEuclidean point)
    {
        return false;
    }

    public EuclideanGraphTriangle[] Subdivide(IEuclidean point)
    {
        EuclideanGraphTriangle[] triangles = new EuclideanGraphTriangle[3];
        return triangles;
    }
}
