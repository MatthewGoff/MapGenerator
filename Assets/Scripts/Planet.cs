﻿using UnityEngine;

public class Planet : CircleCollider
{
    public static readonly float MIN_RADIUS = 0.5f;
    public static readonly float MAX_RADIUS = 2.0f;
    public static float AVERAGE_AREA
    {
        get
        {
            float averageRadius = (MIN_RADIUS + MAX_RADIUS) / 2f;
            return Mathf.PI * Mathf.Pow(averageRadius, 2f);
        }
    }

    private float Radius;
    private Vector2 LocalPosition;
    private GameObject GameObject;

	public Planet(Vector2 position)
    {
        Radius = Random.Range(0.5f, 2.0f);
        LocalPosition = position;
    }

    public void Realize(Vector2 parentPosition)
    {
        GameObject prefab = (GameObject) Resources.Load("Prefabs/Planet");
        GameObject = GameObject.Instantiate(prefab, parentPosition + LocalPosition, Quaternion.identity);
        GameObject.transform.localScale = new Vector3(Radius * 2f, Radius * 2f, 1f);
    }

    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }

    public float GetRadius()
    {
        return Radius;
    }

    public Vector2 GetLocalPosition()
    {
        return LocalPosition;
    }

    public void Push(Vector2 vector)
    {
        LocalPosition += vector;
    }
}