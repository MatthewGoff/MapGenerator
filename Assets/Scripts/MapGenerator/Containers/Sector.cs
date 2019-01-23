using UnityEngine;

namespace MapGenerator.Containers
{
    public class Sector : Container
    {
        private static readonly int MIN_SOLAR_SYSTEMS = 4;
        private static readonly int MAX_SOLAR_SYSTEMS = 20;
        public static readonly float MAX_RADIUS = SolarSystem.MAX_RADIUS * 4;

        public Sector(int randomSeed, bool root) : base(CelestialBodyType.Sector, 1f, randomSeed, MAX_RADIUS, root)
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
            ProgressTracker.Instance.TotalSectors++;
        }

        public override void Initialize()
        {
            StartActivity("Create Solar Systems");
            InitializeSolarSystems();
            EndActivity();
            StartActivity("Distribute Solar Systems");
            Distribute(true, true);
            EndActivity();
            StartActivity("Create Stars");
            InitializeStars();
            EndActivity();
            StartActivity("Distribute Stars");
            Distribute(false, true);
            EndActivity();
            FinalizeContainer();
            ProgressTracker.Instance.SectorInitialized();
        }
    }
}