using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering_Sprite
{
    public class MapRenderer_Sprite : MonoBehaviour
    {
        public GameObject Contents;
        public GameObject MapCamera;
        public GameObject PlanetPrefab;
        public GameObject StarPrefab;
        public GameObject CelestialBodyPrefab;
        public GameObject EdgePrefab;

        public void Initialize(CelestialBodies.CelestialBody map, Network network)
        {
            MapCamera.GetComponent<CameraController>().Initialize(map.Radius);
            MapCamera.GetComponent<CameraController>().DisplayLargeGrid = true;

            foreach (CelestialBodies.Universe body in map.GetAllUniverses())
            {
                GameObject newGameObject = Instantiate(CelestialBodyPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)body.Type;
            }
            foreach (CelestialBodies.Expanse body in map.GetAllExpanses())
            {
                GameObject newGameObject = Instantiate(CelestialBodyPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)body.Type;
            }
            foreach (CelestialBodies.Galaxy body in map.GetAllGalaxies())
            {
                GameObject newGameObject = Instantiate(CelestialBodyPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)body.Type;
            }
            foreach (CelestialBodies.Sector body in map.GetAllSectors())
            {
                GameObject newGameObject = Instantiate(CelestialBodyPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)body.Type;
            }
            foreach (CelestialBodies.SolarSystem body in map.GetAllSolarSystems())
            {
                GameObject newGameObject = Instantiate(CelestialBodyPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)body.Type;
            }
            foreach (CelestialBodies.Star body in map.GetAllStars())
            {
                GameObject newGameObject = Instantiate(StarPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            foreach (CelestialBodies.Planet body in map.GetAllPlanets())
            {
                GameObject newGameObject = Instantiate(PlanetPrefab, body.Position, Quaternion.identity);
                newGameObject.transform.localScale = new Vector3(body.Radius * 2, body.Radius * 2, 1);
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }

            foreach (CelestialBodies.CelestialBody[] edge in network.Edges)
            {
                Vector2 position = (edge[0].Position + edge[1].Position) / 2;
                Vector2 difference = edge[0].Position - edge[1].Position;
                Vector3 scale = new Vector3(difference.magnitude, 0.25f, 1f);

                GameObject newGameObject = Instantiate(EdgePrefab, position, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, difference)));
                newGameObject.transform.localScale = scale;
                newGameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }

        public void OpenMap()
        {
            Contents.SetActive(true);
        }

        public void CloseMap()
        {
            Contents.SetActive(false);
        }
    }
}
