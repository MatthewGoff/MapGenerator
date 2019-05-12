using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.EuclideanGraph.DelaunayTree
{
    public class DelaunayTreeNode<T> where T : IEuclidean
    {
        public readonly EuclideanGraphTriangle<T> Triangle;
        public readonly List<DelaunayTreeNode<T>> Children;
        public bool Old;

        public DelaunayTreeNode(EuclideanGraphTriangle<T> triangle)
        {
            Triangle = triangle;
            Children = new List<DelaunayTreeNode<T>>();
            Old = false;
        }

        public EuclideanGraphTriangle<T> GetTriangle(Vector2 point)
        {
            if (Old)
            {
                foreach (DelaunayTreeNode<T> child in Children)
                {
                    if (child.Triangle.Contains(point))
                    {
                        return child.GetTriangle(point);
                    }
                }

                // If a child is not found to contain the point then it is most
                // likely the case that the point lies on the line between two
                // children.

                return null;
                /*
                float bestMeasure = -float.MaxValue;
                DelaunayTreeNode<T> bestchild = null;
                foreach (DelaunayTreeNode<T> child in Children)
                {
                    float measure = child.Triangle.ContainsHeuristic(point);
                    if (measure > bestMeasure)
                    {
                        bestMeasure = measure;
                        bestchild = child;
                    }
                }
                return bestchild.Triangle;
                */
            }
            else
            {
                return Triangle;
            }
        }
    }
}