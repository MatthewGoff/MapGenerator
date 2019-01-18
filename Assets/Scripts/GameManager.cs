using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool Log = true;
    public readonly bool PlainRendering = true;

    private Group Group;

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
        int seed = 865259136;
        // int seed = (int)(Random.value * int.MaxValue);
        Random.InitState(seed);
        Debug.Log(seed);
        if (Group != null)
        {
            Group.Destroy();   
        }
        float time = Time.realtimeSinceStartup;
        Group = new Group(new Vector2(0, 0));
        Debug.Log("Setup Duration = " + (Time.realtimeSinceStartup - time) + " seconds");
        time = Time.realtimeSinceStartup;
        Group.Realize(new Vector2(0,0));
        Debug.Log("Render Duration = " + (Time.realtimeSinceStartup - time) + " seconds");
    }
}
