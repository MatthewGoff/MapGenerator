﻿using UnityEngine;

namespace MapGenerator.Containers
{
    public class Galaxy : Container
    {
        private static readonly int MIN_SECTORS = 4;
        private static readonly int MAX_SECTORS = 20;
        public static readonly float MAX_RADIUS = Sector.MAX_RADIUS * 4;

        public Galaxy(int randomSeed, bool root) : base(CelestialBodyType.Galaxy, 1f, randomSeed, MAX_RADIUS, root)
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

        public override void Initialize(Callback callback = null)
        {
            InitializeSectors();
            Distribute(true, true);
            InitializeSolarSystems();
            Distribute(false, true);
            InitializeStars();
            Distribute(false, true);
            FinalizeContainer();
            ProgressTracker.Instance.GalaxiesInitialized++;
        }
    }
}