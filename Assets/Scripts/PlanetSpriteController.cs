using UnityEngine;

public class PlanetSpriteController : MonoBehaviour
{
    private static readonly float MIN_FREQUENCY = 0.001f;
    private static readonly float MAX_FREQUENCY = 0.02f;

    public GameObject Background;

    private void Awake()
    {
        Texture2D backgroundTexture = new Texture2D(512, 512); //CreateBackgroundTexture();
        Rect backgroundRect = new Rect(0, 0, 512, 512);
        Background.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backgroundTexture, backgroundRect, new Vector2(0.5f, 0.5f), 512);
    }

    private Texture2D CreateBackgroundTexture()
    {
        Gradient colorGradient = CreateColorGradient();
        float noiseMultiplier = Random.Range(3.0f, 3.0f);
        float frequency1 = Random.Range(MIN_FREQUENCY, MAX_FREQUENCY);
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
