using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool Log = true;
    public readonly bool PlainRendering = true;

    private Universe Universe;

    public Gradient StarGradient;
    public Quadtree LastQuadtree;

    private void Awake()
    {
        Instance = this;
        Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        // int seed = 64834568;
        int seed = (int)(Random.value * int.MaxValue);
        Random.InitState(seed);
        Debug.Log(seed);
        if (Universe != null)
        {
            //Universe.Destroy();   
        }
        float time = Time.realtimeSinceStartup;
        Universe = new Universe(new Vector2(0, 0));
        Debug.Log("Setup Duration = " + (Time.realtimeSinceStartup - time) + " seconds");
        time = Time.realtimeSinceStartup;
        Universe.Realize(new Vector2(0,0));
        Debug.Log("Render Duration = " + (Time.realtimeSinceStartup - time) + " seconds");
    }
}
