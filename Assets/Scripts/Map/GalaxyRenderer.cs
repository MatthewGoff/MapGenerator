using UnityEngine;

namespace Map
{
    public class GalaxyRenderer
    {
        private readonly CelestialBodies.Galaxy Data;
        private GameObject GameObject;

        public GalaxyRenderer(CelestialBodies.Galaxy data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            GameObject = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -4;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}