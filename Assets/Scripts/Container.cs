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
    private static readonly float OVERLAP_DISPLACEMENT = 4f / 4f;

    protected float SmallestContainerRadius = float.MaxValue;
    protected float MinimumPush;

    protected Sector[] Sectors;
    protected Cloud[] Clouds;
    protected SolarSystem[] SolarSystems;
    protected Star[] Stars;

    protected void CreateSectors(int number, List<CircleRigidBody> colliders)
    {
        Sectors = new Sector[number];
        for (int i = 0; i < Sectors.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Sectors[i] = new Sector(localPosition);
            colliders.Add(Sectors[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Sectors[i].Radius);
        }
    }

    protected void CreateClouds(int number, List<CircleRigidBody> colliders)
    {
        Clouds = new Cloud[number];
        for (int i = 0; i < Clouds.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Clouds[i] = new Cloud(localPosition);
            colliders.Add(Clouds[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Clouds[i].Radius);
        }
    }

    protected void CreateSolarSystems(int number, List<CircleRigidBody> colliders)
    {
        SolarSystems = new SolarSystem[number];
        for (int i = 0; i < SolarSystems.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            SolarSystems[i] = new SolarSystem(localPosition);
            colliders.Add(SolarSystems[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, SolarSystems[i].Radius);
        }
    }

    protected void CreateStars(int number, List<CircleRigidBody> colliders)
    {
        Stars = new Star[number];
        for (int i = 0; i < Stars.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Stars[i] = new Star(localPosition, false);
            colliders.Add(Stars[i]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Stars[i].Radius);
        }
    }

    protected void Distribute(List<CircleRigidBody> colliders, bool establishRadius, bool bounded)
    {
        MinimumPush = SmallestContainerRadius / 10f;

        float maxOverlap;
        int iterations = 0;
        do
        {
            if (establishRadius)
            {
                Radius += MinimumPush;
            }
            maxOverlap = DistributeTick(colliders, bounded);
            iterations++;
        } while (maxOverlap > MARGIN && iterations < 1000);
        if (GameManager.Instance.Log)
        {
            Debug.Log(iterations);
        }
    }

    private float DistributeTick(List<CircleRigidBody> colliders, bool bounded)
    {
        float maxOverlap = 0f;
        foreach (CircleRigidBody collider1 in colliders)
        {
            if (bounded)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckBoundry(collider1));
            }
            foreach (CircleRigidBody collider2 in colliders)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckCollision(collider1, collider2));
            }
        }
        return maxOverlap;
    }

    private float CheckCollision(CircleRigidBody collider1, CircleRigidBody collider2)
    {
        if (collider1 == collider2)
        {
            return 0;
        }

        Vector2 distance = collider1.LocalPosition - collider2.LocalPosition;
        float minDistance = collider1.Radius + collider2.Radius + MARGIN;
        if (distance.magnitude < minDistance)
        {
            float overlap = minDistance - distance.magnitude;
            float pushDistance = OVERLAP_DISPLACEMENT * overlap;// + MinimumPush;
            float totalMass = collider1.Mass + collider2.Mass;
            float collider1Contribution = collider2.Mass / totalMass;
            float collider2Contribution = collider1.Mass / totalMass;
            collider1.Push(distance.normalized * collider1Contribution * pushDistance);
            collider2.Push(-distance.normalized * collider2Contribution * pushDistance);
            return overlap;
        }
        else
        {
            return 0;
        }
    }

    private float CheckBoundry(CircleRigidBody collider1)
    {
        float distance = Radius - collider1.LocalPosition.magnitude;
        float minDistance = collider1.Radius + MARGIN;
        if (distance < minDistance)
        {
            float overlap = minDistance - distance;
            float pushDistance = OVERLAP_DISPLACEMENT * overlap;// + MinimumPush;
            collider1.Push(-collider1.LocalPosition.normalized * pushDistance);
            return Mathf.Abs(overlap);
        }
        else
        {
            return 0;
        }
    }

    protected void FinalizeRadius(List<CircleRigidBody> colliders)
    {
        float farthestExtent = 0f;
        foreach (CircleRigidBody collider in colliders)
        {
            farthestExtent = Mathf.Max(farthestExtent, collider.LocalPosition.magnitude + collider.Radius);
        }
        Radius = farthestExtent + MARGIN;
    }
}
