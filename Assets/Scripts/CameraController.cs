using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static readonly float PAN_SPEED_MULTIPLIER = 0.1f;
    private static readonly float ZOOM_MULTIPLIER = 1.2f;

    public GameObject MapSprite;

    private float UPP;
    private Vector2 Position;

    private void Awake()
    {
        UpdateUPP();
        Position = transform.position;
    }

    private void Update()
    {
        ApplyInputs();
        UpdateMap();
    }

    private void UpdateUPP()
    {
        UPP = (GetComponent<Camera>().orthographicSize * 2) / GetComponent<Camera>().pixelHeight;
    }

    private void ApplyInputs()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput > 0)
        {
            GetComponent<Camera>().orthographicSize /= ZOOM_MULTIPLIER;
            UpdateUPP();
        }
        else if (zoomInput < 0)
        {
            GetComponent<Camera>().orthographicSize *= ZOOM_MULTIPLIER;
            UpdateUPP();
        }
        
        Vector2 panInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Position += panInput * PAN_SPEED_MULTIPLIER * GetComponent<Camera>().orthographicSize;
        float x = Mathf.RoundToInt(Position.x / UPP) * UPP;
        float y = Mathf.RoundToInt(Position.y / UPP) * UPP;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void UpdateMap()
    {
        Camera camera = GetComponent<Camera>();
        MapSprite.transform.localScale = new Vector2(camera.orthographicSize * 2, camera.orthographicSize * 2);

        float height = camera.orthographicSize * 2;
        float width = height * camera.aspect;
        float y = transform.position.y - height / 2f;
        float x = transform.position.x - width / 2f;
        Rect worldRect = new Rect(x, y, width, height);
        Rect screenRect = new Rect(0, 0, camera.pixelWidth, camera.pixelHeight);
        if (GameManager.Instance.MapRenderer != null)
        {
            Texture2D texture = GameManager.Instance.MapRenderer.RenderMap(worldRect);
            MapSprite.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, screenRect, new Vector2(0.5f, 0.5f), camera.pixelHeight);
        }
    }

    private void OnPostRender()
    {
        if (GameManager.Instance.Quadtree != null && GameManager.Instance.DrawQuadtree)
        {
            DrawQuadtree();
        }
    }

    private void DrawQuadtree()
    {
        Material lineMat = new Material(Shader.Find("Sprites/Default"));
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(new Color(0f, 1f, 0f, 1f));

        foreach (Vector2 point in GameManager.Instance.Quadtree.GetLinePoints())
        {
            GL.Vertex(point);
        }

        GL.End();
    }
}
