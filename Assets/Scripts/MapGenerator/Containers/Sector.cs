using UnityEngine;

namespace MapGenerator.Containers
{
    public class Sector : Container
    {
        private static readonly int MIN_CLOUDS = 3;
        private static readonly int MAX_CLOUDS = 9;
        public static readonly float MAX_RADIUS = Cloud.MAX_RADIUS * 4;

        public Sector(int randomSeed, bool root) : base(CelestialBodyType.Sector, 1f, randomSeed, MAX_RADIUS, root)
        {
            int population;
            if (root)
            {
                population = MAX_CLOUDS;
            }
            else
            {
                population = RNG.Next(MIN_CLOUDS, MAX_CLOUDS + 1);
            }
            AllocateClouds(population);
            AllocateSolarSystems(population * 2);
            AllocateStars(population * 4);
            ProgressTracker.Instance.TotalSectors++;
        }

        public override void Initialize(Callback callback = null)
        {
            InitializeClouds();
            Distribute(true, true);
            InitializeSolarSystems();
            Distribute(false, true);
            InitializeStars();
            Distribute(false, true);
            FinalizeContainer();
            ProgressTracker.Instance.SectorsInitialized++;
        }
    }
}