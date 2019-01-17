using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool Log = true;
    public readonly bool PlainRendering = true;

    private Galaxy Galaxy;

    public Gradient StarGradient;

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
        // int seed = 587056704;
        int seed = (int)(Random.value * int.MaxValue);
        Random.InitState(seed);
        Debug.Log(seed);
        if (Galaxy != null)
        {
            Galaxy.Destroy();   
        }
        Galaxy = new Galaxy(new Vector2(0, 0));
        Galaxy.Realize(new Vector2(0,0));
    }
}
