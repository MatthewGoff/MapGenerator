using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering_Shader
{
    public class ManyLayerController : MonoBehaviour
    {
        public float StartAlpha;
        public float EndAlpha;
        public int MaxObjectsPerShaderSegment;
        public int ColumnsPerShader;
        public int RowsPerShader;
        public int SpriteColumns;
        public int SpriteRows;
        public Color ExternalColor;
        public Color InternalColor;
        public GameObject MapCamera;
        public Texture2D Texture;
        public int TextureSize;
        public string ShaderName;
        public int SortingOrder;

        private Quadtree Contents;
        private Vector4[,][] BodyVectors;
        private float[,][] NumberOfBodies;
        private GameObject[,] Sprites;

        public void Initialize(Quadtree contents)
        {
            Contents = contents;
            InitializeSprites();
            InitializeShaderParameters();
        }

        private void InitializeShaderParameters()
        {
            BodyVectors = new Vector4[SpriteColumns, SpriteRows][];
            for (int x = 0; x < BodyVectors.GetLength(0); x++)
            {
                for (int y = 0; y < BodyVectors.GetLength(1); y++)
                {
                    BodyVectors[x, y] = new Vector4[ColumnsPerShader * RowsPerShader * MaxObjectsPerShaderSegment];
                }
            }

            NumberOfBodies = new float[SpriteColumns, SpriteRows][];
            for (int x = 0; x < NumberOfBodies.GetLength(0); x++)
            {
                for (int y = 0; y < NumberOfBodies.GetLength(1); y++)
                {
                    NumberOfBodies[x, y] = new float[ColumnsPerShader * RowsPerShader];
                }
            }
        }

        private void InitializeSprites()
        {
            Rect rect = new Rect(0, 0, TextureSize, TextureSize);

            Sprites = new GameObject[SpriteColumns, SpriteRows];
            for (int x = 0; x < Sprites.GetLength(0); x++)
            {
                for (int y = 0; y < Sprites.GetLength(1); y++)
                {
                    Sprites[x, y] = new GameObject("Sprite (" + x + ", " + y + ")");
                    Sprites[x, y].transform.SetParent(gameObject.transform);
                    Sprites[x, y].AddComponent<SpriteRenderer>();
                    Sprites[x, y].GetComponent<SpriteRenderer>().sprite = Sprite.Create(Texture, rect, Vector2.zero, TextureSize);
                    Sprites[x, y].GetComponent<SpriteRenderer>().sortingOrder = SortingOrder;
                    Material material = new Material(Shader.Find(ShaderName));
                    material.SetColor("_ExternalColor", ExternalColor);
                    material.SetColor("_InternalColor", InternalColor);
                    Sprites[x, y].GetComponent<SpriteRenderer>().material = material;
                }
            }
        }

        private void LateUpdate()
        {
            Rect cameraRect = MapCamera.GetComponent<CameraController>().GetCameraRect();
            float pixelWidth = MapCamera.GetComponent<CameraController>().GetPixelWidth();
            float masterAlpha = Mathf.Lerp(1, 0, (MapCamera.GetComponent<Camera>().orthographicSize - StartAlpha) / (EndAlpha - StartAlpha));
            Rect[,] spriteRects = Helpers.SubdivideRect(cameraRect, SpriteColumns, SpriteRows);
            for (int spriteColumn = 0; spriteColumn < SpriteColumns; spriteColumn++)
            {
                for (int spriteRow = 0; spriteRow < SpriteRows; spriteRow++)
                {
                    Sprites[spriteColumn, spriteRow].transform.position = spriteRects[spriteColumn, spriteRow].min;
                    Sprites[spriteColumn, spriteRow].transform.localScale = spriteRects[spriteColumn, spriteRow].size;
                    Rect[,] shaderRects = Helpers.SubdivideRect(spriteRects[spriteColumn, spriteRow], ColumnsPerShader, RowsPerShader);

                    for (int shaderColumn = 0; shaderColumn < ColumnsPerShader; shaderColumn++)
                    {
                        for (int shaderRow = 0; shaderRow < RowsPerShader; shaderRow++)
                        {
                            int offset = RowsPerShader * shaderColumn + shaderRow;
                            int count = 0;
                            Contents.GetLocalBodies(shaderRects[shaderColumn, shaderRow], BodyVectors[spriteColumn, spriteRow], MaxObjectsPerShaderSegment * offset, ref count);
                            NumberOfBodies[spriteColumn, spriteRow][offset] = count;                            
                        }
                    }

                    Sprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetVectorArray("bodies", BodyVectors[spriteColumn, spriteRow]);
                    Sprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetFloatArray("numberOfBodies", NumberOfBodies[spriteColumn, spriteRow]);
                    Sprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetFloat("pixelWidth", pixelWidth);
                    Sprites[spriteColumn, spriteRow].GetComponent<SpriteRenderer>().material.SetFloat("masterAlpha", masterAlpha);
                }
            }
        }
    }
}