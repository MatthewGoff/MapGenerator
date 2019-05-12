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
        private static readonly int MAX_ITERATIONS = 1000;
        public delegate void Callback();

        public Expanse[] Expanses { get; protected set; }
        public Galaxy[] Galaxies { get; protected set; }
        public Sector[] Sectors { get; protected set; }
        public SolarSystem[] SolarSystems { get; protected set; }
        public Star[] Stars { get; protected set; }
        public Planet[] Planets { get; protected set; }

        public readonly CelestialBodyType Type;
        public readonly CelestialBodyIdentifier ID;
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
        private Callback ExpansesInitializedCallback;

        public Container(CelestialBodyType type, CelestialBodyIdentifier id, float radius, int randomSeed, float quadtreeRadius, bool root, bool immovable = false) : base(radius, immovable)
        {
            AccessToInitialized = new object();
            Type = type;
            ID = id;
            RNG = new System.Random(randomSeed);
            Quadtree = new Quadtree(this, quadtreeRadius, -quadtreeRadius, -quadtreeRadius, quadtreeRadius);
            Initialized = false;
            Root = root;
        }

        public virtual void Initialize()
        {

        }

        protected void AllocateExpanses(int number)
        {
            Expanses = new Expanse[number];
            CelestialBodyIdentifier newID;
            for (int i = 0; i < Expanses.Length; i++)
            {
                newID = new CelestialBodyIdentifier(ID, CelestialBodyType.Expanse, i);
                Expanses[i] = new Expanse(newID, RNG.Next(), false);
            }
        }

        protected void InitializeExpanses(Callback expansesInitializedCallback)
        {
            ExpansesInitializedCallback = expansesInitializedCallback;
            for (int i = 0; i < Expanses.Length; i++)
            {
                Expanses[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                InitializeExpanse thread = new InitializeExpanse(Expanses[i], OnExpanseInitialized);
                ThreadManager.Instance.Enqueue(thread);
            }
        }

        public void OnExpanseInitialized()
        {
            if (AllExpansesInitialized())
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    Quadtree.Insert(Expanses[i]);
                    SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Expanses[i].Radius);
                }
                ExpansesInitializedCallback();
            }
        }

        public bool AllExpansesInitialized()
        {
            bool allExpansesInitialized = true;
            for (int i = 0; i < Expanses.Length; i++)
            {
                allExpansesInitialized &= Expanses[i].Initialized;
            }
            return allExpansesInitialized;
        }

        protected void AllocateGalaxies(int number)
        {
            Galaxies = new Galaxy[number];
            CelestialBodyIdentifier newID;
            for (int i = 0; i < Galaxies.Length; i++)
            {
                newID = new CelestialBodyIdentifier(ID, CelestialBodyType.Galaxy, i);
                Galaxies[i] = new Galaxy(newID, RNG.Next(), false);
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
            CelestialBodyIdentifier newID;
            for (int i = 0; i < Sectors.Length; i++)
            {
                newID = new CelestialBodyIdentifier(ID, CelestialBodyType.Sector, i);
                Sectors[i] = new Sector(newID, RNG.Next(), false);
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
            CelestialBodyIdentifier newID;
            for (int i = 0; i < SolarSystems.Length; i++)
            {
                newID = new CelestialBodyIdentifier(ID, CelestialBodyType.SolarSystem, i);
                SolarSystems[i] = new SolarSystem(newID, RNG.Next(), false);
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
            CelestialBodyIdentifier newID;
            for (int i = 0; i < Stars.Length; i++)
            {
                newID = new CelestialBodyIdentifier(ID, CelestialBodyType.Star, i);
                Stars[i] = new Star(newID, RNG.Next(), false, false);
            }
        }

        protected void InitializeStars()
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                ProgressTracker.Instance.StarInitialized();
                Stars[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Quadtree.Insert(Stars[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Stars[i].Radius);
            }
        }

        protected void AllocatePlanets(int number)
        {
            Planets = new Planet[number];
            CelestialBodyIdentifier newID;
            for (int i = 0; i < Planets.Length; i++)
            {
                newID = new CelestialBodyIdentifier(ID, CelestialBodyType.Planet, i);
                Planets[i] = new Planet(newID, RNG.Next(), false);
            }
        }

        protected void InitializePlanets()
        {
            for (int i = 0; i < Planets.Length; i++)
            {
                ProgressTracker.Instance.PlanetInitialized();
                Planets[i].LocalPosition = Helpers.InsideUnitCircle(RNG) * Radius;
                Quadtree.Insert(Planets[i]);
                SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Planets[i].Radius);
            }
        }

        public int Distribute(bool growRadius, bool bounded)
        {
            return Distribute(growRadius, bounded, 0);
        }

        public int Distribute(bool growRadius, bool bounded, int depth)
        {
            float radiusIncrement = SmallestContainerRadius / 10f;

            float maxOverlap;
            int iterations = 0;
            do
            {
                StartActivity("Iteration " + (iterations + depth * MAX_ITERATIONS).ToString());
                if (growRadius)
                {
                    Radius += radiusIncrement;
                }
                maxOverlap = Quadtree.ResolveCollisions(bounded);
                iterations++;
                EndActivity();
            } while (Mathf.Max(maxOverlap) > Quadtree.COLLISION_MARGIN && iterations < MAX_ITERATIONS);
            if (iterations >= MAX_ITERATIONS)
            {
                Radius += radiusIncrement;
                return iterations + Distribute(growRadius, bounded, depth + 1);
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

        protected void StartActivity(string activityDescription)
        {
            if (Root)
            {
                ProgressTracker.Instance.PushActivity(activityDescription);
            }
        }
        
        protected void EndActivity()
        {
            if (Root)
            {
                ProgressTracker.Instance.PopActivity();
            }
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

        public Util.LinkedList<Container> GetAllSmallBodies()
        {
            Util.LinkedList<Container> bodies = new Util.LinkedList<Container>();
            if (Expanses != null)
            {
                foreach (Expanse expanse in Expanses)
                {
                    bodies.Append(expanse.GetAllSmallBodies());
                }
            }
            if (Galaxies != null)
            {
                foreach (Galaxy galaxy in Galaxies)
                {
                    bodies.Append(galaxy.GetAllSmallBodies());
                }
            }
            if (Sectors != null)
            {
                foreach (Sector sector in Sectors)
                {
                    bodies.Append(sector.GetAllSmallBodies());
                }
            }
            if (SolarSystems != null)
            {
                foreach (SolarSystem solarSystem in SolarSystems)
                {
                    bodies.Append(solarSystem.GetAllSmallBodies());
                }
            }
            if (Stars != null)
            {
                foreach (Star star in Stars)
                {
                    bodies.AddLast(star);
                }
            }
            if (Planets != null)
            {
                foreach (Planet planet in Planets)
                {
                    bodies.AddLast(planet);
                }
            }
            return bodies;
        }
    }
}