using System.Collections.Generic;
using UnityEngine;

public abstract class Container : CircleCollider
{
    private static readonly float MARGIN = 0.5f;

    protected Vector2 LocalPosition;
    protected float Radius;

    protected bool BoundryConstricts;
    protected bool BoundryStatic;
    protected float SmallestColliderRadius = float.MaxValue;
    protected float MinimumPush;

    protected Sector[] Sectors;
    protected Cloud[] Clouds;
    protected SolarSystem[] SolarSystems;
    protected Star[] Stars;

    protected void CreateSectors(int number, List<CircleCollider> colliders)
    {
        Sectors = new Sector[number];
        for (int i = 0; i < Sectors.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Sectors[i] = new Sector(localPosition);
            colliders.Add(Sectors[i]);
            SmallestColliderRadius = Mathf.Min(SmallestColliderRadius, Sectors[i].GetRadius());
        }
    }

    protected void CreateClouds(int number, List<CircleCollider> colliders)
    {
        Clouds = new Cloud[number];
        for (int i = 0; i < Clouds.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Clouds[i] = new Cloud(localPosition);
            colliders.Add(Clouds[i]);
            SmallestColliderRadius = Mathf.Min(SmallestColliderRadius, Clouds[i].GetRadius());
        }
    }

    protected void CreateSolarSystems(int number, List<CircleCollider> colliders)
    {
        SolarSystems = new SolarSystem[number];
        for (int i = 0; i < SolarSystems.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            SolarSystems[i] = new SolarSystem(localPosition);
            colliders.Add(SolarSystems[i]);
            SmallestColliderRadius = Mathf.Min(SmallestColliderRadius, SolarSystems[i].GetRadius());
        }
    }

    protected void CreateStars(int number, List<CircleCollider> colliders)
    {
        Stars = new Star[number];
        for (int i = 0; i < Stars.Length; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * Radius;
            Stars[i] = new Star(localPosition, false);
            colliders.Add(Stars[i]);
            SmallestColliderRadius = Mathf.Min(SmallestColliderRadius, Stars[i].GetRadius());
        }
    }

    protected void Distribute(List<CircleCollider> colliders)
    {
        MinimumPush = SmallestColliderRadius / 10f;

        float maxOverlap;
        int iterations = 0;
        do
        {
            maxOverlap = DistributeTick(colliders);
            iterations++;
        } while (maxOverlap > MinimumPush / 2 && iterations < 10000);
        if (GameManager.Instance.Log)
        {
            Debug.Log(iterations);
        }
    }

    private float DistributeTick(List<CircleCollider> colliders)
    {
        if (!BoundryStatic)
        {
            Radius -= MinimumPush;
        }
        float maxOverlap = 0f;
        foreach (CircleCollider collider1 in colliders)
        {
            if (BoundryConstricts)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckBoundry(collider1));
            }
            foreach (CircleCollider collider2 in colliders)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckCollision(collider1, collider2));
            }
        }
        return maxOverlap;
    }

    private float CheckCollision(CircleCollider collider1, CircleCollider collider2)
    {
        if (collider1 == collider2)
        {
            return 0;
        }

        Vector2 distance = collider1.GetLocalPosition() - collider2.GetLocalPosition();
        float minDistance = collider1.GetRadius() + collider2.GetRadius() + MARGIN;
        if (distance.magnitude < minDistance)
        {
            float overlap = minDistance - distance.magnitude;
            float pushDistance = overlap / 2f + MinimumPush;
            float sumOfSquares = Mathf.Pow(collider1.GetRadius(), 2f) + Mathf.Pow(collider2.GetRadius(), 2f);
            float collider1Contribution = Mathf.Pow(collider1.GetRadius(), 2f) / sumOfSquares;
            float collider2Contribution = Mathf.Pow(collider2.GetRadius(), 2f) / sumOfSquares;
            collider1.Push(distance.normalized * collider1Contribution * pushDistance);
            collider2.Push(-distance.normalized * collider2Contribution * pushDistance);
            return overlap;
        }
        else
        {
            return 0;
        }
    }

    private float CheckBoundry(CircleCollider collider1)
    {
        Vector2 distance = (Radius - collider1.GetLocalPosition().magnitude) * collider1.GetLocalPosition().normalized;
        float minDistance = collider1.GetRadius() + MARGIN;
        if (distance.magnitude < minDistance)
        {
            float overlap = minDistance - distance.magnitude;
            float pushDistance = overlap / 2 + MinimumPush;
            collider1.Push(-collider1.GetLocalPosition().normalized * pushDistance);
            if (!BoundryStatic)
            {
                Radius += pushDistance / 2f;
            }
            return overlap;
        }
        else
        {
            return 0;
        }
    }

    protected void FinalizeRadius(List<CircleCollider> colliders)
    {
        float farthestExtent = 0f;
        foreach (CircleCollider collider in colliders)
        {
            farthestExtent = Mathf.Max(farthestExtent, collider.GetLocalPosition().magnitude + collider.GetRadius());
        }
        Radius = farthestExtent + MARGIN;
    }

    public float GetRadius()
    {
        return Radius;
    }

    public Vector2 GetLocalPosition()
    {
        return LocalPosition;
    }

    public void Push(Vector2 vector)
    {
        LocalPosition += vector;
    }
}
