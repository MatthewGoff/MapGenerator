using System.Collections.Generic;
using UnityEngine;

public class MapRenderer
{
    private static readonly Color[] COLOR = new Color[]
    {
        new Color(1.0f, 1.0f, 1.0f),
        new Color(1.0f, 1.0f, 1.0f),
        new Color(0.6f, 0.6f, 0.6f),
        new Color(0.5f, 0.5f, 0.5f),
        new Color(0.4f, 0.4f, 0.4f),
        new Color(0.3f, 0.3f, 0.3f),
        new Color(0.2f, 0.2f, 0.2f),
        new Color(0.1f, 0.1f, 0.1f),
        new Color(0.0f, 0.0f, 0.0f)
    };

    private readonly Quadtree Quadtree;
    private readonly int ScreenWidth;
    private readonly int ScreenHeight;

    private readonly Texture2D Texture;
    private Color[] CurrentFrameColorArray;
    private Color[] LastFrameColorArray;
    private Rect LastFrameWorldRect;

    public MapRenderer(int screenWidth, int screenHeight, CelestialBodies.CelestialBody body)
    {
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;

        Texture = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        CurrentFrameColorArray = new Color[ScreenWidth * ScreenHeight];
        LastFrameColorArray = new Color[ScreenWidth * ScreenHeight];

        Quadtree = new Quadtree(body.Position.y + body.Radius,
            body.Position.y - body.Radius,
            body.Position.x - body.Radius,
            body.Position.x + body.Radius);
        Quadtree.InsertAll(body.GetAllContents());
        GameManager.Instance.Quadtree = Quadtree;
    }

    /// <summary>
    /// Get a (ScreenWidth by ScreenHeight) texture depicting the portion of
    /// the world within worldRect.
    /// </summary>
    /// <param name="worldRect"></param>
    /// <returns></returns>
    public Texture2D RenderMap(Rect worldRect)
    {
        if (Quadtree == null)
        {
            return new Texture2D(ScreenWidth, ScreenHeight);
        }

        List<CelestialBodies.CelestialBody> bodies = Quadtree.GetOverlappingBodies(worldRect);
        bodies.Sort((a, b) => (int)b.Type - (int)a.Type);

        if (worldRect.width == LastFrameWorldRect.width)
        {
            RecycleLastFrame(CurrentFrameColorArray, worldRect);
            Rect[] remaining = RectMinus(worldRect, LastFrameWorldRect);
            for (int i = 0; i < remaining.Length; i++)
            {
                PasteAll(CurrentFrameColorArray, bodies, worldRect, remaining[i]);
            }
        }
        else
        {
            PasteAll(CurrentFrameColorArray, bodies, worldRect, worldRect);
        }

        Texture.SetPixels(CurrentFrameColorArray);
        Texture.Apply();

        Color[] holder = CurrentFrameColorArray;
        CurrentFrameColorArray = LastFrameColorArray;
        LastFrameColorArray = holder;
        LastFrameWorldRect = worldRect;

        return Texture;
    }

    /// <summary>
    /// Fill the provided image with the relevant portion of the last generated
    /// image.
    /// </summary>
    /// <param name="colorArray"></param>
    /// <param name="worldRect"></param>
    private void RecycleLastFrame(Color[] colorArray, Rect worldRect)
    {
        Rect recycledRect = Helpers.RectOverlap(worldRect, LastFrameWorldRect);

        float UPP = worldRect.width / ScreenWidth;
        int worldRect_xOffset = Mathf.RoundToInt((recycledRect.x - worldRect.x) / UPP);
        int worldRect_yOffset = Mathf.RoundToInt((recycledRect.y - worldRect.y) / UPP);
        int lastRect_xOffset = Mathf.RoundToInt((recycledRect.x - LastFrameWorldRect.x) / UPP);
        int lastRect_yOffset = Mathf.RoundToInt((recycledRect.y - LastFrameWorldRect.y) / UPP);
        int pixelWidth = Mathf.RoundToInt(recycledRect.width / UPP);
        int pixelHeight = Mathf.RoundToInt(recycledRect.height / UPP);
        for (int x = 0; x < pixelWidth; x++)
        {
            for (int y = 0; y < pixelHeight; y++)
            {
                colorArray[(worldRect_xOffset + x) + (worldRect_yOffset + y) * ScreenWidth] = LastFrameColorArray[(lastRect_xOffset + x) + (lastRect_yOffset + y) * ScreenWidth];
            }
        }
    }

    private void PasteAll(Color[] colorArray, List<CelestialBodies.CelestialBody> bodies, Rect worldRect, Rect mask)
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            Paste(colorArray, bodies[i], worldRect, mask);
        }
    }

    /// <summary>
    /// Rasterize the body into the color array where the viewport of the
    /// color array is worldRect. The color array is a flattened 2D array.
    /// </summary>
    /// <param name="colorArray"></param>
    /// <param name="body"></param>
    /// <param name="worldRect"></param>
    private void Paste(Color[] colorArray, CelestialBodies.CelestialBody body, Rect worldRect, Rect mask)
    {
        Rect bodyRect = new Rect(body.Position.x - body.Radius,
                body.Position.y - body.Radius,
                body.Radius * 2,
                body.Radius * 2);
        Rect overlapRect = Helpers.RectOverlap(bodyRect, worldRect);
        overlapRect = Helpers.RectOverlap(overlapRect, mask);
        float UPP = worldRect.width / ScreenWidth;
        float xOffset = Mathf.Ceil((overlapRect.x - worldRect.x) / UPP) * UPP;
        float yOffset = Mathf.Ceil((overlapRect.y - worldRect.y) / UPP) * UPP;
        int xOffsetPixels = Mathf.RoundToInt(xOffset / UPP);
        int yOffsetPixels = Mathf.RoundToInt(yOffset / UPP);
        float pixelWidth = Mathf.RoundToInt((Mathf.Floor((overlapRect.x + overlapRect.width - worldRect.x) / UPP) * UPP - xOffset) / UPP);
        float pixelHeight = Mathf.RoundToInt((Mathf.Floor((overlapRect.y + overlapRect.height - worldRect.y) / UPP) * UPP - yOffset) / UPP);
        for (int x = 0; x < pixelWidth; x++)
        {
            for (int y = 0; y < pixelHeight; y++)
            {
                float relativeX = worldRect.x + xOffset + x * UPP - body.Position.x;
                float relativeY = worldRect.y + yOffset + y * UPP - body.Position.y;
                if (relativeX * relativeX + relativeY * relativeY < body.Radius * body.Radius)
                {
                    colorArray[(xOffsetPixels + x) + (yOffsetPixels + y) * ScreenWidth] = SampleCelestialBody(body, relativeX, relativeY);
                }
            }
        }
    }

    /// <summary>
    /// Get the visual color of a celestial body at the given point relative to
    /// the body's center. (Rasterization)
    /// </summary>
    /// <param name="body"></param>
    /// <param name="relativePosition"></param>
    /// <returns></returns>
    private Color SampleCelestialBody(CelestialBodies.CelestialBody body, float relativeX, float relativeY)
    {
        return COLOR[(int)body.Type];
    }

    /// <summary>
    /// Get the three remaining areas after subtracing rect2 from rect1.
    /// Assumes both rects are the same dimentions. Also adds a margin to the
    /// remaining areas making them slightly larger. This is to avoid missing
    /// pixels.
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    public Rect[] RectMinus(Rect rect1, Rect rect2)
    {
        Rect[] rects = new Rect[3];
        float Margin = 3 * rect1.width / ScreenWidth;
        float x;
        float y;
        float width;
        float height;
        if (rect2.yMin < rect1.yMin)
        {
            if (rect2.xMin < rect1.xMin)
            {
                x = rect1.xMin;
                y = rect2.yMax - Margin;
                width = rect2.xMax - rect1.xMin;
                height = rect1.yMax - y;
                rects[0] = new Rect(x, y, width, height);

                x = rect2.xMax - Margin;
                y = rect2.yMax - Margin;
                width = rect1.xMax - x;
                height = rect1.yMax - y;
                rects[1] = new Rect(x, y, width, height);
                
                x = rect2.xMax - Margin;
                y = rect1.yMin;
                width = rect1.xMax - x;
                height = rect2.yMax - rect1.yMin;
                rects[2] = new Rect(x, y, width, height);
            }
            else
            {
                x = rect2.xMin;
                y = rect2.yMax - Margin;
                width = rect1.xMax - rect2.xMin;
                height = rect1.yMax - y;
                rects[0] = new Rect(x, y, width, height);

                x = rect1.xMin;
                y = rect2.yMax - Margin;
                width = rect2.xMin - rect1.xMin + Margin;
                height = rect1.yMax - y;
                rects[1] = new Rect(x, y, width, height);

                x = rect1.xMin;
                y = rect1.yMin;
                width = rect2.xMin - rect1.xMin + Margin;
                height = rect2.yMax - rect1.yMin;
                rects[2] = new Rect(x, y, width, height);

            }
        }
        else
        {
            if (rect2.xMin < rect1.xMin)
            {
                x = rect2.xMax - Margin;
                y = rect2.yMin;
                width = rect1.xMax - x;
                height = rect1.yMax - rect2.yMin;
                rects[0] = new Rect(x, y, width, height);

                x = rect2.xMax - Margin;
                y = rect1.yMin;
                width = rect1.xMax - x;
                height = rect2.yMin - rect1.yMin + Margin;
                rects[1] = new Rect(x, y, width, height);

                x = rect1.xMin;
                y = rect1.yMin;
                width = rect2.xMax - rect1.xMin;
                height = rect2.yMin - rect1.yMin + Margin;
                rects[2] = new Rect(x, y, width, height);
            }
            else
            {
                x = rect2.xMin;
                y = rect1.yMin;
                width = rect1.xMax - rect1.xMin;
                height = rect2.yMin - rect1.yMin + Margin;
                rects[0] = new Rect(x, y, width, height);

                x = rect1.xMin;
                y = rect1.yMin;
                width = rect2.xMin - rect1.xMin + Margin;
                height = rect2.yMin - rect1.yMin + Margin;
                rects[1] = new Rect(x, y, width, height);

                x = rect1.xMin;
                y = rect2.yMin;
                width = rect2.xMin - rect1.xMin + Margin;
                height = rect1.yMax - rect1.yMin;
                rects[2] = new Rect(x, y, width, height);
            }
        }
        return rects;
    }

}