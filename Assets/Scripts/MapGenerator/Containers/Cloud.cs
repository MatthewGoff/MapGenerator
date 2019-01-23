using UnityEngine;

namespace MapGenerator.Containers
{
    public class Cloud : Container
    {
        private static readonly int MIN_SOLAR_SYSTEMS = 3;
        private static readonly int MAX_SOLAR_SYSTEMS = 9;
        public static readonly float MAX_RADIUS = SolarSystem.MAX_RADIUS * 4;

        public Cloud(int randomSeed, bool root) : base(CelestialBodyType.Cloud, 1f, randomSeed, MAX_RADIUS, root)
        {
            int population;
            if (root)
            {
                population = MAX_SOLAR_SYSTEMS;
            }
            else
            {
                population = RNG.Next(MIN_SOLAR_SYSTEMS, MAX_SOLAR_SYSTEMS + 1);
            }
            AllocateSolarSystems(population);
            AllocateStars(population * 2);
            ProgressTracker.Instance.TotalClouds++;
        }

        public override void Initialize(Callback callback = null)
        {
            InitializeSolarSystems();
            Distribute(true, true);
            InitializeStars();
            Distribute(false, true);
            FinalizeContainer();
            ProgressTracker.Instance.CloudsInitialized++;
        }
    }
}