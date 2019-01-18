using UnityEngine;

namespace MapGenerator.Containers
{
    /// <summary>
    /// A specialized data structure which is also a circular rigid body. Meant to
    /// contain other Containers and be contained within one. Its principle
    /// behaviour is to distribute its contents in euclidean space such that they
    /// all fit within its radius without overlapping.
    /// </summary>
    public abstract class Container : CircleRigidBody
    {
        public static readonly float MAX_ITERATIONS = 1000;

        public Galaxy[] Galaxies { get; protected set; }
        public Sector[] Sectors { get; protected set; }
        public Cloud[] Clouds { get; protected set; }
        public SolarSystem[] SolarSystems { get; protected set; }
        public Star[] Stars { get; protected set; }
        public Planet[] Planets { get; protected set; }
        
        protected Quadtree Quadtree;
        protected float SmallestContainerRadius = float.MaxValue;

        public Container(Vector2 localPosition, float radius, float quadtreeRadius, bool immovable = false) : base(localPosition, radius, immovable)
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

        protected void CreatePlanets(int number)
        {
            Planets = new Planet[number];
            for (int i = 0; i < Planets.Length; i++)
            {
                Vector2 localPosition = Random.insideUnitCircle * Radius;
                Planets[i] = new Planet(localPosition);
                Quadtree.Insert(Planets[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Planets[i].Radius);
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
            } while (maxOverlap > Quadtree.COLLISION_MARGIN && iterations < MAX_ITERATIONS);
            if (iterations >= MAX_ITERATIONS)
            {
                Radius += radiusIncrement;
                Distribute(growRadius, bounded);
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
            Radius = farthestExtent + Quadtree.COLLISION_MARGIN;
        }

        public void CalculateGlobalPositions(Vector2 parentPosition)
        {
            GlobalPosition = parentPosition + LocalPosition;

            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    Galaxies[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    Sectors[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
            if (Clouds != null)
            {
                for (int i = 0; i < Clouds.Length; i++)
                {
                    Clouds[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
            if (SolarSystems != null)
            {
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    SolarSystems[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
            if (Stars != null)
            {
                for (int i = 0; i < Stars.Length; i++)
                {
                    Stars[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
            if (Planets != null)
            {
                for (int i = 0; i < Planets.Length; i++)
                {
                    Planets[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
        }
    }
}