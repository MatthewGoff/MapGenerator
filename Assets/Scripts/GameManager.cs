using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AnimationCurve EDGE_PROBABILITY = new AnimationCurve
    (
        new Keyframe(2, 0.05f),
        new Keyframe(20, 0.5f)
    );

    public static GameManager Instance;

    public bool RenderEdges;
    public Gradient StarGradient;
    public GameObject MapGenScreen;
    public GameObject MapRenderer_Shader;
    public GameObject MapRenderer_Sprite;
    public GameObject SeedText;
    public GameObject GenesisText;
    public GameObject MenuScreen;

    private float TimeStamp;
    private CelestialBodyType MapSize;
    private CelestialBodies.CelestialBody Map;
    private Network Network;
    private bool Menu;

    private void Awake()
    {
        Instance = this;
        Menu = true;
    }

    private void DelaunayTest()
    {
        MapGenerator.ProgressTracker.Initialize();

        MapGenerator.Containers.Container Map = new MapGenerator.Containers.SolarSystem(new CelestialBodyIdentifier(CelestialBodyType.SolarSystem), 1216466133, true);
        Map.Initialize();
        MapGenerator.EuclideanGraph.EuclideanGraph<MapGenerator.Containers.Container> graph = new MapGenerator.EuclideanGraph.EuclideanGraph<MapGenerator.Containers.Container>();
        Util.LinkedList<MapGenerator.Containers.Container> smallBodies = Map.GetAllSmallBodies();
        foreach (MapGenerator.Containers.Container body in smallBodies)
        {
            graph.AddNode(body);
        }
        graph.GenerateDelaunayTriangulation();
    }

    private void LoadGame()
    {
        LoadMap();
        DisplayMap();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && Menu)
        {
            Menu = false;
            MenuScreen.SetActive(false);
            RenderEdges = true;
            MapSize = CelestialBodyType.Galaxy;
            InitializeGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Menu)
        {
            Menu = false;
            MenuScreen.SetActive(false);
            RenderEdges = false;
            MapSize = CelestialBodyType.Universe;
            InitializeGame();
        }
        //Debug.Log(1f/Time.deltaTime);
    }

    private void InitializeGame()
    {
        System.Random rng = new System.Random();
        // int seed = 1216466133;
        int seed = rng.Next();
        SeedText.GetComponent<Text>().text = "Seed = " + seed.ToString();
        rng = new System.Random(seed);
        Debug.Log("Seed = " + seed);

        TimeStamp = Time.realtimeSinceStartup;
        MapGenerator.GenerateMap thread = new MapGenerator.GenerateMap(MapSize, rng.Next(), System.Environment.ProcessorCount - 1, OnWorldGenerated);
        thread.Start();
        StartCoroutine(WaitForMapGeneration(thread));
    }

    public IEnumerator WaitForMapGeneration(MapGenerator.GenerateMap thread)
    {
        MapGenScreen.SetActive(true);
        while (!thread.IsDone)
        {
            GenesisText.GetComponent<Text>().text = "Genesis took " + Mathf.FloorToInt(Time.realtimeSinceStartup - TimeStamp) + " seconds";
            thread.Update();
            yield return null;
        }
        thread.Update();
        MapGenScreen.SetActive(false);
    }

    private void OnWorldGenerated(MapGenerator.Containers.Container map, List<CelestialBodyIdentifier[]> warpJumps)
    {
        Debug.Log("Genesis Duration = " + (Time.realtimeSinceStartup - TimeStamp) + " seconds");

        CreateMap(map, warpJumps);
        SaveMap();
        DisplayMap();
    }

    private void DisplayMap()
    {
        if (RenderEdges)
        {
            MapRenderer_Sprite.SetActive(true);
            MapRenderer_Sprite.GetComponent<MapRendering_Sprite.MapRenderer_Sprite>().Initialize(Map, Network);
            MapRenderer_Sprite.GetComponent<MapRendering_Sprite.MapRenderer_Sprite>().OpenMap();
        }
        else
        {
            MapRenderer_Shader.SetActive(true);
            MapRenderer_Shader.GetComponent<MapRendering_Shader.MapRenderer_Shader>().Initialize(Map, Network);
            MapRenderer_Shader.GetComponent<MapRendering_Shader.MapRenderer_Shader>().OpenMap();
        }
    }

    private void SaveMap()
    {
        Util.LinkedList<byte> linkedList = Map.Serialize();
        linkedList.Append(Network.Serialize());
        byte[] bytes = new byte[linkedList.Length];
        int i = 0;
        foreach (byte b in linkedList)
        {
            bytes[i++] = b;
        }
        System.IO.File.WriteAllBytes("MapInfo.dat", bytes);
    }

    private void LoadMap()
    {
        byte[] bytes = System.IO.File.ReadAllBytes("MapInfo.dat");
        CelestialBodyType type = (CelestialBodyType)System.BitConverter.ToInt32(bytes, 4);
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

        int offset = System.BitConverter.ToInt32(bytes, 0);
        Network = new Network(bytes, offset, Map);
    }

    private void CreateMap(MapGenerator.Containers.Container map, List<CelestialBodyIdentifier[]> warpJumps)
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

        Network = new Network(warpJumps, Map);
    }
}
