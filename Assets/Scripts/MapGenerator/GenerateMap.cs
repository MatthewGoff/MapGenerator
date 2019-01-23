using System.Collections.Generic;
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
            maxThreads = Mathf.Max(2, maxThreads);
            ThreadManager.Initialize(maxThreads - 1);
            ProgressTracker.Initialize();
        }

        private void PreInitializeMap(CelestialBodyType size, int seed)
        {
            if (size == CelestialBodyType.Universe)
            {
                Map = new Containers.Universe(seed, true);
            }
            else if (size == CelestialBodyType.Expanse)
            {
                Map = new Containers.Expanse(seed, true);
            }
            else if (size == CelestialBodyType.Galaxy)
            {
                Map = new Containers.Galaxy(seed, true);
            }
            else if (size == CelestialBodyType.Sector)
            {
                Map = new Containers.Sector(seed, true);
            }
            else
            {
                Map = new Containers.SolarSystem(seed, true);
            }
        }

        protected override void ThreadFunction()
        {
            ProgressTracker.Instance.PushActivity("Creating the " + MapSize.ToString());
            ProgressTracker.Instance.PushActivity("Allocating space");
            PreInitializeMap(MapSize, MapSeed);
            ProgressTracker.Instance.PopActivity();

            ProgressTracker.Instance.PushActivity("Populating");
            Map.Initialize();
            while (!Map.Initialized)
            {
                ThreadManager.Instance.Update();
            }
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PopActivity();
        }

        protected override void OnFinished()
        {
            FinishedCallback(Map);
        }
    }
}