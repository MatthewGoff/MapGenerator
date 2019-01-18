using UnityEngine;

public abstract class CircleRigidBody
{
    protected bool Immovable = false;

    public Vector2 LocalPosition;
    public float Radius;
    public float Mass { get; private set; }
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

    public CircleRigidBody(Vector2 localPosition, float radius)
    {
        LocalPosition = localPosition;
        Radius = radius;
        Mass = Mathf.Pow(Radius, 2);
    }

    public void Push(Vector2 vector)
    {
        if (!Immovable)
        {
            LocalPosition += vector;
        }
    }
}
