using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.EuclideanGraph
{
    public class EuclideanGraphEdge<T> where T : IEuclidean
    {
        public bool Active;
        public EuclideanGraphNode<T>[] Nodes;

        public EuclideanGraphEdge(EuclideanGraphNode<T> node1, EuclideanGraphNode<T> node2)
        {
            Active = true;
            Nodes = new EuclideanGraphNode<T>[] { node1, node2 };
        }

        public float GetLength()
        {
            return (Nodes[0].Position - Nodes[1].Position).magnitude;
        }

        public static int Compare(EuclideanGraphEdge<T> edge1, EuclideanGraphEdge<T> edge2)
        {
            float comparison = edge1.GetLength() - edge2.GetLength();
            if (comparison < 0)
            {
                return -1;
            }
            else if (comparison == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}