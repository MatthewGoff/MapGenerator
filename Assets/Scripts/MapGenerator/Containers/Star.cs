using UnityEngine;

namespace MapGenerator.Containers
{
    public class Star : Container
    {
        public static readonly float MIN_RADIUS = 0.5f;
        public static readonly float MAX_RADIUS = 2f;

        public Star(Vector2 localPosition, int randomSeed, bool immovable) : base(CelestialBodyType.Star, localPosition, 1f, randomSeed, MAX_RADIUS, immovable)
        {
            Radius = (float)RNG.NextDouble() * (MAX_RADIUS - MIN_RADIUS) + MIN_RADIUS;
        }
    }
}