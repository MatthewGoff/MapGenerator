using UnityEngine;

namespace MapGenerator.Containers
{
    public class Galaxy : Container
    {
        private static readonly int MIN_SECTORS = 3;
        private static readonly int MAX_SECTORS = 9;
        public static readonly float MAX_RADIUS = Sector.MAX_RADIUS * 4;

        public Galaxy(Vector2 localPosition, int randomSeed, bool root) : base(CelestialBodyType.Galaxy, localPosition, 1f, randomSeed, MAX_RADIUS, root)
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
            CreateSectors(population);
            Distribute(true, true);
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