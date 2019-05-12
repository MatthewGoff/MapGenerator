using UnityEngine;

namespace MapGenerator.Containers
{
    public class Star : Container
    {
        public static readonly float MIN_RADIUS = 0.5f;
        public static readonly float MAX_RADIUS = 2f;

        public Star(CelestialBodyIdentifier id, int randomSeed, bool immovable, bool root) : base(CelestialBodyType.Star, id, 1f, randomSeed, MAX_RADIUS, root, immovable)
        {
            Radius = (float)RNG.NextDouble() * (MAX_RADIUS - MIN_RADIUS) + MIN_RADIUS;
            ProgressTracker.Instance.TotalStars++;
        }
    }
}