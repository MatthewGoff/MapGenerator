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

    public float GetPixelWidth()
    {
        Camera camera = GetComponent<Camera>();
        return camera.pixelHeight / (2 * camera.orthographicSize);
    }

    private void OnPostRender()
    {
        //DrawSegments();
    }

    private void DrawSegments()
    {
        Rect rect = GetCameraRect();
        float horizontalSpacing = rect.width / 16;
        float verticalSpacing = rect.height / 9;

        Material lineMat = new Material(Shader.Find("Sprites/Default"));
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(new Color(1f, 0f, 0f, 1f));
        for (int column = 0; column < 16; column++)
        {
            GL.Vertex3(column * horizontalSpacing + rect.xMin, rect.yMin, 0);
            GL.Vertex3(column * horizontalSpacing + rect.xMin, rect.yMax, 0);
        }
        for (int row = 0; row < 9; row++)
        {
            GL.Vertex3(rect.xMin, row * verticalSpacing + rect.yMin, 0);
            GL.Vertex3(rect.xMax, row * verticalSpacing + rect.yMin, 0);
        }
        GL.End();
    }
}
