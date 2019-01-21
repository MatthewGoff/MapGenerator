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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSize"></param>
        /// <param name="seed"></param>
        /// <param name="maxThreads">Minimum of 2 threads required</param>
        /// <param name="callback"></param>
        public GenerateMap(CelestialBodyType mapSize, int seed, int maxThreads, Callback callback)
        {
            MapSize = mapSize;
            MapSeed = seed;
            FinishedCallback = callback;
            maxThreads = Mathf.Min(2, maxThreads);
            ThreadManager.Initialize(maxThreads - 1);
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
            while (!Map.Initialized)
            {
                ThreadManager.Instance.Update();
            }
        }

        protected override void OnFinished()
        {
            FinishedCallback(Map);
        }
    }
}