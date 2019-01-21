using UnityEngine;

namespace Map
{
    public class SectorRenderer
    {
        private readonly CelestialBodies.Sector Data;
        private GameObject GameObject;

        public SectorRenderer(CelestialBodies.Sector data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            GameObject = UnityEngine.GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -3;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}