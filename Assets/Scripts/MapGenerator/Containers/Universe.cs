using UnityEngine;

namespace MapGenerator.Containers
{
    public class Universe : Container
    {
        private static readonly int MIN_SECTORS = 3;
        private static readonly int MAX_SECTORS = 9;
        public static readonly float MAX_RADIUS = Group.MAX_RADIUS * 4;

        public Universe(int randomSeed, bool root) : base(CelestialBodyType.Universe, 1f, randomSeed, MAX_RADIUS, root)
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
            AllocateExpanses(population);
            AllocateGroups(population * 2);
            AllocateGalaxies(population * 4);
            AllocateSectors(population * 8);
            AllocateClouds(population * 16);
            AllocateSolarSystems(population * 32);
            AllocateStars(population * 64);
            ProgressTracker.Instance.TotalUniverses++;
        }

        public override void Initialize(Callback callback = null)
        {
            InitializeExpanses(ExpansesInitialized);
        }

        public void ExpansesInitialized()
        {
            Distribute(true, true);
            InitializeGroups(GroupsInitialized);
        }

        public void GroupsInitialized()
        {
            Distribute(false, true);
            InitializeGalaxies();
            Distribute(false, true);
            InitializeSectors();
            Distribute(false, true);
            InitializeClouds();
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