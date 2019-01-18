using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public readonly bool PlainRendering = true;

    public CelestialBodies.CelestialBodyType MapSize = CelestialBodies.CelestialBodyType.SolarSystem;
    public CelestialBodies.CelestialBody Map;

    public Gradient StarGradient;
    public MapGenerator.Quadtree LastQuadtree;
    public GameObject MapManager;

    private void Awake()
    {
        Instance = this;

        // int seed = 865259136;
        int seed = (int)(Random.value * int.MaxValue);
        Random.InitState(seed);
        Debug.Log("Seed = " + seed);

        float time = Time.realtimeSinceStartup;
        MapGenerator.Containers.Container map = MapGenerator.MapGenerator.GenerateMap(MapSize);
        Debug.Log("Genesis Duration = " + (Time.realtimeSinceStartup - time) + " seconds");

        time = Time.realtimeSinceStartup;
        CreateMap(map);
        Debug.Log("Transfer Duration = " + (Time.realtimeSinceStartup - time) + " seconds");

        time = Time.realtimeSinceStartup;
        MapManager.GetComponent<Map.MapManager>().Initialize();
        Debug.Log("Render Duration = " + (Time.realtimeSinceStartup - time) + " seconds");
    }

    private void CreateMap(MapGenerator.Containers.Container map)
    {
        if (MapSize == CelestialBodies.CelestialBodyType.Galaxy)
        {
            Map = new CelestialBodies.Galaxy((MapGenerator.Containers.Galaxy)map);
        }
        if (MapSize == CelestialBodies.CelestialBodyType.Sector)
        {
            Map = new CelestialBodies.Sector((MapGenerator.Containers.Sector)map);
        }
        if (MapSize == CelestialBodies.CelestialBodyType.Cloud)
        {
            Map = new CelestialBodies.Cloud((MapGenerator.Containers.Cloud)map);
        }
        if (MapSize == CelestialBodies.CelestialBodyType.SolarSystem)
        {
            Map = new CelestialBodies.SolarSystem((MapGenerator.Containers.SolarSystem)map);
        }
    }
}
