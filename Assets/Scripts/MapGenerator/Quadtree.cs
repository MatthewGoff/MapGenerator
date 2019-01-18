using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    /// <summary>
    /// Specialized Quadtree only for resolving collisions in a Container.
    /// Principle behaviour is ResolveCollisions which executes one physics tick.
    /// </summary>
    public class Quadtree
    {
        public static readonly float COLLISION_MARGIN = 0.5f;

        private readonly int MaximumContents;
        private List<CircleRigidBody> Contents;
        private List<CircleRigidBody> ToRectify;

        private readonly CircleRigidBody Owner;
        private readonly bool NearBoundry;

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

        private readonly Quadtree Parent;
        private Quadtree QuadOne;
        private Quadtree QuadTwo;
        private Quadtree QuadThree;
        private Quadtree QuadFour;

        public Quadtree(CircleRigidBody owner, float top, float bottom, float left, float right, Quadtree parent = null, int maximumContents = 5)
        {
            MaximumContents = maximumContents;
            Contents = new List<CircleRigidBody>();
            ToRectify = new List<CircleRigidBody>();

            Owner = owner;
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;

            NearBoundry = FarthestCorner() >= Owner.Radius;
        }

        public void Insert(CircleRigidBody circle)
        {
            if (QuadOne == null)
            {
                if (Contents.Count >= MaximumContents)
                {
                    Split();
                    Insert(circle);
                }
                else
                {
                    Contents.Add(circle);
                    circle.MyQuad = this;
                }
            }
            else if (QuadOne.Contains(circle))
            {
                QuadOne.Insert(circle);
            }
            else if (QuadTwo.Contains(circle))
            {
                QuadTwo.Insert(circle);
            }
            else if (QuadThree.Contains(circle))
            {
                QuadThree.Insert(circle);
            }
            else if (QuadFour.Contains(circle))
            {
                QuadFour.Insert(circle);
            }
            else
            {
                Contents.Add(circle);
                circle.MyQuad = this;
            }
        }

        private void Split()
        {
            QuadOne = new Quadtree(Owner, Top, VerticalMedian, HorizontalMedian, Right, this, MaximumContents);
            QuadTwo = new Quadtree(Owner, Top, VerticalMedian, Left, HorizontalMedian, this, MaximumContents);
            QuadThree = new Quadtree(Owner, VerticalMedian, Bottom, Left, HorizontalMedian, this, MaximumContents);
            QuadFour = new Quadtree(Owner, VerticalMedian, Bottom, HorizontalMedian, Right, this, MaximumContents);

            List<CircleRigidBody> toInstert = Contents;
            Contents = new List<CircleRigidBody>();
            foreach (CircleRigidBody circle in toInstert)
            {
                Insert(circle);
            }
        }

        private bool Contains(CircleRigidBody circle)
        {
            return (circle.Top < Top
                && circle.Bottom > Bottom
                && circle.Left > Left
                && circle.Right < Right);
        }

        public float ResolveCollisions(bool bounded)
        {
            float maxOverlap = 0f;
            List<CircleRigidBody> allContents = GetAllContents();
            foreach (CircleRigidBody circle1 in Contents)
            {
                foreach (CircleRigidBody circle2 in allContents)
                {
                    maxOverlap = Mathf.Max(maxOverlap, CheckCollision(circle1, circle2));
                }
                if (NearBoundry && bounded)
                {
                    maxOverlap = Mathf.Max(maxOverlap, CheckBoundry(circle1));
                }
            }
            foreach (CircleRigidBody circle in ToRectify)
            {
                Rectify(circle);
            }
            ToRectify.Clear();
            if (QuadOne != null)
            {
                maxOverlap = Mathf.Max(maxOverlap, QuadOne.ResolveCollisions(bounded));
                maxOverlap = Mathf.Max(maxOverlap, QuadTwo.ResolveCollisions(bounded));
                maxOverlap = Mathf.Max(maxOverlap, QuadThree.ResolveCollisions(bounded));
                maxOverlap = Mathf.Max(maxOverlap, QuadFour.ResolveCollisions(bounded));
            }
            return maxOverlap;
        }

        public List<CircleRigidBody> GetAllContents()
        {
            List<CircleRigidBody> returnList = new List<CircleRigidBody>();
            returnList.AddRange(Contents);
            if (QuadOne != null)
            {
                returnList.AddRange(QuadOne.GetAllContents());
                returnList.AddRange(QuadTwo.GetAllContents());
                returnList.AddRange(QuadThree.GetAllContents());
                returnList.AddRange(QuadFour.GetAllContents());
            }
            return returnList;
        }

        private float CheckCollision(CircleRigidBody circle1, CircleRigidBody circle2)
        {
            if (circle1 == circle2)
            {
                return 0;
            }

            Vector2 relativePosition = circle1.LocalPosition - circle2.LocalPosition;
            float distance = relativePosition.magnitude;
            Vector2 direction = relativePosition / distance;
            float minDistance = circle1.Radius + circle2.Radius + COLLISION_MARGIN;
            if (distance < minDistance)
            {
                float overlap = minDistance - distance;
                float circle1Contribution = circle2.Mass / (circle1.Mass + circle2.Mass);
                float circle2Contribution = 1 - circle1Contribution;
                circle1.Push(direction * circle1Contribution * overlap);
                circle2.Push(-direction * circle2Contribution * overlap);
                ToRectify.Add(circle1);
                ToRectify.Add(circle2);
                return overlap;
            }
            else
            {
                return 0;
            }
        }

        private float CheckBoundry(CircleRigidBody circle)
        {
            float distance = Owner.Radius - circle.LocalPosition.magnitude;
            float minDistance = circle.Radius + COLLISION_MARGIN;
            if (distance < minDistance)
            {
                float overlap = minDistance - distance;
                float pushDistance = overlap;
                circle.Push(-circle.LocalPosition.normalized * pushDistance);
                ToRectify.Add(circle);
                return Mathf.Abs(overlap);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Check if a container is still within the same quad and if not move it
        /// to the appropriate quad.
        /// </summary>
        /// <param name="circle"></param>
        private void Rectify(CircleRigidBody circle)
        {
            if (!Contains(circle) && Parent != null)
            {
                Parent.Rectify(circle);
            }
            else if (circle.MyQuad != this)
            {
                circle.MyQuad.Remove(circle);
                Insert(circle);
            }
        }

        private void Remove(CircleRigidBody circle)
        {
            Contents.Remove(circle);
        }

        /// <summary>
        /// Get a list of points which are endpoints of the lines neccesary to draw
        /// this quadtree
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetLinePoints()
        {
            List<Vector2> returnList = new List<Vector2>();
            returnList.Add(new Vector2(Left, Top));
            returnList.Add(new Vector2(Right, Top));
            returnList.Add(new Vector2(Right, Top));
            returnList.Add(new Vector2(Right, Bottom));
            returnList.Add(new Vector2(Right, Bottom));
            returnList.Add(new Vector2(Left, Bottom));
            returnList.Add(new Vector2(Left, Bottom));
            returnList.Add(new Vector2(Left, Top));
            if (QuadOne != null)
            {
                returnList.AddRange(QuadOne.GetLinePoints());
                returnList.AddRange(QuadTwo.GetLinePoints());
                returnList.AddRange(QuadThree.GetLinePoints());
                returnList.AddRange(QuadFour.GetLinePoints());
            }
            return returnList;
        }

        /// <summary>
        /// The distance to the corner farthest from the center of the quadtree
        /// owner.
        /// </summary>
        /// <returns></returns>
        private float FarthestCorner()
        {
            return Mathf.Max(
                (new Vector2(Top, Left)).magnitude,
                (new Vector2(Top, Right)).magnitude,
                (new Vector2(Bottom, Left)).magnitude,
                (new Vector2(Bottom, Right)).magnitude
                );
        }
    }
}