using UnityEngine;

namespace Map
{
    public class StarRenderer
    {
        private static readonly int TEXTURE_SIZE = 64;

        private readonly CelestialBodies.Star Data;
        private GameObject GameObject;

        public StarRenderer(CelestialBodies.Star data)
        {
            Data = data;

            GameObject = new GameObject("Star");
            GameObject.transform.position = Data.Position;
            GameObject.transform.localScale = new Vector3(Data.Radius * 2, Data.Radius * 2, 1f);

            Texture2D texture = ChooseTexture();
            Rect rect = new Rect(0, 0, TEXTURE_SIZE, TEXTURE_SIZE);
            GameObject.AddComponent<SpriteRenderer>();
            GameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), TEXTURE_SIZE);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }

        private Texture2D ChooseTexture()
        {
            if (GameManager.Instance.PlainRendering)
            {
                return (Texture2D)Resources.Load("Sprites/WhiteCircle64x64");
            }
            else
            {
                return CreateRandomTexture();
            }
        }

        private Texture2D CreateRandomTexture()
        {
            Texture2D background = (Texture2D)Resources.Load("Sprites/StarBackground128x128");
            Texture2D halo = (Texture2D)Resources.Load("Sprites/StarHalo128x128");

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
}