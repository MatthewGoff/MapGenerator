using System.Collections.Generic;
using UnityEngine;

public class Quadtree
{
    private readonly int MaximumContents;
    private List<Container> Contents;
    private float Margin;

    private float Top;
    private float Bottom;
    private float Left;
    private float Right;
    private float HorizontalMedian
    {
        get
        {
            return (Left + Right) / 2f;
        }
    }
    private float VerticalMedian
    {
        get
        {
            return (Top + Bottom) / 2f;
        }
    }

    private Quadtree QuadOne;
    private Quadtree QuadTwo;
    private Quadtree QuadThree;
    private Quadtree QuadFour;

    public Quadtree(int maximumContents, float margin, float top, float bottom, float left, float right)
    {
        MaximumContents = maximumContents;
        Contents = new List<Container>();
        Margin = margin;

        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public void Insert(Container container)
    {
        if (QuadOne == null)
        {
            if (Contents.Count >= MaximumContents)
            {
                Split();
                Insert(container);
            }
            else
            {
                Contents.Add(container);
            }
        }
        else if (QuadOne.Contains(container))
        {
            QuadOne.Insert(container);
        }
        else if (QuadTwo.Contains(container))
        {
            QuadTwo.Insert(container);
        }
        else if (QuadThree.Contains(container))
        {
            QuadThree.Insert(container);
        }
        else if (QuadFour.Contains(container))
        {
            QuadFour.Insert(container);
        }
        else
        {
            Contents.Add(container);
        }
    }

    private void Split()
    {
        QuadOne = new Quadtree(MaximumContents, Margin, Top, VerticalMedian, HorizontalMedian, Right);
        QuadTwo = new Quadtree(MaximumContents, Margin, Top, VerticalMedian, Left, HorizontalMedian);
        QuadThree = new Quadtree(MaximumContents, Margin, VerticalMedian, Bottom, Left, HorizontalMedian);
        QuadFour = new Quadtree(MaximumContents, Margin, VerticalMedian, Bottom, HorizontalMedian, Right);

        List<Container> toInstert = Contents;
        Contents = new List<Container>();
        foreach (Container container in toInstert)
        {
            Insert(container);
        }
    }

    private bool Contains(CircleRigidBody circle)
    {
        return (circle.Top < Top
            && circle.Bottom > Bottom
            && circle.Left > Left
            && circle.Right < Right);
    }

    public float ResolveCollisions()
    {
        float maxOverlap = 0f;
        List<Container> allContents = GetAllContents();
        foreach(Container container1 in Contents)
        {
            foreach (Container container2 in allContents)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckCollision(container1, container2));
            }
        }
        if (QuadOne != null)
        {
            maxOverlap = Mathf.Max(maxOverlap, QuadOne.ResolveCollisions());
            maxOverlap = Mathf.Max(maxOverlap, QuadTwo.ResolveCollisions());
            maxOverlap = Mathf.Max(maxOverlap, QuadThree.ResolveCollisions());
            maxOverlap = Mathf.Max(maxOverlap, QuadFour.ResolveCollisions());
        }
        return maxOverlap;
    }
    
    private List<Container> GetAllContents()
    {
        List<Container> returnList = new List<Container>();
        returnList.AddRange(Contents);
        if (QuadOne != null)
        {
            returnList.AddRange(QuadOne.GetAllContents());
            returnList.AddRange(QuadTwo.GetAllContents());
            returnList.AddRange(QuadThree.GetAllContents());
            returnList.AddRange(QuadFour.GetAllContents());
        }
        return returnList;
    }

    private float CheckCollision(Container container1, Container container2)
    {
        if (container1 == container2)
        {
            return 0;
        }

        Vector2 relativePosition = container1.LocalPosition - container2.LocalPosition;
        float distance = relativePosition.magnitude;
        Vector2 direction = relativePosition / distance;
        float minDistance = container1.Radius + container2.Radius + Margin;
        if (distance < minDistance)
        {
            float overlap = minDistance - distance;
            float container1Contribution = container2.Mass / (container1.Mass + container2.Mass);
            float container2Contribution = 1 - container1Contribution;
            container1.Push(direction * container1Contribution * overlap);
            container2.Push(-direction * container2Contribution * overlap);
            return overlap;
        }
        else
        {
            return 0;
        }
    }

    public List<Vector2> GetLinePoints()
    {
        List<Vector2> returnList = new List<Vector2>();
        returnList.Add(new Vector2(Left, Top));
        returnList.Add(new Vector2(Right, Top));
        returnList.Add(new Vector2(Right, Top));
        returnList.Add(new Vector2(Right, Bottom));
        returnList.Add(new Vector2(Right, Bottom));
        returnList.Add(new Vector2(Left, Bottom));
        returnList.Add(new Vector2(Left, Bottom));
        returnList.Add(new Vector2(Left, Top));
        if (QuadOne != null)
        {
            returnList.AddRange(QuadOne.GetLinePoints());
            returnList.AddRange(QuadTwo.GetLinePoints());
            returnList.AddRange(QuadThree.GetLinePoints());
            returnList.AddRange(QuadFour.GetLinePoints());
        }
        return returnList;
    }
}