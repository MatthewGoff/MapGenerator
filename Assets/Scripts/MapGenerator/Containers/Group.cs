using UnityEngine;

namespace MapGenerator.Containers
{
    public class Group : Container
    {
        private static readonly int MIN_GALAXIES = 9;
        private static readonly int MAX_GALAXIES = 9;
        public static readonly float MAX_RADIUS = Galaxy.MAX_RADIUS * 4;

        public Group(int randomSeed, bool root) : base(CelestialBodyType.Group, 1f, randomSeed, MAX_RADIUS, root)
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
            AllocateClouds(population * 4);
            AllocateSolarSystems(population * 8);
            AllocateStars(population * 16);
            ProgressTracker.Instance.TotalGroups++;
        }

        public override void Initialize(Callback callback = null)
        {
            InitializeGalaxies();
            Distribute(true, true);
            InitializeSectors();
            Distribute(false, true);
            InitializeClouds();
            Distribute(false, true);
            InitializeSolarSystems();
            Distribute(false, true);
            InitializeStars();
            Distribute(false, true);
            FinalizeContainer();
            ProgressTracker.Instance.GroupsInitialized++;
        }
    }
}