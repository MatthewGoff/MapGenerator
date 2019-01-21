using UnityEngine;

namespace Map
{
    public class UniverseRenderer
    {
        private readonly CelestialBodies.Universe Data;
        private GameObject GameObject;

        public UniverseRenderer(CelestialBodies.Universe data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            GameObject = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -7;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}