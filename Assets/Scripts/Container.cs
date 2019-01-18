using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A specialized data structure which is also a circular rigid body. Meant to
/// contain other  Containers and be contained within one. Its principle
/// behaviour is to distribute its contents in euclidean space such that they
/// all fit within its radius.
/// </summary>
public abstract class Container : CircleRigidBody
{
    private static readonly float MARGIN = 0.5f;

    protected float SmallestContainerRadius = float.MaxValue;

    protected Sector[] Sectors;
    protected Cloud[] Clouds;
    protected SolarSystem[] SolarSystems;
    protected Star[] Stars;

    public Container(Vector2 localPosition, float radius) : base(localPosition, radius)
    {

    }

    protected void CreateSectors(int number, List<Container> containers)
    {
        Sectors = new Sector[number];
        for (int i = 0; i < Sectors.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Sectors[i] = new Sector(localPosition);
            containers.Add(Sectors[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Sectors[i].Radius);
        }
    }

    protected void CreateClouds(int number, List<Container> containers)
    {
        Clouds = new Cloud[number];
        for (int i = 0; i < Clouds.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Clouds[i] = new Cloud(localPosition);
            containers.Add(Clouds[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Clouds[i].Radius);
        }
    }

    protected void CreateSolarSystems(int number, List<Container> containers)
    {
        SolarSystems = new SolarSystem[number];
        for (int i = 0; i < SolarSystems.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            SolarSystems[i] = new SolarSystem(localPosition);
            containers.Add(SolarSystems[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, SolarSystems[i].Radius);
        }
    }

    protected void CreateStars(int number, List<Container> containers)
    {
        Stars = new Star[number];
        for (int i = 0; i < Stars.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Stars[i] = new Star(localPosition, false);
            containers.Add(Stars[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Stars[i].Radius);
        }
    }

    protected void Distribute(List<Container> containers, bool establishRadius, bool bounded)
    {
        float radiusIncrement = SmallestContainerRadius / 10f;

        float maxOverlap;
        int iterations = 0;
        do
        {
            if (establishRadius)
            {
                Radius += radiusIncrement;
            }
            maxOverlap = DistributeTick(containers, bounded);
            iterations++;
        } while (maxOverlap > MARGIN && iterations < 10000);
        if (GameManager.Instance.Log)
        {
            Debug.Log(iterations);
        }
    }

    private float DistributeTick(List<Container> containers, bool bounded)
    {
        Quadtree quadtree = new Quadtree(5, MARGIN, Radius * 2, -Radius * 2, -Radius * 2, Radius * 2);
        float maxOverlap = 0f;
        foreach (Container container in containers)
        {
            if (bounded)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckBoundry(container));
            }
            quadtree.Insert(container);
        }
        GameManager.Instance.LastQuadtree = quadtree;
        maxOverlap = Mathf.Max(maxOverlap, quadtree.ResolveCollisions());
        return maxOverlap;
    }

    private float CheckBoundry(Container container)
    {
        float distance = Radius - container.LocalPosition.magnitude;
        float minDistance = container.Radius + MARGIN;
        if (distance < minDistance)
        {
            float overlap = minDistance - distance;
            float pushDistance = overlap;
            container.Push(-container.LocalPosition.normalized * pushDistance);
            return Mathf.Abs(overlap);
        }
        else
        {
            return 0;
        }
    }

    protected void FinalizeRadius(List<Container> containers)
    {
        float farthestExtent = 0f;
        foreach (Container container in containers)
        {
            farthestExtent = Mathf.Max(farthestExtent, container.LocalPosition.magnitude + container.Radius);
        }
        Radius = farthestExtent + MARGIN;
    }
}
