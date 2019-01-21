using UnityEngine;

namespace Map
{
    public class CloudRenderer
    {
        private readonly CelestialBodies.Cloud Data;
        private GameObject Backdrop;

        public CloudRenderer(CelestialBodies.Cloud data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            Backdrop = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            Backdrop.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -2;
            Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        public void Destroy()
        {
            GameObject.Destroy(Backdrop);
        }
    }
}