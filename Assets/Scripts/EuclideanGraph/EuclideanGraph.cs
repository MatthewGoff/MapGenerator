using System.Collections.Generic;
using UnityEngine;

public class EuclideanGraph
{
    private List<EuclideanGraphNode> Nodes;
    private List<EuclideanGraphEdge> Edges;

    public EuclideanGraph(List<IEuclidean> data)
    {
        Nodes = new List<EuclideanGraphNode>();
        foreach (IEuclidean datum in data)
        {
            Nodes.Add(new EuclideanGraphNode(datum));
        }
        Edges = new List<EuclideanGraphEdge>();
    }

    public void GenerateDelaunayEdges(Vector2 Center, float radius)
    {
        EuclideanGraphNode node1 = new EuclideanGraphNode(Center + new Vector2(0f, 2f * radius));
        EuclideanGraphNode node2 = new EuclideanGraphNode(Center + new Vector2(Mathf.Sqrt(3f) * radius, -radius));
        EuclideanGraphNode node3 = new EuclideanGraphNode(Center + new Vector2(-Mathf.Sqrt(3f) * radius, -radius));

        List<EuclideanGraphTriangle> triangles = new List<EuclideanGraphTriangle>();
        triangles.Add(new EuclideanGraphTriangle(node1, node2, node3));

        foreach (EuclideanGraphNode node in Nodes)
        {
            AddNodeToDelaunayTriangles(node, triangles);
        }
    }

    private static void AddNodeToDelaunayTriangles(EuclideanGraphNode node, List<EuclideanGraphTriangle> triangles)
    {

    }

}
