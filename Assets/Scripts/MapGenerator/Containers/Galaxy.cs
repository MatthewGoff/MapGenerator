using UnityEngine;

namespace MapGenerator.Containers
{
    public class Galaxy : Container
    {
        private static readonly int MIN_SECTORS = 4;
        private static readonly int MAX_SECTORS = 20;
        public static readonly float MAX_RADIUS = Sector.MAX_RADIUS * 4;

        public Galaxy(CelestialBodyIdentifier id, int randomSeed, bool root) : base(CelestialBodyType.Galaxy, id, 1f, randomSeed, MAX_RADIUS, root)
        {
            int population;
            if (root)
            {
                population = MAX_SECTORS;
            }
            else
            {
                population = RNG.Next(MIN_SECTORS, MAX_SECTORS + 1);
            }
            AllocateSectors(population);
            AllocateSolarSystems(population * 2);
            AllocateStars(population * 4);
            ProgressTracker.Instance.TotalGalaxies++;
        }

        public override void Initialize()
        {
            StartActivity("Creating Sectors");
            InitializeSectors();
            EndActivity();
            StartActivity("Initializing Sectors");
            Distribute(true, true);
            EndActivity();
            StartActivity("Creating Solar Systems");
            InitializeSolarSystems();
            EndActivity();
            StartActivity("Distributing Solar Systems");
            Distribute(false, true);
            EndActivity();
            StartActivity("Creating Stars");
            InitializeStars();
            EndActivity();
            StartActivity("Distributing Stars");
            Distribute(false, true);
            EndActivity();
            FinalizeContainer();
            ProgressTracker.Instance.GalaxyInitialized();
        }
    }
}