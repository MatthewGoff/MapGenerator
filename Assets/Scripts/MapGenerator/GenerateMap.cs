using UnityEngine;

namespace MapGenerator
{
    public class GenerateMap : ThreadedJob
    {
        public delegate void Callback(Containers.Container container);

        private readonly CelestialBodyType MapSize;
        private readonly int MapSeed;
        private readonly Callback FinishedCallback;
        private Containers.Container Map;

        public GenerateMap(CelestialBodyType celestialBodyType, int seed, Callback callback)
        {
            MapSize = celestialBodyType;
            MapSeed = seed;
            FinishedCallback = callback;
        }

        protected override void ThreadFunction()
        {
            if (MapSize == CelestialBodyType.Universe)
            {
                Map = new Containers.Universe(new Vector2(0f, 0f), MapSeed, true);
            }
            else if (MapSize == CelestialBodyType.Expanse)
            {
                Map = new Containers.Expanse(new Vector2(0f, 0f), MapSeed, true);
            }
            else if (MapSize == CelestialBodyType.Group)
            {
                Map = new Containers.Group(new Vector2(0f, 0f), MapSeed, true);
            }
            else if (MapSize == CelestialBodyType.Galaxy)
            {
                Map = new Containers.Galaxy(new Vector2(0f, 0f), MapSeed, true);
            }
            else if (MapSize == CelestialBodyType.Sector)
            {
                Map = new Containers.Sector(new Vector2(0f, 0f), MapSeed, true);
            }
            else if (MapSize == CelestialBodyType.Cloud)
            {
                Map = new Containers.Cloud(new Vector2(0f, 0f), MapSeed, true);
            }
            else
            {
                Map = new Containers.SolarSystem(new Vector2(0f, 0f), MapSeed, true);
            }
            Map.CalculateGlobalPositions(new Vector2(0f, 0f));
        }

        protected override void OnFinished()
        {
            FinishedCallback(Map);
        }
    }
}