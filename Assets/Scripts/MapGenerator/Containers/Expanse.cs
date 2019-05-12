using UnityEngine;

namespace MapGenerator.Containers
{
    public class Expanse : Container
    {
        private static readonly int MIN_GALAXIES = 4;
        private static readonly int MAX_GALAXIES = 20;
        public static readonly float MAX_RADIUS = Galaxy.MAX_RADIUS * 4;

        public Expanse(CelestialBodyIdentifier id, int randomSeed, bool root) : base(CelestialBodyType.Expanse, id, 1f, randomSeed, MAX_RADIUS, root)
        {
            int population;
            if (root)
            {
                population = MAX_GALAXIES;
            }
            else
            {
                population = RNG.Next(MIN_GALAXIES, MAX_GALAXIES + 1);
            }
            AllocateGalaxies(population);
            AllocateSectors(population * 2);
            AllocateSolarSystems(population * 4);
            AllocateStars(population * 8);
            ProgressTracker.Instance.TotalExpanses++;
        }

        public override void Initialize()
        {
            StartActivity("CreatingGalaxies");
            InitializeGalaxies();
            EndActivity();
            StartActivity("Distributing Galaxies");
            Distribute(true, true);
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
            ProgressTracker.Instance.ExpanseInitialized();
        }
    }
}