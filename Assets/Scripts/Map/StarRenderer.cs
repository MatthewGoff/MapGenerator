using UnityEngine;

namespace Map
{
    public class StarRenderer
    {
        private static readonly int TEXTURE_SIZE = 64;
        private static readonly float MIN_CAMERA_PPU = 2f;

        private readonly CelestialBodies.Star Data;
        private readonly Texture2D Texture;
        private GameObject GameObject;
        private Rect Rect
        {
            get
            {
                Vector2 size = new Vector2(Data.Radius * 2f, Data.Radius * 2f);
                Vector2 position = Data.Position - size / 2;
                return new Rect(position, size);
            }
        }

        public StarRenderer(CelestialBodies.Star data)
        {
            Data = data;
            Texture = ChooseTexture();
        }

        public void Update()
        {
            if (Visible() && GameObject == null)
            {
                CreateGameObject();
            }
            else if (!Visible() && GameObject != null)
            {
                GameObject.Destroy(GameObject);
            }
        }

        private void CreateGameObject()
        {
            GameObject = new GameObject("Star");
            GameObject.transform.position = Data.Position;
            GameObject.transform.localScale = new Vector3(Data.Radius * 2, Data.Radius * 2, 1f);

            Rect rect = new Rect(0, 0, TEXTURE_SIZE, TEXTURE_SIZE);
            GameObject.AddComponent<SpriteRenderer>();
            GameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Texture, rect, new Vector2(0.5f, 0.5f), TEXTURE_SIZE);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        private bool Visible()
        {
            return GameManager.Instance.GetCameraRect().Overlaps(Rect)
                && GameManager.Instance.GetCameraResolution() > MIN_CAMERA_PPU;
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