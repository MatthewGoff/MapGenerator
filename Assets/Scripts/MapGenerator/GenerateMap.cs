using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    public class GenerateMap : ThreadedJob
    {
        public delegate void Callback(Containers.Container container, List<CelestialBodyIdentifier[]> jumps);

        private readonly CelestialBodyType MapSize;
        private readonly int MapSeed;
        private readonly Callback FinishedCallback;
        private Containers.Container Map;
        List<CelestialBodyIdentifier[]> WarpJumps;

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
                Map = new Containers.Universe(new CelestialBodyIdentifier(size), seed, true);
            }
            else if (size == CelestialBodyType.Expanse)
            {
                Map = new Containers.Expanse(new CelestialBodyIdentifier(size), seed, true);
            }
            else if (size == CelestialBodyType.Galaxy)
            {
                Map = new Containers.Galaxy(new CelestialBodyIdentifier(size), seed, true);
            }
            else if (size == CelestialBodyType.Sector)
            {
                Map = new Containers.Sector(new CelestialBodyIdentifier(size), seed, true);
            }
            else
            {
                Map = new Containers.SolarSystem(new CelestialBodyIdentifier(size), seed, true);
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

            if (GameManager.Instance.RenderEdges)
            {
                ProgressTracker.Instance.PushActivity("Moving data into graph module");
                EuclideanGraph.EuclideanGraph<Containers.Container> graph = new EuclideanGraph.EuclideanGraph<Containers.Container>();
                Util.LinkedList<Containers.Container> smallBodies = Map.GetAllSmallBodies();
                foreach (Containers.Container body in smallBodies)
                {
                    graph.AddNode(body);
                }
                ProgressTracker.Instance.PopActivity();

                ProgressTracker.Instance.PushActivity("Creating graph");
                graph.GenerateDelaunayTriangulation();
                graph.GenerateMST();
                graph.AddNoise(MapSeed);
                ProgressTracker.Instance.PopActivity();

                ProgressTracker.Instance.PushActivity("Moving data out of graph module");
                WarpJumps = new List<CelestialBodyIdentifier[]>();
                foreach (Containers.Container[] edge in graph.GetEdges())
                {
                    WarpJumps.Add(new CelestialBodyIdentifier[]
                    {
                    edge[0].ID,
                    edge[1].ID
                    });
                }
                ProgressTracker.Instance.PopActivity();
            }
            else
            {
                WarpJumps = new List<CelestialBodyIdentifier[]>();
            }

            ProgressTracker.Instance.PopActivity();
        }

        protected override void OnFinished()
        {
            FinishedCallback(Map, WarpJumps);
        }
    }
}