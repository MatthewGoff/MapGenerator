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

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle1024x1024");
            Backdrop = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            Backdrop.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -2;
            Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f);
        }

        public void Destroy()
        {
            GameObject.Destroy(Backdrop);
        }
    }
}