using UnityEngine;

public class Group : Container
{
    private static readonly int MIN_GALAXIES = 9;
    private static readonly int MAX_GALAXIES = 9;
    public static readonly float MAX_RADIUS = Galaxy.MAX_RADIUS * 4;

    private GameObject Backdrop;

    public Group(Vector2 localPosition) : base(localPosition, 1f, MAX_RADIUS)
    {
        CreateGalaxies(Random.Range(MIN_GALAXIES, MAX_GALAXIES + 1));
        Distribute(true, true);
        CreateSectors(Galaxies.Length * 2);
        Distribute(false, true);
        CreateClouds(Sectors.Length * 2);
        Distribute(false, true);
        CreateSolarSystems(Clouds.Length * 2);
        Distribute(false, true);
        CreateStars(SolarSystems.Length * 2);
        Distribute(false, true);
        FinalizeContainer();
    }

    public void Realize(Vector2 parentPosition)
    {
        CreateBackdrop(parentPosition + LocalPosition);
        for (int i = 0; i < Galaxies.Length; i++)
        {
            Galaxies[i].Realize(parentPosition + LocalPosition);
        }
        for (int i = 0; i < Sectors.Length; i++)
        {
            Sectors[i].Realize(parentPosition + LocalPosition);
        }
        for (int i = 0; i < Clouds.Length; i++)
        {
            Clouds[i].Realize(parentPosition + LocalPosition);
        }
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
        Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -5;
        Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);
    }

    public void Destroy()
    {
        GameObject.Destroy(Backdrop);
        for (int i = 0; i < Galaxies.Length; i++)
        {
            Galaxies[i].Destroy();
        }
        for (int i = 0; i < Sectors.Length; i++)
        {
            Sectors[i].Destroy();
        }
        for (int i = 0; i < Clouds.Length; i++)
        {
            Clouds[i].Destroy();
        }
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
