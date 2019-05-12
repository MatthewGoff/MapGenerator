using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.EuclideanGraph.DelaunayTree
{
    public class DelaunayTree<T> where T : IEuclidean
    {
        private DelaunayTreeNode<T> Root;
        private readonly List<DelaunayTreeNode<T>> Nodes;

        public DelaunayTree(EuclideanGraphTriangle<T> root)
        {
            Root = new DelaunayTreeNode<T>(root);
            Nodes = new List<DelaunayTreeNode<T>>
            {
                Root
            };
        }

        public EuclideanGraphTriangle<T> GetTriangle(Vector2 point)
        {
            return Root.GetTriangle(point);
        }

        public void Subdivide(EuclideanGraphTriangle<T> parent, EuclideanGraphTriangle<T>[] children)
        {
            DelaunayTreeNode<T> parentNode = FindNode(parent);
            foreach (EuclideanGraphTriangle<T> child in children)
            {
                DelaunayTreeNode<T> newNode = new DelaunayTreeNode<T>(child);
                parentNode.Children.Add(newNode);
                Nodes.Add(newNode);
            }
            parentNode.Old = true;
        }

        public List<EuclideanGraphTriangle<T>> GetCurrentTriangles()
        {
            List<EuclideanGraphTriangle<T>> list = new List<EuclideanGraphTriangle<T>>();
            foreach (DelaunayTreeNode<T> node in Nodes)
            {
                if (!node.Old)
                {
                    list.Add(node.Triangle);
                }
            }
            return list;
        }

        public void GetTrianglesOnEdge(EuclideanGraphNode<T>[] edge, ref EuclideanGraphTriangle<T> triangle1, ref EuclideanGraphTriangle<T> triangle2)
        {
            bool foundFirst = false;
            foreach (DelaunayTreeNode<T> node in Nodes)
            {
                if (!node.Old && node.Triangle.HasEdge(edge))
                {
                    if (!foundFirst)
                    {
                        triangle1 = node.Triangle;
                        foundFirst = true;
                    }
                    else
                    {
                        triangle2 = node.Triangle;
                    }
                }
            }
        }

        private DelaunayTreeNode<T> FindNode(EuclideanGraphTriangle<T> triangle)
        {
            foreach(DelaunayTreeNode<T> node in Nodes)
            {
                if (node.Triangle == triangle)
                {
                    return node;
                }
            }
            return null;
        }

        public void Replace(
            EuclideanGraphTriangle<T> oldTriangle1,
            EuclideanGraphTriangle<T> oldTriangle2,
            EuclideanGraphTriangle<T> newTriangle1,
            EuclideanGraphTriangle<T> newTriangle2
            )
        {
            DelaunayTreeNode<T> oldNode1 = FindNode(oldTriangle1);
            oldNode1.Old = true;
            DelaunayTreeNode<T> oldNode2 = FindNode(oldTriangle2);
            oldNode2.Old = true;

            DelaunayTreeNode<T> newNode1 = new DelaunayTreeNode<T>(newTriangle1);
            DelaunayTreeNode<T> newNode2 = new DelaunayTreeNode<T>(newTriangle2);
            Nodes.Add(newNode1);
            Nodes.Add(newNode2);

            oldNode1.Children.Add(newNode1);
            oldNode1.Children.Add(newNode2);
            oldNode2.Children.Add(newNode1);
            oldNode2.Children.Add(newNode2);
        }
    }
}
