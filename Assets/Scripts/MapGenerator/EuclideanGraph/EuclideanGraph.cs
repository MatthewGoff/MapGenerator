using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.EuclideanGraph
{
    public class EuclideanGraph<T> where T : IEuclidean
    {
        private static readonly int MAXIMUM_NOISE_DISTANCE = 20;
        private static readonly AnimationCurve NOISE_PROBABILITY = new AnimationCurve
        (
            new Keyframe(2, 0.05f),
            new Keyframe(MAXIMUM_NOISE_DISTANCE, 0.5f)
        );

        private float Radius;
        private LinkedList<EuclideanGraphNode<T>> Nodes;
        private List<EuclideanGraphEdge<T>> Edges;

        public EuclideanGraph()
        {
            Radius = 0f;
            Nodes = new LinkedList<EuclideanGraphNode<T>>();
            Edges = new List<EuclideanGraphEdge<T>>();
        }

        public void AddNode(T data)
        {
            Radius = Mathf.Max(Radius, data.GetPosition().magnitude);
            Nodes.AddLast(new EuclideanGraphNode<T>(data));
        }

        private void AddEdge(EuclideanGraphNode<T> node1, EuclideanGraphNode<T> node2)
        {
            if (!ContainsEdge(node1, node2))
            {
                EuclideanGraphEdge<T> newEdge = new EuclideanGraphEdge<T>(node1, node2);
                Edges.Add(newEdge);
                node1.Edges.Add(newEdge);
                node2.Edges.Add(newEdge);
            }
        }

        private int PathLength(EuclideanGraphNode<T> node1, EuclideanGraphNode<T> node2, int maxIterations = int.MaxValue)
        {
            List<EuclideanGraphNode<T>> visited = new List<EuclideanGraphNode<T>>();
            List<EuclideanGraphNode<T>> toVisitNow = new List<EuclideanGraphNode<T>>();
            List<EuclideanGraphNode<T>> toVisitLater = new List<EuclideanGraphNode<T>>();

            toVisitNow.Add(node1);
            int iterations = 0;
            while (toVisitNow.Count > 0 && iterations < maxIterations)
            {
                foreach (EuclideanGraphNode<T> node in toVisitNow)
                {
                    if (node == node2)
                    {
                        return iterations;
                    }
                    else
                    {
                        foreach (EuclideanGraphNode<T> neighbor in node.GetNeighbors())
                        {
                            if (!visited.Contains(neighbor))
                            {
                                toVisitLater.Add(neighbor);
                            }
                        }
                    }
                }

                iterations++;
                visited.AddRange(toVisitNow);
                toVisitNow = toVisitLater;
                toVisitLater = new List<EuclideanGraphNode<T>>();
            }

            return -1;
        }

        public void AddNoise(int seed)
        {
            ProgressTracker.Instance.PushActivity("Generating noise");

            System.Random rng = new System.Random(seed);
            int counter = 1;
            foreach(EuclideanGraphEdge<T> edge in Edges)
            {
                if (!edge.Active)
                {
                    ProgressTracker.Instance.PushActivity("Adding noise " + (counter++) + "/" + (Edges.Count - Nodes.Count));
                    int distance = PathLength(edge.Nodes[0], edge.Nodes[1], MAXIMUM_NOISE_DISTANCE);
                    if (distance == -1)
                    {
                        distance = MAXIMUM_NOISE_DISTANCE;
                    }
                    float probability = NOISE_PROBABILITY.Evaluate(distance);
                    if (((float)rng.Next() / int.MaxValue) < probability)
                    {
                        edge.Active = true;
                    }
                    ProgressTracker.Instance.PopActivity();
                }
            }

            ProgressTracker.Instance.PopActivity();
        }

        public void GenerateMST()
        {
            ProgressTracker.Instance.PushActivity("Generating MST");
            Edges.Sort(EuclideanGraphEdge<T>.Compare);
            foreach (EuclideanGraphEdge<T> edge in Edges)
            {
                edge.Active = false;
            }

            int counter = 0;
            ProgressTracker.Instance.PushActivity("Inserting edge " + (counter++) + "/" + Nodes.Count);
            foreach (EuclideanGraphEdge<T> edge in Edges)
            {
                if (PathLength(edge.Nodes[0], edge.Nodes[1]) == -1)
                {
                    edge.Active = true;
                    ProgressTracker.Instance.PopActivity();
                    ProgressTracker.Instance.PushActivity("Inserting edge " + (counter++) + "/" + Nodes.Count);

                }
            }
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PopActivity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void GenerateDelaunayTriangulation()
        {
            ProgressTracker.Instance.PushActivity("Triangulating");

            EuclideanGraphNode<T> node = AttemptDelaunayTriangulation();
            int attempt = 2;
            while (node != null)
            {
                ProgressTracker.Instance.PushActivity("Attempt " + attempt++);
                Nodes.Remove(node);
                Nodes.AddFirst(node);
                node = AttemptDelaunayTriangulation();
                ProgressTracker.Instance.PopActivity();
            }

            ProgressTracker.Instance.PopActivity();
        }

        /// <summary>
        /// Attempt to generate the Delaunay triangulation for this graph.
        /// Returns null on success. Otherwise returns the node which could not
        /// be traingulated.
        /// </summary>
        /// <returns></returns>
        private EuclideanGraphNode<T> AttemptDelaunayTriangulation()
        {
            // Create convex hull which spans the data
            EuclideanGraphNode<T> anchor1 = new EuclideanGraphNode<T>(new Vector2(0f, 2f * Radius));
            EuclideanGraphNode<T> anchor2 = new EuclideanGraphNode<T>(new Vector2(Mathf.Sqrt(3f) * Radius, -Radius));
            EuclideanGraphNode<T> anchor3 = new EuclideanGraphNode<T>(new Vector2(-Mathf.Sqrt(3f) * Radius, -Radius));
            DelaunayTree.DelaunayTree<T> tree = new DelaunayTree.DelaunayTree<T>(new EuclideanGraphTriangle<T>(anchor1, anchor2, anchor3));

            int nodesInserted = 0;
            // Insert one node at a time, always updating the delaunay triangulation
            foreach (EuclideanGraphNode<T> node in Nodes)
            {
                ProgressTracker.Instance.PushActivity("Inserting node " + (nodesInserted++).ToString() + "/" + Nodes.Count.ToString());
                EuclideanGraphTriangle<T> entryTriangle = tree.GetTriangle(node.GetPosition());
                if (entryTriangle == null)
                {
                    ProgressTracker.Instance.PopActivity();
                    return node;
                }
                EuclideanGraphTriangle<T>[] subdivisions = entryTriangle.Subdivide(node);
                tree.Subdivide(entryTriangle, subdivisions);

                Queue<EuclideanGraphNode<T>[]> edgesToVerify = entryTriangle.GetEdges();
                while (edgesToVerify.Count > 0)
                {
                    VerifyNextEdge(tree, edgesToVerify);
                }
                ProgressTracker.Instance.PopActivity();
            }

            ProgressTracker.Instance.PushActivity("Gathering edges");
            // Transfer to edge data structure
            List<EuclideanGraphTriangle<T>> triangles = tree.GetCurrentTriangles();
            foreach (EuclideanGraphTriangle<T> triangle in triangles)
            {
                Queue<EuclideanGraphNode<T>[]> edges = triangle.GetEdges();
                foreach (EuclideanGraphNode<T>[] edge in edges)
                {
                    if (edge[0].HasData && edge[1].HasData)
                    {
                        AddEdge(edge[0], edge[1]);
                    }
                }
            }
            ProgressTracker.Instance.PopActivity();
            return null;
        }

        private void VerifyNextEdge(DelaunayTree.DelaunayTree<T> delaunayTree, Queue<EuclideanGraphNode<T>[]> edgesToVerify)
        {
            EuclideanGraphNode<T>[] edge = edgesToVerify.Dequeue();

            EuclideanGraphTriangle<T> triangle1 = null;
            EuclideanGraphTriangle<T> triangle2 = null;
            delaunayTree.GetTrianglesOnEdge(edge, ref triangle1, ref triangle2);

            //There is only one triangle on an edge between anchor points (which cannot be flipped)
            if (triangle2 == null)
            {
                return;
            }

            float alpha = triangle1.GetOppositeAngle(edge);
            float beta = triangle2.GetOppositeAngle(edge);

            // If the sum of opposite angles is > 180 we need to flip
            if (alpha + beta > 180f)
            {
                // Enqueue other edges which may need to be flipped as a result of this flip
                EuclideanGraphNode<T>[][] otherEdges = triangle1.GetOtherEdges(edge);
                edgesToVerify.Enqueue(otherEdges[0]);
                edgesToVerify.Enqueue(otherEdges[1]);
                otherEdges = triangle2.GetOtherEdges(edge);
                edgesToVerify.Enqueue(otherEdges[0]);
                edgesToVerify.Enqueue(otherEdges[1]);

                // Create two new triangles
                EuclideanGraphNode<T> corner1 = triangle1.GetOppositeCorner(edge);
                EuclideanGraphNode<T> corner2 = triangle2.GetOppositeCorner(edge);
                EuclideanGraphTriangle<T> newTriangle1 = new EuclideanGraphTriangle<T>(corner1, corner2, edge[0]);
                EuclideanGraphTriangle<T> newTriangle2 = new EuclideanGraphTriangle<T>(corner1, corner2, edge[1]);

                // Update the Delaunay Tree
                delaunayTree.Replace(triangle1, triangle2, newTriangle1, newTriangle2);
            }
        }

        private bool ContainsEdge(EuclideanGraphNode<T> node1, EuclideanGraphNode<T> node2)
        {
            foreach (EuclideanGraphEdge<T> edge in Edges)
            {
                if ((edge.Nodes[0] == node1 && edge.Nodes[1] == node2)
                    || (edge.Nodes[0] == node2 && edge.Nodes[1] == node1))
                {
                    return true;
                }
            }
            return false;
        }

        public List<T[]> GetEdges()
        {
            List<T[]> returnEdges = new List<T[]>();
            foreach (EuclideanGraphEdge<T> edge in Edges)
            {
                if (edge.Active)
                {
                    returnEdges.Add(new T[]
                    {
                    edge.Nodes[0].Data,
                    edge.Nodes[1].Data
                    });
                }
            }
            return returnEdges;
        }

        /// <summary>
        /// Save an image of the given Delaunay tree to the specified file
        /// location
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tree"></param>
        private void SaveSnapshot(string fileName, DelaunayTree.DelaunayTree<T> tree)
        {
            Texture2D texture = GetDebugImage(tree);
            byte[] bytes = texture.EncodeToPNG();
            string path = Application.dataPath + "/../GraphSnapshots";
            Directory.CreateDirectory(path);
            File.WriteAllBytes(path + "/" + fileName + ".png", bytes);
        }

        /// <summary>
        /// Get an image of a graph described by a Delaunay tree
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private Texture2D GetDebugImage(DelaunayTree.DelaunayTree<T> tree)
        {
            Texture2D texture = new Texture2D(1024, 1024);

            List<EuclideanGraphTriangle<T>> triangles = tree.GetCurrentTriangles();
            foreach (EuclideanGraphTriangle<T> triangle in triangles)
            {
                EuclideanGraphNode<T>[] corners = triangle.GetCorners();
                foreach (EuclideanGraphNode<T> corner in corners)
                {
                    Vector2 position = corner.Position * (1024 / (4 * Radius)) + new Vector2(1024 / 2, 1024 / 2);
                    Drawing.DrawCircle(texture, position, 10, Color.red);
                }

                Queue<EuclideanGraphNode<T>[]> edges = triangle.GetEdges();
                foreach (EuclideanGraphNode<T>[] edge in edges)
                {
                    Vector2 position1 = edge[0].Position * (1024 / (4 * Radius)) + new Vector2(1024 / 2, 1024 / 2);
                    Vector2 position2 = edge[1].Position * (1024 / (4 * Radius)) + new Vector2(1024 / 2, 1024 / 2);
                    Drawing.DrawLine(texture, position1, position2, Color.black);
                }
            }
            
            return texture;
        }
    }
}