using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : Container
{
    private static readonly int MIN_PLANETS = 4;
    private static readonly int MAX_PLANETS = 12;
    private static readonly float INITIAL_RADIUS = 1;

    private Star Star;
    private Planet[] Planets;
    private GameObject Backdrop;

	public SolarSystem(Vector2 position)
    {
        LocalPosition = position;
        Radius = INITIAL_RADIUS;

        List<CircleCollider> colliders = new List<CircleCollider>();
        Star = new Star(new Vector2(0, 0), true);
        colliders.Add(Star);

        CreatePlanets(Random.Range(MIN_PLANETS, MAX_PLANETS + 1), colliders);
        
        BoundryConstricts = false;
        BoundryStatic = true;
        Distribute(colliders);
        FinalizeRadius(colliders);
    }

    private void CreatePlanets(int number, List<CircleCollider> colliders)
    {
        Planets = new Planet[number];
        for (int i = 0; i < Planets.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Planets[i] = new Planet(localPosition);
            colliders.Add(Planets[i]);
            SmallestColliderRadius = Mathf.Min(SmallestColliderRadius, Planets[i].GetRadius());
        }
    }

    public void Realize(Vector2 parentPosition)
    {
        CreateBackdrop(parentPosition + LocalPosition);
        Star.Realize(parentPosition + LocalPosition);
        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].Realize(parentPosition + LocalPosition);
        }
        
    }

    private void CreateBackdrop(Vector2 position)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/WhiteCircle");
        Backdrop = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Backdrop.transform.localScale = new Vector3(Radius * 2f, Radius * 2f, 1f);
        Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -1;
        Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void Destroy()
    {
        GameObject.Destroy(Backdrop);
        Star.Destroy();
        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].Destroy();
        }
    }
}
