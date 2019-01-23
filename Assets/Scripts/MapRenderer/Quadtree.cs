using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specialized Quadtree only for rendering celestial bodies
/// </summary>
public class Quadtree
{    
    private readonly int MaximumContents;
    private List<CelestialBodies.CelestialBody> Contents;
    private CelestialBodyType LargestBody;

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
    private float Width
    {
        get
        {
            return Right - Left;
        }
    }
    private float Height
    {
        get
        {
            return Top - Bottom;
        }
    }

    private Quadtree QuadOne;
    private Quadtree QuadTwo;
    private Quadtree QuadThree;
    private Quadtree QuadFour;

    public Quadtree(float top, float bottom, float left, float right, int maximumContents = 5)
    {
        MaximumContents = maximumContents;
        Contents = new List<CelestialBodies.CelestialBody>();
        LargestBody = CelestialBodyType.Planet;

        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public void InsertAll(List<CelestialBodies.CelestialBody> bodies)
    {
        foreach (CelestialBodies.CelestialBody body in bodies)
        {
            Insert(body);
        }
    }

    public void Insert(CelestialBodies.CelestialBody body)
    {
        if (QuadOne == null)
        {
            if (Contents.Count >= MaximumContents)
            {
                Split();
                Insert(body);
            }
            else
            {
                FinalizeInsert(body);
            }
        }
        else if (QuadOne.Contains(body))
        {
            QuadOne.Insert(body);
        }
        else if (QuadTwo.Contains(body))
        {
            QuadTwo.Insert(body);
        }
        else if (QuadThree.Contains(body))
        {
            QuadThree.Insert(body);
        }
        else if (QuadFour.Contains(body))
        {
            QuadFour.Insert(body);
        }
        else
        {
            FinalizeInsert(body);
        }
    }

    private void FinalizeInsert(CelestialBodies.CelestialBody body)
    {
        LargestBody = (CelestialBodyType) Mathf.Max((int)LargestBody, (int)body.Type);
        Contents.Add(body);
    }

    private void Split()
    {
        QuadOne = new Quadtree(Top, VerticalMedian, HorizontalMedian, Right, MaximumContents);
        QuadTwo = new Quadtree(Top, VerticalMedian, Left, HorizontalMedian, MaximumContents);
        QuadThree = new Quadtree(VerticalMedian, Bottom, Left, HorizontalMedian, MaximumContents);
        QuadFour = new Quadtree(VerticalMedian, Bottom, HorizontalMedian, Right, MaximumContents);

        List<CelestialBodies.CelestialBody> toInstert = Contents;
        Contents = new List<CelestialBodies.CelestialBody>();
        foreach (CelestialBodies.CelestialBody body in toInstert)
        {
            Insert(body);
        }
    }

    private bool Contains(CelestialBodies.CelestialBody body)
    {
        float top = body.Position.y + body.Radius;
        float bottom = body.Position.y - body.Radius;
        float left = body.Position.x - body.Radius;
        float right = body.Position.x + body.Radius;
        return (top < Top && bottom > Bottom && left > Left && right < Right);
    }

    private bool Contains(Vector2 position)
    {
        return position.y < Top && position.y > Bottom && position.x > Left && position.x < Right;
    }

    /// <summary>
    /// Get all bodies who's rect overlaps with the provided worldRect (Cull).
    /// Disregards all bodies smaller than the provided minimum (LoD).
    /// </summary>
    /// <param name="worldRect"></param>
    /// <param name="minimumSize"></param>
    /// <returns></returns>
    public List<CelestialBodies.CelestialBody> GetLocalBodies(Rect worldRect, CelestialBodyType minimumSize)
    {
        List<CelestialBodies.CelestialBody> list = new List<CelestialBodies.CelestialBody>();

        Rect rect;
        foreach(CelestialBodies.CelestialBody celestialBody in Contents)
        {
            if (celestialBody.Type >= minimumSize)
            {
                rect = new Rect(celestialBody.Position.x - celestialBody.Radius,
                    celestialBody.Position.y - celestialBody.Radius,
                    celestialBody.Radius * 2,
                    celestialBody.Radius * 2);
                if (rect.Overlaps(worldRect))
                {
                    list.Add(celestialBody);
                }
            }
        }

        if (QuadOne != null)
        {
            rect = new Rect(QuadOne.Left, QuadOne.Bottom, QuadOne.Width, QuadOne.Height);
            if (rect.Overlaps(worldRect) && QuadOne.LargestBody >= minimumSize)
            {
                list.AddRange(QuadOne.GetLocalBodies(worldRect, minimumSize));
            }
            rect = new Rect(QuadTwo.Left, QuadTwo.Bottom, QuadTwo.Width, QuadTwo.Height);
            if (rect.Overlaps(worldRect) && QuadTwo.LargestBody >= minimumSize)
            {
                list.AddRange(QuadTwo.GetLocalBodies(worldRect, minimumSize));
            }
            rect = new Rect(QuadThree.Left, QuadThree.Bottom, QuadThree.Width, QuadThree.Height);
            if (rect.Overlaps(worldRect) && QuadThree.LargestBody >= minimumSize)
            {
                list.AddRange(QuadThree.GetLocalBodies(worldRect, minimumSize));
            }
            rect = new Rect(QuadFour.Left, QuadFour.Bottom, QuadFour.Width, QuadFour.Height);
            if (rect.Overlaps(worldRect) && QuadFour.LargestBody >= minimumSize)
            {
                list.AddRange(QuadFour.GetLocalBodies(worldRect, minimumSize));
            }
        }

        return list;
    }

    /// <summary>
    /// Get a list of points which are endpoints of the lines neccesary to draw
    /// this quadtree
    /// </summary>
    /// <returns></returns>
    public List<Vector2> GetLinePoints()
    {
        List<Vector2> returnList = new List<Vector2>();
        returnList.Add(new Vector2(HorizontalMedian, Top));
        returnList.Add(new Vector2(HorizontalMedian, Bottom));
        returnList.Add(new Vector2(Left, VerticalMedian));
        returnList.Add(new Vector2(Right, VerticalMedian));
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
