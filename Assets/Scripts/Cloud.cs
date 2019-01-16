using System.Collections.Generic;
using UnityEngine;

public class Cloud : Container
{
    private static readonly int MIN_SOLAR_SYSTEMS = 4;
    private static readonly int MAX_SOLAR_SYSTEMS = 12;
    private static readonly float INITIAL_RADIUS = 1;

    private GameObject Backdrop;

    public Cloud(Vector2 position)
    {
        LocalPosition = position;
        Radius = INITIAL_RADIUS;

        List<CircleCollider> colliders = new List<CircleCollider>();
        CreateSolarSystems(Random.Range(MIN_SOLAR_SYSTEMS, MAX_SOLAR_SYSTEMS + 1), colliders);

        BoundryConstricts = true;
        BoundryStatic = false;
        Distribute(colliders);

        CreateStars(SolarSystems.Length * 2, colliders);

        BoundryStatic = true;
        Distribute(colliders);

        FinalizeRadius(colliders);
    }

    public void Realize(Vector2 parentPosition)
    {
        CreateBackdrop(parentPosition + LocalPosition);
        for (int i = 0; i < SolarSystems.Length; i++)
        {
            SolarSystems[i].Realize(parentPosition + LocalPosition);
        }
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
        Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -2;
        Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f);
    }

    public void Destroy()
    {
        GameObject.Destroy(Backdrop);
        for (int i = 0; i < SolarSystems.Length; i++)
        {
            SolarSystems[i].Destroy();
        }
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].Destroy();
        }
    }
}
