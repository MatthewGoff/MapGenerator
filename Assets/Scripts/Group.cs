using System.Collections.Generic;
using UnityEngine;

public class Group : IGroupable
{
    private readonly int Order;
    private float MemberDiameter
    {
        get
        {
            return Mathf.Pow(GameManager.SIZE, Order - 1);
        }
    }
    private float Diameter
    {
        get
        {
            return MemberDiameter * GameManager.SIZE;
        }
    }

    private Vector2 Position;
    private IGroupable Capital;
    private List<IGroupable> Members;
    private GameObject Backdrop;

    public Group(int order, Vector2 position)
    {
        Order = order;
        Position = position;
    }

    public void Populate()
    {
        Backdrop = GameObject.Instantiate(GameManager.Instance.Node, Position, Quaternion.identity);
        Backdrop.transform.localScale = new Vector3(Diameter, Diameter, 1f);
        Backdrop.GetComponent<SpriteRenderer>().sortingOrder = -Order;
        if (Order % 2 == 1)
        {
            Backdrop.GetComponent<SpriteRenderer>().color = Color.red;
        }

        Members = new List<IGroupable>();
        Capital = CreateMember(Position);
        int population;
        if (Order == GameManager.HIGHEST_ORDER)
        {
            population = GameManager.MAX_CLUSTER_POPULATION;
        }
        else
        {
            population = GameManager.RandomInt(GameManager.MIN_CLUSTER_POPULATION, GameManager.MAX_CLUSTER_POPULATION - 1);
        }

        for (int i = 0; i < population; i++)
        {
            Vector2 localPosition = Random.insideUnitCircle * (Diameter / 2f - (GameManager.MARGIN * MemberDiameter));
            Members.Add(CreateMember(Position + localPosition));
        }

        Distribute();

        Capital.Populate();
        foreach (IGroupable member in Members)
        {
            member.Populate();
        }
    }

    private IGroupable CreateMember(Vector2 position)
    {
        if (Order == 1)
        {
            return new Planet(position);
        }
        else
        {
            return new Group(Order - 1, position);
        }
    }

    private void Distribute()
    {
        bool overlap = false;
        int iterations = 0;
        do
        {
            overlap = DistributeTick();
            iterations++;
        } while (overlap && iterations < GameManager.MAX_DISTRIBUTE_ITERATIONS);
        if (iterations == GameManager.MAX_DISTRIBUTE_ITERATIONS)
        {
            Members.RemoveAt(0);
            Distribute();
        }
    }

    private bool DistributeTick()
    {
        bool overlap = false;
        foreach (IGroupable member1 in Members)
        {
            overlap = overlap || CheckCollision(member1, Capital);
            overlap = overlap || CheckBoundry(member1);
            foreach (IGroupable member2 in Members)
            {
                overlap = overlap || CheckCollision(member1, member2);
            }
        }
        return overlap;
    }

    private bool CheckBoundry(IGroupable group)
    {
        Vector2 distance = Position - group.GetPosition();
        if (distance.magnitude > (Diameter / 2f) - (GameManager.MARGIN * MemberDiameter))
        {
            group.SetPosition(group.GetPosition() + distance.normalized * MemberDiameter / 20f);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckCollision(IGroupable group1, IGroupable group2)
    {
        if (group1 == group2)
        {
            return false;
        }

        Vector2 distance = group1.GetPosition() - group2.GetPosition();
        if (distance.magnitude < GameManager.MINIMUM_DISTANCE * MemberDiameter)
        {
            if (group1 != Capital)
            {
                group1.SetPosition(group1.GetPosition() + distance.normalized * MemberDiameter / 20f);
            }
            if (group2 != Capital)
            {
                group2.SetPosition(group2.GetPosition() - distance.normalized * MemberDiameter / 20f);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Destroy()
    {
        Capital.Destroy();
        foreach (IGroupable member in Members)
        {
            member.Destroy();
        }
        GameObject.Destroy(Backdrop);
    }

    public Vector2 GetPosition()
    {
        return Position;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}
