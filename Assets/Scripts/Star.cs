using UnityEngine;

public class Star : CircleCollider
{
    public static readonly float MIN_RADIUS = 0.5f;
    public static readonly float MAX_RADIUS = 2.0f;
    public static float AVERAGE_AREA
    {
        get
        {
            float averageRadius = (MIN_RADIUS + MAX_RADIUS) / 2f;
            return Mathf.PI * Mathf.Pow(averageRadius, 2f);
        }
    }

    private float Radius;
    private Vector2 LocalPosition;
    private bool Immovable;
    private GameObject GameObject;

    public Star(Vector2 position, bool immovable)
    {
        Radius = Random.Range(0.5f, 2.0f);
        LocalPosition = position;
        Immovable = immovable;
    }

    public void Realize(Vector2 parentPosition)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Star");
        GameObject = GameObject.Instantiate(prefab, parentPosition + LocalPosition, Quaternion.identity);
        GameObject.transform.localScale = new Vector3(Radius * 2f, Radius * 2f, 1f);
    }

    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }

    public float GetRadius()
    {
        return 2f * Radius;
    }

    public Vector2 GetLocalPosition()
    {
        return LocalPosition;
    }

    public void Push(Vector2 vector)
    {
        if (!Immovable)
        {
            LocalPosition += vector;
        }
    }
}
