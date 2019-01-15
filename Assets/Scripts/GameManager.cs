using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static readonly int HIGHEST_ORDER = 4;
    public static readonly int MIN_CLUSTER_POPULATION = 3;
    public static readonly int MAX_CLUSTER_POPULATION = 24;
    public static readonly float SIZE = 8f;
    public static readonly float MINIMUM_DISTANCE = 1.1f;
    public static readonly float MARGIN = 0.5f;
    public static readonly int MAX_DISTRIBUTE_ITERATIONS = 1000;

    public static GameManager Instance;
    public GameObject Node;

    private Group Universe;

    private void Awake()
    {
        Instance = this;
        CreateUniverse();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateUniverse();
        }
    }

    private void CreateUniverse()
    {
        if (Universe != null)
        {
            Universe.Destroy();
        }
        Universe = new Group(HIGHEST_ORDER, new Vector2(0, 0));
        Universe.Populate();
    }

    public static int RandomInt(int min, int max)
    {
        return Mathf.FloorToInt(Mathf.Clamp(Random.Range(min, max + 1), min, max));
    }
}
