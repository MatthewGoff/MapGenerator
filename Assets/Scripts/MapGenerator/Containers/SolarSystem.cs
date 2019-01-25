using UnityEngine;

namespace MapGenerator.Containers
{
    public class SolarSystem : Container
    {
        private static readonly int MIN_PLANETS = 4;
        private static readonly int MAX_PLANETS = 20;
        public static readonly float MAX_RADIUS = Planet.MAX_RADIUS * 10;

        public SolarSystem(int randomSeed, bool root) : base(CelestialBodyType.SolarSystem, 5f, randomSeed, MAX_RADIUS, root)
        {
            Stars = new Star[] { new Star(RNG.Next(), true, false) };
            int population;
            if (root)
            {
                population = MAX_PLANETS;
            }
            else
            {
                population = RNG.Next(MIN_PLANETS, MAX_PLANETS + 1);
            }
            AllocatePlanets(population);
            ProgressTracker.Instance.TotalSolarSystems++;
        }

        public override void Initialize()
        {
            StartActivity("Creating Star");
            InitializeStars();
            Stars[0].LocalPosition = new Vector2(0, 0);
            EndActivity();
            StartActivity("Creating Planets");
            InitializePlanets();
            EndActivity();
            StartActivity("Distributing Planets");
            Distribute(false, false);
            EndActivity();
            FinalizeContainer();
            ProgressTracker.Instance.SolarSystemInitialized();
        }
    }
}