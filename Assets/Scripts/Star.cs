using UnityEngine;

public class Star : Container
{
    public static readonly float MIN_RADIUS = 1f;
    public static readonly float MAX_RADIUS = 4f;

    private GameObject GameObject;

    public Star(Vector2 position, bool immovable)
    {
        Radius = Random.Range(MIN_RADIUS, MAX_RADIUS);
        LocalPosition = position;
        Immovable = immovable;
    }

    public void Realize(Vector2 parentPosition)
    {
        GameObject = new GameObject("Star");
        GameObject.transform.position = parentPosition + LocalPosition;
        GameObject.transform.localScale = new Vector3(Radius * 2, Radius * 2, 1f);

        Texture2D texture = ChooseTexture();
        Rect rect = new Rect(0, 0, 1024, 1024);
        GameObject.AddComponent<SpriteRenderer>();
        GameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 1024);
    }

    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }

    private Texture2D ChooseTexture()
    {
        if (GameManager.Instance.PlainRendering)
        {
            return (Texture2D)Resources.Load("Sprites/WhiteCircle");
        }
        else
        {
            return CreateRandomTexture();
        }
    }

    private Texture2D CreateRandomTexture()
    {
        Texture2D background = (Texture2D)Resources.Load("Sprites/StarBackground");
        Texture2D halo = (Texture2D)Resources.Load("Sprites/StarHalo");

        halo = ColorTexture(halo, GameManager.Instance.StarGradient.Evaluate(Random.Range(0.0f, 1.0f)));
        return Helpers.ComposeTextures(halo, background);
    }

    private Texture2D ColorTexture(Texture2D texture, Color color)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                color.a = texture.GetPixel(x, y).a;
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }
}
