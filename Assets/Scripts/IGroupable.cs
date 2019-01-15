using UnityEngine;

public interface IGroupable
{
    void Populate();
    void Destroy();
    Vector2 GetPosition();
    void SetPosition(Vector2 position);
}
