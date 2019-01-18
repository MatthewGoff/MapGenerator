using UnityEngine;

namespace MapGenerator.Containers
{
    public class Sector : Container
    {
        private static readonly int MIN_CLOUDS = 3;
        private static readonly int MAX_CLOUDS = 9;
        public static readonly float MAX_RADIUS = Cloud.MAX_RADIUS * 4;

        public Sector(Vector2 localPosition, bool maximize = false) : base(localPosition, 1f, MAX_RADIUS)
        {
            int population;
            if (maximize)
            {
                population = MAX_CLOUDS;
            }
            else
            {
                population = Random.Range(MIN_CLOUDS, MAX_CLOUDS + 1);
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