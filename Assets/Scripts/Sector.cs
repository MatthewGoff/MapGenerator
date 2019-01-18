﻿using System.Collections.Generic;
using UnityEngine;

public class Sector : Container
{
    private static readonly int MIN_CLOUDS = 3;
    private static readonly int MAX_CLOUDS = 9;

    private GameObject Backdrop;

    public Sector(Vector2 localPosition) : base(localPosition, 1f)
    {
        List<Container> containers = new List<Container>();
        CreateClouds(Random.Range(MIN_CLOUDS, MAX_CLOUDS + 1), containers);

        Distribute(containers, true, true);

        CreateSolarSystems(Clouds.Length * 2, containers);

        Distribute(containers, false, true);

        CreateStars(SolarSystems.Length * 2, containers);

        Distribute(containers, false, true);

        FinalizeRadius(containers);
    }

    public void Realize(Vector2 parentPosition)
    {
        CreateBackdrop(parentPosition + LocalPosition);
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
        Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -3;
        Backdrop.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
    }

    public void Destroy()
    {
        GameObject.Destroy(Backdrop);
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
