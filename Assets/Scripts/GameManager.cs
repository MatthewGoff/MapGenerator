using System.Collections;
using System.IO;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Gradient StarGradient;
    public GameObject MapGenScreen;
    public GameObject MapRenderer;

    private float TimeStamp;
    private readonly CelestialBodyType MapSize = CelestialBodyType.Universe;
    private CelestialBodies.CelestialBody Map;

    private void Awake()
    {
        Instance = this;
        //CreateWorld();
        temp();
    }

    private void temp()
    {
        LoadMap();
        MapRenderer.GetComponent<MapRendering.MapRenderer>().Initialize(Map);
        MapRenderer.GetComponent<MapRendering.MapRenderer>().OpenMap();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapGenScreen.SetActive(!MapGenScreen.activeSelf);
        }
        Debug.Log(1f/Time.deltaTime);
    }

    private void CreateWorld()
    {
        System.Random rng = new System.Random();
        // int seed = 1216466133;
        int seed = rng.Next();
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
        SaveMap();

        MapRenderer.GetComponent<MapRendering.MapRenderer>().Initialize(Map);
        MapRenderer.GetComponent<MapRendering.MapRenderer>().OpenMap();
    }

    private void SaveMap()
    {
        Util.LinkedList<byte> linkedList = Map.Serialize();
        byte[] bytes = new byte[linkedList.Length];
        int i = 0;
        foreach (byte b in linkedList)
        {
            bytes[i++] = b;
        }
        File.WriteAllBytes("MapInfo.dat", bytes);
    }

    private void LoadMap()
    {
        byte[] bytes = File.ReadAllBytes("MapInfo.dat");
        CelestialBodyType type = (CelestialBodyType)BitConverter.ToInt32(bytes, 4);
        if (type == CelestialBodyType.Universe)
        {
            Map = new CelestialBodies.Universe(bytes, 0);
        }
        else if (type == CelestialBodyType.Expanse)
        {
            Map = new CelestialBodies.Expanse(bytes, 0);
        }
        else if (type == CelestialBodyType.Galaxy)
        {
            Map = new CelestialBodies.Galaxy(bytes, 0);
        }
        else if (type == CelestialBodyType.Sector)
        {
            Map = new CelestialBodies.Sector(bytes, 0);
        }
        else
        {
            Map = new CelestialBodies.SolarSystem(bytes, 0);
        }
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
