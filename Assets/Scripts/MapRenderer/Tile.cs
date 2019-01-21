using UnityEngine;

public class Tile
{
    public static readonly int TEXTURE_SIZE = 4096;

    private readonly Vector2 Position;

    public Tile(Vector2 position)
    {
        Position = position;
    }

    public void Show()
    {
        GameObject sprite = (GameObject)Resources.Load("Prefabs/Square4096x4096");
        GameObject.Instantiate(sprite, Position, Quaternion.identity);
    }

    public void Hide()
    {

    }
}