using System.Collections.Generic;
using UnityEngine;

namespace MapRendering_Shader
{
    /// <summary>
    /// Specialized Quadtree only for rendering celestial bodies
    /// </summary>
    public class Quadtree
    {
        private readonly int MaximumContents;
        private List<Vector4> Contents;

        private readonly float Top;
        private readonly float Bottom;
        private readonly float Left;
        private readonly float Right;
        private float HorizontalMedian
        {
            get
            {
                return (Left + Right) / 2f;
            }
        }
        private float VerticalMedian
        {
            get
            {
                return (Top + Bottom) / 2f;
            }
        }
        private float Width
        {
            get
            {
                return Right - Left;
            }
        }
        private float Height
        {
            get
            {
                return Top - Bottom;
            }
        }
        private Rect Rect;

        private Quadtree QuadOne;
        private Quadtree QuadTwo;
        private Quadtree QuadThree;
        private Quadtree QuadFour;

        public Quadtree(float top, float bottom, float left, float right, int maximumContents = 5)
        {
            MaximumContents = maximumContents;
            Contents = new List<Vector4>();

            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
            Rect = new Rect(Left, Bottom, Width, Height);
        }

        public void InsertAll(List<CelestialBodies.CelestialBody> bodies)
        {
            foreach (CelestialBodies.CelestialBody body in bodies)
            {
                Insert(body);
            }
        }

        public void Insert(CelestialBodies.CelestialBody body)
        {
            Insert(new Vector4(body.Position.x, body.Position.y, body.Radius, (int)body.Type));
        }

        public void Instert(CelestialBodyIdentifier[] ids)
        {

        }

        private void Insert(Vector4 body)
        {
            if (QuadOne == null)
            {
                if (Contents.Count >= MaximumContents)
                {
                    Split();
                    Insert(body);
                }
                else
                {
                    Contents.Add(body);
                }
            }
            else if (QuadOne.Contains(body))
            {
                QuadOne.Insert(body);
            }
            else if (QuadTwo.Contains(body))
            {
                QuadTwo.Insert(body);
            }
            else if (QuadThree.Contains(body))
            {
                QuadThree.Insert(body);
            }
            else if (QuadFour.Contains(body))
            {
                QuadFour.Insert(body);
            }
            else
            {
                Contents.Add(body);
            }
        }

        private void Split()
        {
            QuadOne = new Quadtree(Top, VerticalMedian, HorizontalMedian, Right, MaximumContents);
            QuadTwo = new Quadtree(Top, VerticalMedian, Left, HorizontalMedian, MaximumContents);
            QuadThree = new Quadtree(VerticalMedian, Bottom, Left, HorizontalMedian, MaximumContents);
            QuadFour = new Quadtree(VerticalMedian, Bottom, HorizontalMedian, Right, MaximumContents);

            List<Vector4> toInstert = Contents;
            Contents = new List<Vector4>();
            foreach (Vector4 body in toInstert)
            {
                Insert(body);
            }
        }

        private bool Contains(Vector4 body)
        {
            float top = body.y + body.z;
            float bottom = body.y - body.z;
            float left = body.x - body.z;
            float right = body.x + body.z;
            return (top < Top && bottom > Bottom && left > Left && right < Right);
        }

        /// <summary>
        /// Get all bodies who's rect overlaps with the provided worldRect (Cull).
        /// </summary>
        /// <param name="worldRect"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        public void GetLocalBodies(Rect worldRect, Vector4[] output, int offset, ref int index)
        {
            foreach (Vector4 celestialBody in Contents)
            {
                float top = celestialBody.y + celestialBody.z;
                float bottom = celestialBody.y - celestialBody.z;
                float left = celestialBody.x - celestialBody.z;
                float right = celestialBody.x + celestialBody.z;
                if (top > worldRect.yMin && bottom < worldRect.yMax && left < worldRect.xMax && right > worldRect.xMin)
                {
                    if (index < 100)
                    {
                        output[offset + index++] = celestialBody;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (QuadOne != null)
            {
                if (QuadOne.Rect.Overlaps(worldRect))
                {
                    QuadOne.GetLocalBodies(worldRect, output, offset, ref index);
                }
                if (QuadTwo.Rect.Overlaps(worldRect))
                {
                    QuadTwo.GetLocalBodies(worldRect, output, offset, ref index);
                }
                if (QuadThree.Rect.Overlaps(worldRect))
                {
                    QuadThree.GetLocalBodies(worldRect, output, offset, ref index);
                }
                if (QuadFour.Rect.Overlaps(worldRect))
                {
                    QuadFour.GetLocalBodies(worldRect, output, offset, ref index);
                }
            }
        }

        /// <summary>
        /// Get a list of points which are endpoints of the lines neccesary to draw
        /// this quadtree
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetLinePoints()
        {
            List<Vector2> returnList = new List<Vector2>();
            returnList.Add(new Vector2(HorizontalMedian, Top));
            returnList.Add(new Vector2(HorizontalMedian, Bottom));
            returnList.Add(new Vector2(Left, VerticalMedian));
            returnList.Add(new Vector2(Right, VerticalMedian));
            if (QuadOne != null)
            {
                returnList.AddRange(QuadOne.GetLinePoints());
                returnList.AddRange(QuadTwo.GetLinePoints());
                returnList.AddRange(QuadThree.GetLinePoints());
                returnList.AddRange(QuadFour.GetLinePoints());
            }
            return returnList;
        }
    }
}