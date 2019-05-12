using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.EuclideanGraph
{
    public class EuclideanGraphNode<T> where T : IEuclidean
    {
        public readonly bool HasData;
        public readonly T Data;
        public readonly Vector2 Position;
        public List<EuclideanGraphEdge<T>> Edges;

        public EuclideanGraphNode(T data)
        {
            Data = data;
            Position = Data.GetPosition();
            HasData = true;
            Edges = new List<EuclideanGraphEdge<T>>();
        }

        public EuclideanGraphNode(Vector2 position)
        {
            Position = position;
            HasData = false;
        }

        public List<EuclideanGraphNode<T>> GetNeighbors()
        {
            List<EuclideanGraphNode<T>> neighbors = new List<EuclideanGraphNode<T>>();
            foreach (EuclideanGraphEdge<T> edge in Edges)
            {
                if (edge.Active)
                {
                    if (this == edge.Nodes[0])
                    {
                        neighbors.Add(edge.Nodes[1]);
                    }
                    else
                    {
                        neighbors.Add(edge.Nodes[0]);
                    }
                }
            }
            return neighbors;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public override string ToString()
        {
            string returnString = "(<" + Position.x + "," + Position.y + ">)";
            return returnString;
        }
    }
}