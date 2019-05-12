using UnityEngine;

namespace MapGenerator
{
    public abstract class CircleRigidBody : EuclideanGraph.IEuclidean
    {
        public Vector2 LocalPosition;
        public Vector2 GlobalPosition { get; protected set; }
        public float Radius;
        public readonly float Mass;
        public float Top
        {
            get
            {
                return LocalPosition.y + Radius;
            }
        }
        public float Bottom
        {
            get
            {
                return LocalPosition.y - Radius;
            }
        }
        public float Left
        {
            get
            {
                return LocalPosition.x - Radius;
            }
        }
        public float Right
        {
            get
            {
                return LocalPosition.x + Radius;
            }
        }

        public Quadtree MyQuad;

        protected readonly bool Immovable = false;

        public CircleRigidBody(float radius, bool immovable)
        {
            Radius = radius;
            Mass = Mathf.Pow(Radius, 2);
            Immovable = immovable;
        }

        public void Push(Vector2 vector)
        {
            if (!Immovable)
            {
                LocalPosition += vector;
            }
        }

        public Vector2 GetPosition()
        {
            return GlobalPosition;
        }
    }
}