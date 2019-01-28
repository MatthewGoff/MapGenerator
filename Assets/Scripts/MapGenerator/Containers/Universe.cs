using UnityEngine;

namespace MapGenerator.Containers
{
    public class Universe : Container
    {
        private static readonly int MIN_EXPANSES = 4;
        private static readonly int MAX_EXPANSES = 20;
        public static readonly float MAX_RADIUS = Expanse.MAX_RADIUS * 4;

        public Universe(int randomSeed, bool root) : base(CelestialBodyType.Universe, 1f, randomSeed, MAX_RADIUS, root)
        {
            int population;
            if (root)
            {
                population = MAX_EXPANSES;
            }
            else
            {
                population = RNG.Next(MIN_EXPANSES, MAX_EXPANSES + 1);
            }
            AllocateExpanses(population);
            AllocateGalaxies(population * 2);
            AllocateSectors(population * 4);
            AllocateSolarSystems(population * 8);
            AllocateStars(population * 16);
            ProgressTracker.Instance.TotalUniverses++;
        }

        public override void Initialize()
        {
            StartActivity("Creating Expanses");
            InitializeExpanses(OnExpansesInitialized);
        }

        public void OnExpansesInitialized()
        {
            EndActivity();
            StartActivity("Distributing Expanses");
            Distribute(true, true);
            EndActivity();
            StartActivity("Creating Galaxies");
            InitializeGalaxies();
            EndActivity();
            StartActivity("Distributing Galaxies");
            Distribute(false, true);
            EndActivity();
            StartActivity("Creating Sectors");
            InitializeSectors();
            EndActivity();
            StartActivity("Distributing Sectors");
            Distribute(false, true);
            EndActivity();
            StartActivity("Creating Solar Systems");
            InitializeSolarSystems();
            EndActivity();
            StartActivity("Distributing SolarSystems");
            Distribute(false, true);
            EndActivity();
            StartActivity("Creating Stars");
            InitializeStars();
            EndActivity();
            StartActivity("Distributing Stars");
            Distribute(false, true);
            EndActivity();
            StartActivity("Finalizing");
            FinalizeContainer();
            EndActivity();
            ProgressTracker.Instance.UniverseInitialized();
        }
    }
}