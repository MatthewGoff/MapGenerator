using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering
{
    public class MapRenderer : MonoBehaviour
    {
        public GameObject MapCamera;
        public GameObject PlanetsSprite;
        public GameObject StarsSprite;

        private float LargestPlanet;
        private float LargestStar;

        public void Initialize(CelestialBodies.CelestialBody map)
        {
            PlanetsSprite.GetComponent<PlanetsSpriteController>().Initialize(map.GetAllPlanets(ref LargestPlanet));
            StarsSprite.GetComponent<StarsSpriteController>().Initialize(map.GetAllStars(ref LargestStar));
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
            scale *= 1.2f;
            PlanetsSprite.transform.localScale = scale;
            StarsSprite.transform.localScale = scale;
        }
    }
}