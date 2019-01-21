using UnityEngine;

namespace Map
{
    public class GroupRenderer
    {
        private readonly CelestialBodies.Group Data;
        private GameObject GameObject;

        public GroupRenderer(CelestialBodies.Group data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            GameObject = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -5;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}