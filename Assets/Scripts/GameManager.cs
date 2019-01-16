using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool Log = false;

    private Cloud Cloud;

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
        int seed = (int)(Random.value * int.MaxValue);
        Random.InitState(seed);
        Debug.Log(seed);
        if (Cloud != null)
        {
            Cloud.Destroy();   
        }
        Cloud = new Cloud(new Vector2(0, 0));
        Cloud.Realize(new Vector2(0,0));
    }
}
