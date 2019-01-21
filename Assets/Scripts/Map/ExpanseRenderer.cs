using UnityEngine;

namespace Map
{
    public class ExpanseRenderer
    {
        private readonly CelestialBodies.Expanse Data;
        private GameObject GameObject;

        public ExpanseRenderer(CelestialBodies.Expanse data)
        {
            Data = data;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle512x512");
            GameObject = GameObject.Instantiate(prefab, Data.Position, Quaternion.identity);
            GameObject.transform.localScale = new Vector3(Data.Radius * 2f, Data.Radius * 2f, 1f);
            GameObject.GetComponent<SpriteRenderer>().sortingOrder = -6;
            GameObject.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}