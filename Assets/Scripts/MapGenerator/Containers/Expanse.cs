using UnityEngine;

namespace MapGenerator.Containers
{
    public class Expanse : Container
    {
        private static readonly int MIN_GALAXIES = 3;
        private static readonly int MAX_GALAXIES = 9;
        public static readonly float MAX_RADIUS = Galaxy.MAX_RADIUS * 4;

        public Expanse(int randomSeed, bool root) : base(CelestialBodyType.Expanse, 1f, randomSeed, MAX_RADIUS, root)
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

        public override void Initialize(Callback callback = null)
        {
            InitializeGalaxies();
            Distribute(true, true);
            InitializeSectors();
            Distribute(false, true);
            InitializeSolarSystems();
            Distribute(false, true);
            InitializeStars();
            Distribute(false, true);
            FinalizeContainer();
            ProgressTracker.Instance.ExpansesInitialized++;
        }
    }
}