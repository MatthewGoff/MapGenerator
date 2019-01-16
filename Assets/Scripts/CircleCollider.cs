using UnityEngine;

public interface CircleCollider
{
    float GetRadius();
    Vector2 GetLocalPosition();

    void Push(Vector2 vector);
}
