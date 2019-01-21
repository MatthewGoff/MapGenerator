using UnityEngine;

namespace Map
{
    public class SolarSystemRenderer
    {
        private static readonly float MIN_CAMERA_PPU = 0.5f;

        private readonly CelestialBodies.SolarSystem Data;
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

        public SolarSystemRenderer(CelestialBodies.SolarSystem data)
        {
            Data = data;
        }

        private void CreateGameObject()
        {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            GameObject = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);
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

        private bool Visible()
        {
            return GameManager.Instance.GetCameraRect().Overlaps(Rect)
                && GameManager.Instance.GetCameraResolution() > MIN_CAMERA_PPU;
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}