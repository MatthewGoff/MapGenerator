using UnityEngine;

namespace MapGenerator.Containers
{
    public class Planet : Container
    {
        public static readonly float MIN_RADIUS = 0.5f;
        public static readonly float MAX_RADIUS = 2.0f;

        public Planet(int randomSeed, bool root) : base(CelestialBodyType.Planet, 1f, randomSeed, MAX_RADIUS, root)
        {
            Radius = (float)RNG.NextDouble() * (MAX_RADIUS - MIN_RADIUS) + MIN_RADIUS;
            ProgressTracker.Instance.TotalPlanets++;
        }
    }
}