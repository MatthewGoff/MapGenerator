using UnityEngine;

/// <summary>
/// A collection of some utility functions for drawing primative shapes to
/// a Texture2D
/// </summary>
public static class Drawing
{
    public static void DrawCircle(Texture2D texture, Vector2 position, float radius, Color color)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Vector2 pixelPosition = new Vector2(x, y);
                float distance = (position - pixelPosition).magnitude;
                if (distance <= radius)
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }
        texture.Apply();
    }

    public static void DrawLine (Texture2D texture, Vector2 position1, Vector2 position2, Color color)
    {
        Vector2 step = (position1 - position2).normalized / 2;
        int numberOfSteps = Mathf.FloorToInt((position1 - position2).magnitude / step.magnitude);
        for (int i = 0; i < numberOfSteps; i++)
        {
            Vector2 position = position2 + step * i;
            texture.SetPixel(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), color);
        }
        texture.Apply();
    }
}
