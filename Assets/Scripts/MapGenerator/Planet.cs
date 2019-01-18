using UnityEngine;

public class Planet : Container
{
    private static readonly float MIN_TEXTURE_FREQUENCY = 0.001f;
    private static readonly float MAX_TEXTURE_FREQUENCY = 0.02f;
    public static readonly float MIN_RADIUS = 0.5f;
    public static readonly float MAX_RADIUS = 2.0f;

    private GameObject GameObject;
    private int PPU;

	public Planet(Vector2 localPosition) : base(localPosition, Random.Range(0.5f, 2.0f), MAX_RADIUS)
    {

    }

    public void Realize(Vector2 parentPosition)
    {
        GameObject = new GameObject("Planet");
        GameObject.transform.position = parentPosition + LocalPosition;
        GameObject.transform.localScale = new Vector3(Radius * 2f, Radius * 2f, 1f);

        Texture2D texture = ChooseTexture();
        Rect rect = new Rect(0, 0, PPU, PPU);
        GameObject.AddComponent<SpriteRenderer>();
        GameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), PPU);
    }

    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }

    private Texture2D ChooseTexture()
    {
        if (GameManager.Instance.PlainRendering)
        {
            PPU = 64;
            return (Texture2D)Resources.Load("Sprites/WhiteCircle64x64");
        }
        else
        {
            PPU = 512;
            return CreateRandomTexture();
        }
    }

    private Texture2D CreateRandomTexture()
    {
        Texture2D background = CreateBackgroundTexture();
        Texture2D cover = (Texture2D)Resources.Load("Sprites/PlanetCover");
        Texture2D mask = (Texture2D)Resources.Load("Sprites/PlanetMask");

        Texture2D result = Helpers.ComposeTextures(cover, background);
        result = Helpers.ApplyMask(result, mask);
        return result;
    }

    private Texture2D CreateBackgroundTexture()
    {
        Gradient colorGradient = CreateColorGradient();
        float noiseMultiplier = Random.Range(3.0f, 3.0f);
        float frequency1 = Random.Range(MIN_TEXTURE_FREQUENCY, MAX_TEXTURE_FREQUENCY);
        float frequency2 = frequency1 * 3;
        float[,] frequencies = new float[,] { { 0.5f, 0.5f }, { frequency1, frequency2 } };

        Texture2D backgroundTexture = new Texture2D(512, 512);
        float offset = Random.Range(0f, 100000f);
        for (int x = 0; x < backgroundTexture.width; x++)
        {
            for (int y = 0; y < backgroundTexture.height; y++)
            {
                float noiseSample = Helpers.PerlinNoise(x, y, frequencies, offset);
                noiseSample = (noiseSample - 0.5f) * noiseMultiplier + 0.5f;
                noiseSample = Mathf.Clamp01(noiseSample);
                Color colorSample = colorGradient.Evaluate(noiseSample);
                backgroundTexture.SetPixel(x, y, colorSample);
            }
        }
        backgroundTexture.Apply();
        return backgroundTexture;
    }

    private Gradient CreateColorGradient()
    {
        Gradient colorGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        colorGradient.SetKeys(colorKeys, alphaKeys);
        return colorGradient;
    }
}
