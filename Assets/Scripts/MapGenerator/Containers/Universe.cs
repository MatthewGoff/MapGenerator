using UnityEngine;

namespace MapGenerator.Containers
{
    public class Universe : Container
    {
        private static readonly int MIN_SECTORS = 3;
        private static readonly int MAX_SECTORS = 9;
        public static readonly float MAX_RADIUS = Group.MAX_RADIUS * 4;

        public Universe(Vector2 localPosition, int randomSeed, bool root) : base(CelestialBodyType.Universe, localPosition, 1f, randomSeed, MAX_RADIUS, root)
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

            CreateExpanses(population, FinishedCreatingExpanses);
        }

        public void FinishedCreatingExpanses()
        {
            Distribute(true, true);
            CreateGroups(Expanses.Length * 2, FinishedCreatingGroups);
        }

        public void FinishedCreatingGroups()
        {
            Distribute(false, true);
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
        }
    }
}