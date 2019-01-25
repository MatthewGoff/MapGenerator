using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static readonly float PAN_SPEED_MULTIPLIER = 0.1f;
    private static readonly float ZOOM_MULTIPLIER = 1.2f;

    private void Update()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput > 0)
        {
            GetComponent<Camera>().orthographicSize /= ZOOM_MULTIPLIER;
        }
        else if (zoomInput < 0)
        {
            GetComponent<Camera>().orthographicSize *= ZOOM_MULTIPLIER;
        }
        
        Vector2 panInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += (Vector3) panInput * PAN_SPEED_MULTIPLIER * GetComponent<Camera>().orthographicSize;
    }

    public Rect GetCameraRect()
    {
        Camera camera = GetComponent<Camera>();
        float height = camera.orthographicSize * 2;
        float width = height * camera.aspect;
        float y = transform.position.y - height / 2f;
        float x = transform.position.x - width / 2f;
        return new Rect(x, y, width, height);
    }
}
