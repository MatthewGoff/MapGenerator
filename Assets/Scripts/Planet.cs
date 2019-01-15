using UnityEngine;

public class Planet : IGroupable, IEuclidean
{
    public static float Diameter
    {
        get
        {
            return 1f;
        }
    }

    private Vector2 Position;
    private GameObject Sprite;

	public Planet(Vector2 position)
    {
        Position = position;
    }

    public void Populate()
    {
        Sprite = GameObject.Instantiate(GameManager.Instance.Node, Position, Quaternion.identity);
        Sprite.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void Destroy()
    {
        GameObject.Destroy(Sprite);
    }

    public Vector2 GetPosition()
    {
        return Position;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}
