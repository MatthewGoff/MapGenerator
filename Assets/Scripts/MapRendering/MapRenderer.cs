using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering
{
    public class MapRenderer : MonoBehaviour
    {
        public GameObject MapCamera;
        public GameObject PlanetsSprite;

        private float LargestPlanet;

        public void Initialize(CelestialBodies.CelestialBody Map)
        {
            PlanetsSprite.GetComponent<PlanetsSpriteController>().Initialize(Map.GetAllPlanets(ref LargestPlanet));
        }

        public void OpenMap()
        {
            gameObject.SetActive(true);
        }
        
        public void CloseMap()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateSprites();
        }

        private void UpdateSprites()
        {
            Rect cameraRect = MapCamera.GetComponent<CameraController>().GetCameraRect();
            Vector3 scale = new Vector3(cameraRect.width, cameraRect.height, 1f);
            PlanetsSprite.transform.localScale = scale;
        }
    }
}