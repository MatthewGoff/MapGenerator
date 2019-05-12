using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Prefab;

    private Queue<float> FPSQueue;

    private void Awake()
    {
        FPSQueue = new Queue<float>();
        for (int i = 0; i < 5; i++)
        {
            FPSQueue.Enqueue(0f);
        }

        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 128; y++)
            {
                Instantiate(Prefab, new Vector2(x, y), Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        Vector3 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += movement * GetComponent<Camera>().orthographicSize * 0.1f;

        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom > 0)
        {
            GetComponent<Camera>().orthographicSize /= 1.1f;
        }
        else if (zoom < 0)
        {
            GetComponent<Camera>().orthographicSize *= 1.1f;
        }
        FPSQueue.Dequeue();
        FPSQueue.Enqueue(Time.deltaTime);
        float sum = 0;
        foreach (float sample in FPSQueue)
        {
            sum += sample;
        }
        sum /= FPSQueue.Count;
        Debug.Log(1f / sum);
    }
}
