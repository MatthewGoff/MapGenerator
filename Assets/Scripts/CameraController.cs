﻿using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static readonly float PAN_SPEED_MULTIPLIER = 0.05f;
    private static readonly float ZOOM_MULTIPLIER = 1.2f;

    private void Update()
    {
        Vector2 panInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += (Vector3)panInput * PAN_SPEED_MULTIPLIER * GetComponent<Camera>().orthographicSize;

        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput > 0)
        {
            GetComponent<Camera>().orthographicSize /= ZOOM_MULTIPLIER;
        }
        else if (zoomInput < 0)
        {
            GetComponent<Camera>().orthographicSize *= ZOOM_MULTIPLIER;
        }

    }

    private void OnPostRender()
    {
        if (GameManager.Instance.LastQuadtree != null)
        {
            //DrawQuadtree();
        }
    }

    private void DrawQuadtree()
    {
        Material lineMat = new Material(Shader.Find("Sprites/Default"));
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(new Color(0f, 1f, 0f, 1f));

        foreach (Vector2 point in GameManager.Instance.LastQuadtree.GetLinePoints())
        {
            GL.Vertex(point);
        }

        GL.End();
    }
}
