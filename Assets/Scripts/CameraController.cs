using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static readonly float PAN_SPEED_MULTIPLIER = 0.1f;
    private static readonly float ZOOM_MULTIPLIER = 1.1f;
    private static readonly float MIN_CAMERA_SIZE = 1;

    private static readonly float GRID_CURVES = 60;
    private static readonly float GRID_CURVATURE = 0.6f;
    private static readonly float GRID_STEP = 0.01f;
    private static readonly float GRID_DURATION = 6f;

    public bool DisplaySmallGrid;
    public bool DisplayLargeGrid;

    private List<List<Vector2>> SmallGrid;
    private List<List<Vector2>> LargeGrid;
    private float MaxCameraSize;
    private float MaxCameraDistance;

    public void Initialize(float radius)
    {
        MaxCameraSize = radius;
        MaxCameraDistance = radius;
        InitializeGrids(radius);
    }

    private void InitializeGrids(float radius)
    {
        LargeGrid = new List<List<Vector2>>();
        for (float phase = 0; phase < 2 * Mathf.PI; phase += 2 * Mathf.PI / GRID_CURVES)
        {
            LargeGrid.Add(PlotCurve(radius, GRID_CURVATURE, phase, 1, GRID_STEP, GRID_DURATION));
            LargeGrid.Add(PlotCurve(radius, GRID_CURVATURE, phase, -1, GRID_STEP, GRID_DURATION));
        }

        List<Vector2> circle = new List<Vector2>();
        for (float theta = 0; theta <= 2 * Mathf.PI; theta += 2 * Mathf.PI / 360)
        {
            circle.Add(new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta)));
        }
        LargeGrid.Add(circle);

        SmallGrid = new List<List<Vector2>>();
        for (float phase = 0; phase < 2 * Mathf.PI; phase += 2 * Mathf.PI / GRID_CURVES)
        {
            SmallGrid.Add(PlotCurve(radius, GRID_CURVATURE, phase, 1, GRID_STEP, GRID_DURATION / 10));
            SmallGrid.Add(PlotCurve(radius, GRID_CURVATURE, phase, -1, GRID_STEP, GRID_DURATION / 10));
        }

        circle = new List<Vector2>();
        for (float theta = 0; theta <= 2 * Mathf.PI; theta += 2 * Mathf.PI / 360)
        {
            circle.Add(new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta)));
        }
        SmallGrid.Add(circle);
    }

    private List<Vector2> PlotCurve(float a, float b, float phase, int direction, float gridStep, float gridDuration)
    {
        List<Vector2> points = new List<Vector2>();
        for (float t = 0; t < gridDuration; t += gridStep)
        {
            float x = a * Mathf.Exp(b * t) * Mathf.Cos(direction * (t + phase));
            float y = a * Mathf.Exp(b * t) * Mathf.Sin(direction * (t + phase));
            points.Add(new Vector2(x, y));
        }
        return points;
    }

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
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, MIN_CAMERA_SIZE, MaxCameraSize);


        Vector2 panInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 newPosition = transform.position + (Vector3) panInput * PAN_SPEED_MULTIPLIER * GetComponent<Camera>().orthographicSize;
        newPosition.x = Mathf.Clamp(newPosition.x, -MaxCameraDistance, MaxCameraDistance);
        newPosition.y = Mathf.Clamp(newPosition.y, -MaxCameraDistance, MaxCameraDistance);
        transform.position = newPosition;
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
        return 2 * camera.orthographicSize /camera.pixelHeight;
    }

    private void OnPostRender()
    {
        if (DisplaySmallGrid)
        {
            DrawGridLines(SmallGrid);
        }
        else if (DisplayLargeGrid)
        {
            DrawGridLines(LargeGrid);
        }
    }

    private void DrawGridLines(List<List<Vector2>> curves)
    {
        GL.Begin(GL.LINES);
        Material lineMat= new Material(Shader.Find("Sprites/Default"));
        lineMat.SetPass(0);
        GL.Color(new Color(0f, 1f, 0f, 1f));
        foreach (List<Vector2> curve in curves)
        {
            for (int i = 1; i < curve.Count; i++)
            {
                GL.Vertex3(curve[i - 1].x, curve[i - 1].y, 0);
                GL.Vertex3(curve[i].x, curve[i].y, 0);
            }
        }
        GL.End();
    }
}
