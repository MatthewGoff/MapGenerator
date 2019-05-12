using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering_Shader
{
    public class FewLayerController : MonoBehaviour
    {
        public float StartAlpha;
        public float EndAlpha;
        public Color ExternalColor;
        public Color InternalColor;
        public GameObject MapCamera;
        public Texture2D Texture;
        public int TextureSize;
        public string ShaderName;
        public int SortingOrder;

        private GameObject[] Sprites;

        public void Initialize(List<CelestialBodies.CelestialBody> contents)
        {
            InitializeSprites(contents);
        }

        private void InitializeSprites(List<CelestialBodies.CelestialBody> contents)
        {
            Rect rect = new Rect(0, 0, TextureSize, TextureSize);

            Sprites = new GameObject[contents.Count];
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = new GameObject("Sprite " + i);
                Sprites[i].transform.SetParent(gameObject.transform);
                Sprites[i].transform.position = contents[i].Position;
                Sprites[i].transform.localScale = new Vector3(contents[i].Radius * 2, contents[i].Radius * 2, 0);
                Sprites[i].AddComponent<SpriteRenderer>();
                Sprites[i].GetComponent<SpriteRenderer>().sprite = Sprite.Create(Texture, rect, new Vector2(0.5f, 0.5f), TextureSize);
                Sprites[i].GetComponent<SpriteRenderer>().sortingOrder = SortingOrder;
                Material material = new Material(Shader.Find(ShaderName));
                material.SetColor("_ExternalColor", ExternalColor);
                material.SetColor("_InternalColor", InternalColor);
                Vector4 bodyVector = new Vector4(contents[i].Position.x, contents[i].Position.y, contents[i].Radius, 0);
                material.SetVector("body", bodyVector);
                Sprites[i].GetComponent<SpriteRenderer>().material = material;
            }
        }

        private void Update()
        {
            float pixelWidth = MapCamera.GetComponent<CameraController>().GetPixelWidth();
            float masterAlpha = Mathf.Lerp(1, 0, (MapCamera.GetComponent<Camera>().orthographicSize - StartAlpha) / (EndAlpha - StartAlpha));
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i].GetComponent<SpriteRenderer>().material.SetFloat("pixelWidth", pixelWidth);
                Sprites[i].GetComponent<SpriteRenderer>().material.SetFloat("masterAlpha", masterAlpha);
            }
        }
    }
}