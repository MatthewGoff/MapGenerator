using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering
{
    public class MapRenderer : MonoBehaviour
    {
        private static readonly int MAX_OBJECTS_PER_SHADER = 100;
        private static readonly int COLUMNS_PER_SHADER = 3;
        private static readonly int ROWS_PER_SHADER = 3;
        private static readonly int SPRITE_COLUMNS = 6;
        private static readonly int SPRITE_ROWS = 3;
        public GameObject MapCamera;

        private float LargestPlanet;
        private float LargestStar;
        private float LargestSolarSystem;
        private float LargestSector;
        private float LargestGalaxy;
        private float LargestExpanse;
        private float LargestUniverse;

        private Quadtree PlanetQuadtree;
        private Vector4[,][] PlanetVectors;
        private float[,][] NumberOfPlanets;
        private GameObject[,] PlanetSprites;

        public void Initialize(CelestialBodies.CelestialBody map)
        {
            InitializePlanetSegments();
            //InitializeUniverseSegments();
            InitializeQuadtrees(map);
            InitializeShaderParameters();
        }

        private void InitializeShaderParameters()
        {
            PlanetVectors = new Vector4[SPRITE_COLUMNS, SPRITE_ROWS][];
            for (int x = 0; x < PlanetVectors.GetLength(0); x++)
            {
                for (int y = 0; y < PlanetVectors.GetLength(1); y++)
                {
                    PlanetVectors[x, y] = new Vector4[COLUMNS_PER_SHADER * ROWS_PER_SHADER * MAX_OBJECTS_PER_SHADER];
                }
            }

            NumberOfPlanets = new float[SPRITE_COLUMNS, SPRITE_ROWS][];
            for (int x = 0; x < NumberOfPlanets.GetLength(0); x++)
            {
                for (int y = 0; y < NumberOfPlanets.GetLength(1); y++)
                {
                    NumberOfPlanets[x, y] = new float[COLUMNS_PER_SHADER * ROWS_PER_SHADER];
                }
            }
        }

        private void InitializeQuadtrees(CelestialBodies.CelestialBody map)
        {
            PlanetQuadtree = new Quadtree(map.Radius, -map.Radius, -map.Radius, map.Radius);
            List<CelestialBodies.Planet> planets = map.GetAllPlanets(ref LargestPlanet);
            foreach (CelestialBodies.Planet planet in planets)
            {
                PlanetQuadtree.Insert(planet);
            }
        }

        private void InitializePlanetSegments()
        {
            Texture2D texture = (Texture2D)Resources.Load("Sprites/PlanetSprite");
            Rect rect = new Rect(0, 0, 512, 512);

            PlanetSprites = new GameObject[SPRITE_COLUMNS, SPRITE_ROWS];
            for (int x = 0; x < PlanetSprites.GetLength(0); x++)
            {
                for (int y = 0; y < PlanetSprites.GetLength(1); y++)
                {
                    PlanetSprites[x, y] = new GameObject("Planet Sprite ("+x+","+y+")");
                    PlanetSprites[x, y].transform.SetParent(gameObject.transform);
                    PlanetSprites[x, y].AddComponent<SpriteRenderer>();
                    PlanetSprites[x, y].GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, Vector2.zero, 512);
                    PlanetSprites[x, y].GetComponent<SpriteRenderer>().sortingOrder = 0;
                    Material material = new Material(Shader.Find("Custom/PlanetShader"));
                    PlanetSprites[x, y].GetComponent<SpriteRenderer>().material = material;
                }
            }
        }

        public void OpenMap()
        {
            gameObject.SetActive(true);
        }

        public void CloseMap()
        {
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            UpdatePlanetSegments();
        }

        private void UpdatePlanetSegments()
        {
            Rect rect = MapCamera.GetComponent<CameraController>().GetCameraRect();
            float pixelWidth = MapCamera.GetComponent<CameraController>().GetPixelWidth();
            Rect[,] spriteRects = Helpers.SubdivideRect(rect, SPRITE_COLUMNS, SPRITE_ROWS);

            for (int spriteColumn = 0; spriteColumn < SPRITE_COLUMNS; spriteColumn++)
            {
                for (int spriteRow = 0; spriteRow < SPRITE_ROWS; spriteRow++)
                {
                    PlanetSprites[spriteColumn, spriteRow].transform.position = spriteRects[spriteColumn, spriteRow].min;
                    PlanetSprites[spriteColumn, spriteRow].transform.localScale = spriteRects[spriteColumn, spriteRow].size;
                    Rect[,] shaderRects = Helpers.SubdivideRect(spriteRects[spriteColumn, spriteRow], COLUMNS_PER_SHADER, ROWS_PER_SHADER);

                    for (int shaderColumn = 0; shaderColumn < COLUMNS_PER_SHADER; shaderColumn++)
                    {
                        for (int shaderRow = 0; shaderRow < ROWS_PER_SHADER; shaderRow++)
                        {
                            int offset = ROWS_PER_SHADER * shaderColumn + shaderRow;
                            int count = 0;
                            PlanetQuadtree.GetLocalBodies(shaderRects[shaderColumn, shaderRow], PlanetVectors[spriteColumn, spriteRow], MAX_OBJECTS_PER_SHADER * offset, ref count);
                            NumberOfPlanets[spriteColumn, spriteRow][offset] = count;
                        }
                    }

                    PlanetSprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetVectorArray("planets", PlanetVectors[spriteColumn, spriteRow]);
                    PlanetSprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetFloatArray("numberOfPlanets", NumberOfPlanets[spriteColumn, spriteRow]);
                    PlanetSprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetFloat("pixelWidth", pixelWidth);
                }
            }
        }
    }
}