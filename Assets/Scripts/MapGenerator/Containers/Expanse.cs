using UnityEngine;

namespace MapGenerator.Containers
{
    public class Expanse : Container
    {
        private static readonly int MIN_GROUPS = 3;
        private static readonly int MAX_GROUPS = 9;
        public static readonly float MAX_RADIUS = Group.MAX_RADIUS * 4;

        private Callback ExpanseCreatedCallback;

        public Expanse(Vector2 localPosition, int randomSeed, bool root, Callback callback = null) : base(CelestialBodyType.Expanse, localPosition, 1f, randomSeed, MAX_RADIUS, root)
        {
            ExpanseCreatedCallback = callback;
            int population;
            if (root)
            {
                population = MAX_GROUPS;
            }
            else
            {
                population = RNG.Next(MIN_GROUPS, MAX_GROUPS + 1);
            }
            CreateGroups(population, FinishedCreatingGroups);
        }

        public void FinishedCreatingGroups()
        {
            Distribute(true, true);
            CreateGalaxies(Groups.Length * 2);
            Distribute(false, true);
            CreateSectors(Galaxies.Length * 2);
            Distribute(false, true);
            CreateClouds(Sectors.Length * 2);
            Distribute(false, true);
            CreateSolarSystems(Clouds.Length * 2);
            Distribute(false, true);
            CreateStars(SolarSystems.Length * 2);
            Distribute(false, true);
            FinalizeContainer();
            if (ExpanseCreatedCallback != null)
            {
                ExpanseCreatedCallback();
            }
        }
    }
}