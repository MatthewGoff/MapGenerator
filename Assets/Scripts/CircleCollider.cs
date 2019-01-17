using UnityEngine;

public abstract class CircleRigidBody
{
    protected bool Immovable = false;

    public Vector2 LocalPosition;
    public float Radius;
    public float Mass
    {
        get
        {
            return Mathf.Pow(Radius, 2f);
        }
    }

    public void Push(Vector2 vector)
    {
        if (!Immovable)
        {
            LocalPosition += vector;
        }
    }
}
