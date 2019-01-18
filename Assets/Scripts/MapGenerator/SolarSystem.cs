using UnityEngine;

public class SolarSystem : Container
{
    private static readonly int MIN_PLANETS = 3;
    private static readonly int MAX_PLANETS = 9;
    public static readonly float MAX_RADIUS = Planet.MAX_RADIUS * 6;

    private Star Star;
    private Planet[] Planets;
    private GameObject Backdrop;

    public SolarSystem(Vector2 position) : base (position, 5f, MAX_RADIUS)
    {
        Star = new Star(new Vector2(0, 0), true);
        Quadtree.Insert(Star);
        CreatePlanets(Random.Range(MIN_PLANETS, MAX_PLANETS + 1));
        Distribute(false, false);
        FinalizeContainer();
    }

    private void CreatePlanets(int number)
    {
        Planets = new Planet[number];
        for (int i = 0; i < Planets.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Planets[i] = new Planet(localPosition);
            Quadtree.Insert(Planets[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Planets[i].Radius);
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
