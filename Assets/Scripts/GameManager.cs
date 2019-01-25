using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Gradient StarGradient;
    public GameObject MapGenScreen;
    public GameObject MapRenderer;

    private float TimeStamp;
    private readonly CelestialBodyType MapSize = CelestialBodyType.SolarSystem;
    private CelestialBodies.CelestialBody Map;

    private void Awake()
    {
        Instance = this;

        CreateWorld();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapGenScreen.SetActive(!MapGenScreen.activeSelf);
        }
        // Debug.Log(1f/Time.deltaTime);
    }

    private void CreateWorld()
    {
        System.Random rng = new System.Random();
        int seed = 1216466133;
        // int seed = rng.Next();
        rng = new System.Random(seed);
        Debug.Log("Seed = " + seed);

        TimeStamp = Time.realtimeSinceStartup;
        MapGenerator.GenerateMap thread = new MapGenerator.GenerateMap(MapSize, rng.Next(), System.Environment.ProcessorCount - 1, OnWorldGenerated);
        thread.Start();
        StartCoroutine(WaitForMapGeneration(thread));
        MapGenScreen.SetActive(true);
    }

    public IEnumerator WaitForMapGeneration(MapGenerator.GenerateMap thread)
    {
        while(Map == null)
        {
            thread.Update();
            yield return null;
        }
    }

    private void OnWorldGenerated(MapGenerator.Containers.Container map)
    {
        Debug.Log("Genesis Duration = " + (Time.realtimeSinceStartup - TimeStamp) + " seconds");

        CreateMap(map);

        MapRenderer.GetComponent<MapRendering.MapRenderer>().Initialize(Map);
        MapRenderer.GetComponent<MapRendering.MapRenderer>().OpenMap();
    }

    private void CreateMap(MapGenerator.Containers.Container map)
    {
        if (MapSize == CelestialBodyType.Universe)
        {
            Map = new CelestialBodies.Universe((MapGenerator.Containers.Universe)map);
        }
        else if (MapSize == CelestialBodyType.Expanse)
        {
            Map = new CelestialBodies.Expanse((MapGenerator.Containers.Expanse)map);
        }
        else if (MapSize == CelestialBodyType.Galaxy)
        {
            Map = new CelestialBodies.Galaxy((MapGenerator.Containers.Galaxy)map);
        }
        else if (MapSize == CelestialBodyType.Sector)
        {
            Map = new CelestialBodies.Sector((MapGenerator.Containers.Sector)map);
        }
        else if (MapSize == CelestialBodyType.SolarSystem)
        {
            Map = new CelestialBodies.SolarSystem((MapGenerator.Containers.SolarSystem)map);
        }
    }
}
