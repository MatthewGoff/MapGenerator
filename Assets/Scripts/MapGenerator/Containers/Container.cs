using System.Collections.Generic;
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
        public static readonly int MAX_ITERATIONS = 1000;
        public delegate void Callback();

        public Expanse[] Expanses { get; protected set; }
        public Galaxy[] Galaxies { get; protected set; }
        public Sector[] Sectors { get; protected set; }
        public SolarSystem[] SolarSystems { get; protected set; }
        public Star[] Stars { get; protected set; }
        public Planet[] Planets { get; protected set; }

        public readonly CelestialBodyType Type;
        public bool Initialized
        {
            get
            {
                bool tmp;
                lock (AccessToInitialized)
                {
                    tmp = m_Initialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToInitialized)
                {
                    m_Initialized = value;
                }
            }
        }

        protected Quadtree Quadtree;
        protected float SmallestContainerRadius = float.MaxValue;
        protected System.Random RNG;

        private readonly bool Root;
        private readonly object AccessToInitialized;
        private bool m_Initialized;

        public Container(CelestialBodyType type, float radius, int randomSeed, float quadtreeRadius, bool root, bool immovable = false) : base(radius, immovable)
        {
            AccessToInitialized = new object();
            Type = type;
            RNG = new System.Random(randomSeed);
            Quadtree = new Quadtree(this, quadtreeRadius, -quadtreeRadius, -quadtreeRadius, quadtreeRadius);
            Initialized = false;
            Root = root;
        }

        public virtual void Initialize(Callback callback = null)
        {

        }

        protected void AllocateExpanses(int number)
        {
            Expanses = new Expanse[number];
            for (int i = 0; i < Expanses.Length; i++)
            {
                Expanses[i] = new Expanse(RNG.Next(), false);
            }
        }

        protected void InitializeExpanses()
        {
            for (int i = 0; i < Expanses.Length; i++)
            {
                Expanses[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Expanses[i].Initialize();
                Quadtree.Insert(Expanses[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Expanses[i].Radius);
            }
        }

        protected void AllocateGalaxies(int number)
        {
            Galaxies = new Galaxy[number];
            for (int i = 0; i < Galaxies.Length; i++)
            {
                Galaxies[i] = new Galaxy(RNG.Next(), false);
            }
        }

        protected void InitializeGalaxies()
        {
            for (int i = 0; i < Galaxies.Length; i++)
            {
                Galaxies[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Galaxies[i].Initialize();
                Quadtree.Insert(Galaxies[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Galaxies[i].Radius);
            }
        }

        protected void AllocateSectors(int number)
        {
            Sectors = new Sector[number];
            for (int i = 0; i < Sectors.Length; i++)
            {
                Sectors[i] = new Sector(RNG.Next(), false);
            }
        }

        protected void InitializeSectors()
        {
            for (int i = 0; i < Sectors.Length; i++)
            {
                Sectors[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Sectors[i].Initialize();
                Quadtree.Insert(Sectors[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Sectors[i].Radius);
            }
        }

        protected void AllocateSolarSystems(int number)
        {
            SolarSystems = new SolarSystem[number];
            for (int i = 0; i < SolarSystems.Length; i++)
            {
                SolarSystems[i] = new SolarSystem(RNG.Next(), false);
            }
        }

        protected void InitializeSolarSystems()
        {
            for (int i = 0; i < SolarSystems.Length; i++)
            {
                SolarSystems[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                SolarSystems[i].Initialize();
                Quadtree.Insert(SolarSystems[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, SolarSystems[i].Radius);
            }
        }

        protected void AllocateStars(int number)
        {
            Stars = new Star[number];
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i] = new Star(RNG.Next(), false, false);
            }
        }

        protected void InitializeStars()
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                ProgressTracker.Instance.StarsInitialized++;
                Stars[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Quadtree.Insert(Stars[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Stars[i].Radius);
            }
        }

        protected void AllocatePlanets(int number)
        {
            Planets = new Planet[number];
            for (int i = 0; i < Planets.Length; i++)
            {
                Planets[i] = new Planet(RNG.Next(), false);
            }
        }

        protected void InitializePlanets()
        {
            for (int i = 0; i < Planets.Length; i++)
            {
                ProgressTracker.Instance.PlanetsInitialized++;
                Planets[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Quadtree.Insert(Planets[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Planets[i].Radius);
            }
        }

        public int Distribute(bool growRadius, bool bounded, bool updateProgressTracker = false, int depth = 0)
        {
            float radiusIncrement = SmallestContainerRadius / 10f;

            float maxOverlap;
            int iterations = 0;
            do
            {
                if (updateProgressTracker)
                {
                    ProgressTracker.Instance.PushActivity("Iteration " + (iterations + depth * MAX_ITERATIONS).ToString());
                }
                if (growRadius)
                {
                    Radius += radiusIncrement;
                }
                maxOverlap = Quadtree.ResolveCollisions(bounded);
                iterations++;
                if (updateProgressTracker)
                {
                    ProgressTracker.Instance.PopActivity();
                }
            } while (maxOverlap > Quadtree.COLLISION_MARGIN && iterations < MAX_ITERATIONS);
            if (iterations >= MAX_ITERATIONS)
            {
                Radius += radiusIncrement * (depth + 1);
                return iterations + Distribute(growRadius, bounded, updateProgressTracker, depth + 1);
            }
            else
            {
                return iterations;
            }
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
            if (Root)
            {
                CalculateGlobalPositions(new Vector2(0, 0));
            }
            Initialized = true;
        }

        public void CalculateGlobalPositions(Vector2 parentPosition)
        {
            GlobalPosition = parentPosition + LocalPosition;
            
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    Expanses[i].CalculateGlobalPositions(GlobalPosition);
                }
            }
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