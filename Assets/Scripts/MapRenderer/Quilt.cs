using UnityEngine;

public class Quilt
{
    private static readonly int PIXELS_PER_TEXTURE = 4096;

    private readonly int PPU;
    private float UPP
    {
        get
        {
            return 1f / PPU;
        }
    }
    private readonly Tile[,] Tiles;
	
    public Quilt(int ppu, float subjectRadius)
    {
        int textureWidth = PIXELS_PER_TEXTURE / PPU;
        int numTiles = 2 * Mathf.CeilToInt(subjectRadius / textureWidth);
        Tiles = new Tile[numTiles, numTiles];
        for (int i = 0; i < numTiles; i++)
        {
            int x = (i - numTiles / 2) * textureWidth;
            for (int j = 0; j < numTiles; j++)
            {
                int y = (j - numTiles / 2) * textureWidth;
                Tiles[i, j] = new Tile(new Vector2(x, y));
            }
        }
    }

    public void PasteObject(CelestialBodies.CelestialBody body)
    {
        float top = body.Position.y + body.Radius;
        top = Mathf.Ceil(top / UPP) * UPP;
        float bottom = body.Position.y - body.Radius;
        bottom = Mathf.Floor(bottom / UPP) * UPP;
        float left = body.Position.x - body.Radius;
        left = Mathf.Floor(left / UPP) * UPP;
        float right = body.Position.x + body.Radius;
        right = Mathf.Ceil(right / UPP) * UPP;
        
        for (float x = left; x <= right; x += UPP)
        {
            for (float y = bottom; y <= top; y += UPP)
            {

            }
        }
    }

    private void SetPixel(Vector2 Position, Color color)
    {
        int x = (int)Position.x;
        int y = (int)Position.y;

    }
}
