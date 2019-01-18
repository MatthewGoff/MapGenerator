using UnityEngine;

namespace MapGenerator.Containers
{
    public class Sector : Container
    {
        private static readonly int MIN_CLOUDS = 3;
        private static readonly int MAX_CLOUDS = 9;
        public static readonly float MAX_RADIUS = Cloud.MAX_RADIUS * 4;

        public Sector(Vector2 localPosition) : base(localPosition, 1f, MAX_RADIUS)
        {
            CreateClouds(Random.Range(MIN_CLOUDS, MAX_CLOUDS + 1));
            Distribute(true, true);
            CreateSolarSystems(Clouds.Length * 2);
            Distribute(false, true);
            CreateStars(SolarSystems.Length * 2);
            Distribute(false, true);
            FinalizeContainer();
        }
    }
}