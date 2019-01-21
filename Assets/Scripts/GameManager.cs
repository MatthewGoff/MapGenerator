using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public readonly bool PlainRendering = true;
    public bool DrawQuadtree = true;

    public CelestialBodyType MapSize = CelestialBodyType.SolarSystem;
    public CelestialBodies.CelestialBody Map;

    public Gradient StarGradient;
    public Quadtree Quadtree;
    public MapRenderer MapRenderer;
    public GameObject Camera;
    public GameObject Square;

    private float TimeStamp;

    private void Awake()
    {
        Instance = this;

        //CreateBoxes();
        CreateWorld();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DrawQuadtree = !DrawQuadtree;
        }
        // Debug.Log(1f/Time.deltaTime);
    }

    private void CreateBoxes()
    {
        for (int i = 0; i < 8; i++)
        {
            int x = i * 4096;
            for (int j = 0; j < 8; j++)
            {
                int y = j * 4096;
                Instantiate(Square, new Vector2(x, y), Quaternion.identity);
            }
        }
    }

    private void CreateWorld()
    {
        System.Random rng = new System.Random();
        // int seed = 865259136;
        int seed = rng.Next();
        rng = new System.Random(seed);
        Debug.Log("Seed = " + seed);

        TimeStamp = Time.realtimeSinceStartup;
        MapGenerator.GenerateMap thread = new MapGenerator.GenerateMap(MapSize, rng.Next(), System.Environment.ProcessorCount - 2, OnWorldGenerated);
        thread.Start();
        StartCoroutine(WaitForMapGeneration(thread));
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

        TimeStamp = Time.realtimeSinceStartup;
        CreateMap(map);
        Debug.Log("Transfer Duration = " + (Time.realtimeSinceStartup - TimeStamp) + " seconds");

        MapRenderer = new MapRenderer(Camera.GetComponent<Camera>().pixelWidth, Camera.GetComponent<Camera>().pixelHeight, Map);
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
        else if (MapSize == CelestialBodyType.Group)
        {
            Map = new CelestialBodies.Group((MapGenerator.Containers.Group)map);
        }
        else if (MapSize == CelestialBodyType.Galaxy)
        {
            Map = new CelestialBodies.Galaxy((MapGenerator.Containers.Galaxy)map);
        }
        else if (MapSize == CelestialBodyType.Sector)
        {
            Map = new CelestialBodies.Sector((MapGenerator.Containers.Sector)map);
        }
        else if (MapSize == CelestialBodyType.Cloud)
        {
            Map = new CelestialBodies.Cloud((MapGenerator.Containers.Cloud)map);
        }
        else if (MapSize == CelestialBodyType.SolarSystem)
        {
            Map = new CelestialBodies.SolarSystem((MapGenerator.Containers.SolarSystem)map);
        }
    }
    
    public Rect GetCameraRect()
    {
        Camera camera = Camera.GetComponent<Camera>();
        float height = camera.orthographicSize * 2;
        float width = height * camera.aspect;
        Vector2 size = new Vector2(width, height);
        Vector2 position = (Vector2) camera.transform.position - size / 2f;
        return new Rect(position, size);
    }

    public float GetCameraResolution()
    {
        Camera camera = Camera.GetComponent<Camera>();
        return camera.pixelHeight / (camera.orthographicSize * 2f);
    }
}
