using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.EuclideanGraph
{
    public class EuclideanGraphTriangle<T> where T : IEuclidean
    {
        private EuclideanGraphNode<T>[] Corners;

        public EuclideanGraphTriangle(EuclideanGraphNode<T> corner1, EuclideanGraphNode<T> corner2, EuclideanGraphNode<T> corner3)
        {
            Corners = new EuclideanGraphNode<T>[] { corner1, corner2, corner3 };
        }

        public EuclideanGraphTriangle(EuclideanGraphNode<T>[] corners)
        {
            Corners = corners;
        }

        /// <summary>
        /// Check whether this triangle contains the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            bool check1 = SameSide(point, Corners[0].GetPosition(), Corners[1].GetPosition(), Corners[2].GetPosition());
            bool check2 = SameSide(point, Corners[1].GetPosition(), Corners[2].GetPosition(), Corners[0].GetPosition());
            bool check3 = SameSide(point, Corners[2].GetPosition(), Corners[0].GetPosition(), Corners[1].GetPosition());
            return check1 && check2 && check3;
        }
        
        /// <summary>
        /// Returns a heuristic measure of whether the given point is inside of
        /// this triangle. The higher the measure, the more certain the point
        /// is in the triangle. Usefull when the Contains method produces an
        /// erroneous result on account of finite precision.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float ContainsHeuristic(Vector2 point)
        {
            float check1 = SameSideHeuristic(point, Corners[0].GetPosition(), Corners[1].GetPosition(), Corners[2].GetPosition());
            float check2 = SameSideHeuristic(point, Corners[1].GetPosition(), Corners[2].GetPosition(), Corners[0].GetPosition());
            float check3 = SameSideHeuristic(point, Corners[2].GetPosition(), Corners[0].GetPosition(), Corners[1].GetPosition());
            return Mathf.Min(check1, check2, check3);
        }

        /// <summary>
        /// Check whether two points lie on the same side of a line
        /// </summary>
        /// <param name="queryPoint1"></param>
        /// <param name="queryPoint2"></param>
        /// <param name="linePoint1"></param>
        /// <param name="linePoint2"></param>
        /// <returns></returns>
        private bool SameSide(Vector2 queryPoint1, Vector2 queryPoint2, Vector2 linePoint1, Vector2 linePoint2)
        {
            Vector3 crossProduct1 = Vector3.Cross(linePoint2 - linePoint1, queryPoint1 - linePoint1);
            Vector3 crossProduct2 = Vector3.Cross(linePoint2 - linePoint1, queryPoint2 - linePoint1);
            return Vector3.Dot(crossProduct1, crossProduct2) >= 0;
        }

        /// <summary>
        /// Returns a heuristic measure of two points being on the same side of
        /// a line. The higher the measure, the more certain the two points are
        /// on the same side of the line. Usefull when the SameSide method
        /// produces an erroneous result on account of finite precision.
        /// </summary>
        /// <param name="queryPoint1"></param>
        /// <param name="queryPoint2"></param>
        /// <param name="linePoint1"></param>
        /// <param name="linePoint2"></param>
        /// <returns></returns>
        private float SameSideHeuristic(Vector2 queryPoint1, Vector2 queryPoint2, Vector2 linePoint1, Vector2 linePoint2)
        {
            Vector3 crossProduct1 = Vector3.Cross((linePoint2 - linePoint1).normalized, (queryPoint1 - linePoint1).normalized);
            Vector3 crossProduct2 = Vector3.Cross((linePoint2 - linePoint1).normalized, (queryPoint2 - linePoint1).normalized);
            return Vector3.Dot(crossProduct1, crossProduct2);
        }

        public EuclideanGraphTriangle<T>[] Subdivide(EuclideanGraphNode<T> point)
        {
            return new EuclideanGraphTriangle<T>[]
            {
                new EuclideanGraphTriangle<T>(point, Corners[0], Corners[1]),
                new EuclideanGraphTriangle<T>(point, Corners[1], Corners[2]),
                new EuclideanGraphTriangle<T>(point, Corners[2], Corners[0])
            };
        }

        public Queue<EuclideanGraphNode<T>[]> GetEdges()
        {
            Queue<EuclideanGraphNode<T>[]> queue = new Queue<EuclideanGraphNode<T>[]>();
            queue.Enqueue(new EuclideanGraphNode<T>[] { Corners[0], Corners[1] });
            queue.Enqueue(new EuclideanGraphNode<T>[] { Corners[1], Corners[2] });
            queue.Enqueue(new EuclideanGraphNode<T>[] { Corners[2], Corners[0] });
            return queue;
        }

        public EuclideanGraphNode<T>[] GetCorners()
        {
            return Corners;
        }

        public bool HasEdge(EuclideanGraphNode<T>[] edge)
        {
            return (edge[0] == Corners[0] || edge[0] == Corners[1] || edge[0] == Corners[2])
                && (edge[1] == Corners[0] || edge[1] == Corners[1] || edge[1] == Corners[2]);
        }

        public float GetOppositeAngle(EuclideanGraphNode<T>[] edge)
        {
            EuclideanGraphNode<T> opposite = GetOppositeCorner(edge);
            return Vector2.Angle(edge[0].GetPosition() - opposite.GetPosition(), edge[1].GetPosition() - opposite.GetPosition());
        }

        public EuclideanGraphNode<T> GetOppositeCorner(EuclideanGraphNode<T>[] edge)
        {
            EuclideanGraphNode<T> opposite = null;
            foreach (EuclideanGraphNode<T> corner in Corners)
            {
                if (edge[0] != corner && edge[1] != corner)
                {
                    opposite = corner;
                }
            }
            return opposite;
        }

        public EuclideanGraphNode<T>[][] GetOtherEdges(EuclideanGraphNode<T>[] edge)
        {
            EuclideanGraphNode<T> opposite = GetOppositeCorner(edge);
            EuclideanGraphNode<T>[][] otherEdges = new EuclideanGraphNode<T>[2][];
            otherEdges[0] = new EuclideanGraphNode<T>[]
            {
                opposite,
                edge[0]
            };
            otherEdges[1] = new EuclideanGraphNode<T>[]
            {
                opposite,
                edge[1]
            };
            return otherEdges;
        }

        public override string ToString()
        {
            string returnString = "(";
            returnString += "<" + Corners[0].Position.x + "," + Corners[0].Position.y + ">,";
            returnString += "<" + Corners[1].Position.x + "," + Corners[1].Position.y + ">,";
            returnString += "<" + Corners[2].Position.x + "," + Corners[2].Position.y + ">)";
            return returnString;
        }
    }
}