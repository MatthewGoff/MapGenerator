using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A specialized data structure which is also a circular rigid body. Meant to
/// contain other Containers and be contained within one. Its principle
/// behaviour is to distribute its contents in euclidean space such that they
/// all fit within its radius without overlapping.
/// </summary>
public abstract class Container : CircleRigidBody
{
    public static readonly float COLLISION_MARGIN = 0.5f;
    public static readonly float MAX_ITERATIONS = 1000;

    protected float SmallestContainerRadius = float.MaxValue;

    protected Galaxy[] Galaxies;
    protected Sector[] Sectors;
    protected Cloud[] Clouds;
    protected SolarSystem[] SolarSystems;
    protected Star[] Stars;

    protected Quadtree Quadtree;
    public Quadtree MyQuad;

    public Container(Vector2 localPosition, float radius, float quadtreeRadius) : base(localPosition, radius)
    {
        Quadtree = new Quadtree(this, quadtreeRadius, -quadtreeRadius, -quadtreeRadius, quadtreeRadius);
    }

    protected void CreateGalaxies(int number)
    {
        Galaxies = new Galaxy[number];
        for (int i = 0; i < Galaxies.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Galaxies[i] = new Galaxy(localPosition);
            Quadtree.Insert(Galaxies[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Galaxies[i].Radius);
        }
    }

    protected void CreateSectors(int number)
    {
        Sectors = new Sector[number];
        for (int i = 0; i < Sectors.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Sectors[i] = new Sector(localPosition);
            Quadtree.Insert(Sectors[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Sectors[i].Radius);
        }
    }

    protected void CreateClouds(int number)
    {
        Clouds = new Cloud[number];
        for (int i = 0; i < Clouds.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Clouds[i] = new Cloud(localPosition);
            Quadtree.Insert(Clouds[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Clouds[i].Radius);
        }
    }

    protected void CreateSolarSystems(int number)
    {
        SolarSystems = new SolarSystem[number];
        for (int i = 0; i < SolarSystems.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            SolarSystems[i] = new SolarSystem(localPosition);
            Quadtree.Insert(SolarSystems[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, SolarSystems[i].Radius);
        }
    }

    protected void CreateStars(int number)
    {
        Stars = new Star[number];
        for (int i = 0; i < Stars.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Stars[i] = new Star(localPosition, false);
            Quadtree.Insert(Stars[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Stars[i].Radius);
        }
    }

    protected void Distribute(bool growRadius, bool bounded)
    {
        float radiusIncrement = SmallestContainerRadius / 10f;

        float maxOverlap;
        int iterations = 0;
        do
        {
            if (growRadius)
            {
                Radius += radiusIncrement;
            }
            maxOverlap = Quadtree.ResolveCollisions(bounded);
            iterations++;
        } while (maxOverlap > COLLISION_MARGIN && iterations < MAX_ITERATIONS);
        if (iterations >= MAX_ITERATIONS)
        {
            Radius += radiusIncrement;
            Distribute(growRadius, bounded);
        }
        if (GameManager.Instance.Log)
        {
            Debug.Log(iterations);
        }
        GameManager.Instance.LastQuadtree = Quadtree;
    }

    protected void FinalizeContainer()
    {
        float farthestExtent = 0f;
        foreach (Container container in Quadtree.GetAllContents())
        {
            farthestExtent = Mathf.Max(farthestExtent, container.LocalPosition.magnitude + container.Radius);
            container.MyQuad = null;
        }
        Quadtree = null;
        Radius = farthestExtent + COLLISION_MARGIN;
    }
}
