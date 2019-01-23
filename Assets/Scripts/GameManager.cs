using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public readonly bool PlainRendering = true;
    public bool DrawQuadtree = false;

    public CelestialBodyType MapSize = CelestialBodyType.SolarSystem;
    public CelestialBodies.CelestialBody Map;

    public Gradient StarGradient;
    public Quadtree Quadtree;
    public MapRenderer MapRenderer;
    public GameObject Camera;
    public GameObject MapGenScreen;

    public GameObject marker;

    private float TimeStamp;

    private void Awake()
    {
        Instance = this;

        //CreateBoxes();
        CreateWorld();
        //RandomTest();
    }

    private void RandomTest()
    {
        System.Random rng = new System.Random();
        for (int i = 0; i < 100; i++)
        {
            Instantiate(marker, Helpers.InsideUnitCircle(rng) * 10f, Quaternion.identity);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DrawQuadtree = !DrawQuadtree;
        }
        // Debug.Log(1f/Time.deltaTime);
    }

    private void CreateWorld()
    {
        System.Random rng = new System.Random();
        int seed = 547291039;
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

        MapRenderer = new MapRenderer(Camera.GetComponent<Camera>().pixelWidth, Camera.GetComponent<Camera>().pixelHeight, Map);
        MapGenScreen.SetActive(false);
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
