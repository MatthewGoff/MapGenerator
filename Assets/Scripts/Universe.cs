using System.Collections.Generic;
using UnityEngine;

public class Universe : Container
{
    private GameObject Backdrop;

    public Universe(Vector2 localPosition) : base(localPosition, 600f)
    {
        List<Container> containers = new List<Container>();
        CreateStars(100000, containers);
        Distribute(containers, false, true);
    }

    public void Realize(Vector2 parentPosition)
    {
        CreateBackdrop(parentPosition + LocalPosition);
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].Realize(parentPosition + LocalPosition);
        }
    }

    private void CreateBackdrop(Vector2 position)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle");
        Backdrop = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Backdrop.transform.localScale = new Vector3(Radius * 2f, Radius * 2f, 1f);
        Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -4;
        Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
    }
}
