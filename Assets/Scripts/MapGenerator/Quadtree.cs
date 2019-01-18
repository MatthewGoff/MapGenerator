using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specialized Quadtree only for resolving collisions in a Container.
/// Principle behaviour is ResolveCollisions which executes one physics tick.
/// </summary>
public class Quadtree
{
    private readonly int MaximumContents;
    private List<Container> Contents;
    private List<Container> ToRectify;

    private readonly Container Owner;
    private readonly bool NearBoundry;

    private readonly float Top;
    private readonly float Bottom;
    private readonly float Left;
    private readonly float Right;
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

    private readonly Quadtree Parent;
    private Quadtree QuadOne;
    private Quadtree QuadTwo;
    private Quadtree QuadThree;
    private Quadtree QuadFour;

    public Quadtree(Container owner, float top, float bottom, float left, float right, Quadtree parent = null, int maximumContents = 5)
    {
        MaximumContents = maximumContents;
        Contents = new List<Container>();
        ToRectify = new List<Container>();

        Owner = owner;
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;

        NearBoundry = FarthestCorner() >= Owner.Radius;
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
                container.MyQuad = this;
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
            container.MyQuad = this;
        }
    }

    private void Split()
    {
        QuadOne = new Quadtree(Owner, Top, VerticalMedian, HorizontalMedian, Right, this, MaximumContents);
        QuadTwo = new Quadtree(Owner, Top, VerticalMedian, Left, HorizontalMedian, this, MaximumContents);
        QuadThree = new Quadtree(Owner, VerticalMedian, Bottom, Left, HorizontalMedian, this, MaximumContents);
        QuadFour = new Quadtree(Owner, VerticalMedian, Bottom, HorizontalMedian, Right, this, MaximumContents);

        List<Container> toInstert = Contents;
        Contents = new List<Container>();
        foreach (Container container in toInstert)
        {
            Insert(container);
        }
    }

    private bool Contains(Container container)
    {
        return (container.Top < Top
            && container.Bottom > Bottom
            && container.Left > Left
            && container.Right < Right);
    }

    public float ResolveCollisions(bool bounded)
    {
        float maxOverlap = 0f;
        List<Container> allContents = GetAllContents();
        foreach(Container container1 in Contents)
        {
            foreach (Container container2 in allContents)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckCollision(container1, container2));
            }
            if (NearBoundry && bounded)
            {
                maxOverlap = Mathf.Max(maxOverlap, CheckBoundry(container1));
            }
        }
        foreach (Container container in ToRectify)
        {
            Rectify(container);
        }
        ToRectify.Clear();
        if (QuadOne != null)
        {
            maxOverlap = Mathf.Max(maxOverlap, QuadOne.ResolveCollisions(bounded));
            maxOverlap = Mathf.Max(maxOverlap, QuadTwo.ResolveCollisions(bounded));
            maxOverlap = Mathf.Max(maxOverlap, QuadThree.ResolveCollisions(bounded));
            maxOverlap = Mathf.Max(maxOverlap, QuadFour.ResolveCollisions(bounded));
        }
        return maxOverlap;
    }
    
    public List<Container> GetAllContents()
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
        float minDistance = container1.Radius + container2.Radius + Container.COLLISION_MARGIN;
        if (distance < minDistance)
        {
            float overlap = minDistance - distance;
            float container1Contribution = container2.Mass / (container1.Mass + container2.Mass);
            float container2Contribution = 1 - container1Contribution;
            container1.Push(direction * container1Contribution * overlap);
            container2.Push(-direction * container2Contribution * overlap);
            ToRectify.Add(container1);
            ToRectify.Add(container2);
            return overlap;
        }
        else
        {
            return 0;
        }
    }

    private float CheckBoundry(Container container)
    {
        float distance = Owner.Radius - container.LocalPosition.magnitude;
        float minDistance = container.Radius + Container.COLLISION_MARGIN;
        if (distance < minDistance)
        {
            float overlap = minDistance - distance;
            float pushDistance = overlap;
            container.Push(-container.LocalPosition.normalized * pushDistance);
            ToRectify.Add(container);
            return Mathf.Abs(overlap);
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Check if a container is still within the same quad and if not move it
    /// to the appropriate quad.
    /// </summary>
    /// <param name="container"></param>
    private void Rectify(Container container)
    {
        if (!Contains(container) && Parent != null)
        {
            Parent.Rectify(container);
        }
        else if (container.MyQuad != this)
        {
            container.MyQuad.Remove(container);
            Insert(container);
        }
    }

    private void Remove(Container container)
    {
        Contents.Remove(container);
    }

    /// <summary>
    /// Get a list of points which are endpoints of the lines neccesary to draw
    /// this quadtree
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// The distance to the corner farthest from the center of the quadtree
    /// owner.
    /// </summary>
    /// <returns></returns>
    private float FarthestCorner()
    {
        return Mathf.Max(
            (new Vector2(Top, Left)).magnitude,
            (new Vector2(Top, Right)).magnitude,
            (new Vector2(Bottom, Left)).magnitude,
            (new Vector2(Bottom, Right)).magnitude
            );
    }
}