using UnityEngine;

namespace Map
{
    public class SolarSystemRenderer
    {
        private readonly CelestialBodies.SolarSystem Data;
        private GameObject GameObject;

        public SolarSystemRenderer(CelestialBodies.SolarSystem data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle1024x1024");
            GameObject = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}