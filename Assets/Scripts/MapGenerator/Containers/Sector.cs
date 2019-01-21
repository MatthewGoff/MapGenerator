using UnityEngine;

namespace MapGenerator.Containers
{
    public class Sector : Container
    {
        private static readonly int MIN_CLOUDS = 3;
        private static readonly int MAX_CLOUDS = 9;
        public static readonly float MAX_RADIUS = Cloud.MAX_RADIUS * 4;

        public Sector(Vector2 localPosition, int randomSeed, bool root) : base(CelestialBodyType.Sector, localPosition, 1f, randomSeed, MAX_RADIUS, root)
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
            CreateClouds(population);
            Distribute(true, true);
            CreateSolarSystems(Clouds.Length * 2);
            Distribute(false, true);
            CreateStars(SolarSystems.Length * 2);
            Distribute(false, true);
            FinalizeContainer();
        }
    }
}