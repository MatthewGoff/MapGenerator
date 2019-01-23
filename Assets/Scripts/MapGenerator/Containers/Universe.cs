using UnityEngine;

namespace MapGenerator.Containers
{
    public class Universe : Container
    {
        private static readonly int MIN_EXPANSES = 3;
        private static readonly int MAX_EXPANSES = 9;
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

        public override void Initialize(Callback callback = null)
        {
            InitializeExpanses();
            Distribute(true, true);
            InitializeGalaxies();
            Distribute(false, true);
            InitializeSectors();
            Distribute(false, true);
            InitializeSolarSystems();
            Distribute(false, true);
            InitializeStars();
            Distribute(false, true);
            FinalizeContainer();
            ProgressTracker.Instance.UniversesInitialized++;
        }
    }
}