using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering_Shader
{
    public class MapRenderer_Shader : MonoBehaviour
    {
        private static readonly float LOD0to1_THRESHOLD = 195;
        private static readonly float LOD1to2_THRESHOLD = 900;
        private static readonly float LOD2to3_THRESHOLD = 3000;

        public GameObject MapCamera;

        public GameObject SmallBodyLayer_LOD0;
        public GameObject SolarSystemLayer_LOD0;
        public GameObject SolarSystemLayer_LOD1;
        public GameObject SectorLayer_LOD1;
        public GameObject SectorLayer_LOD2;
        public GameObject GalaxyLayer_LOD2;
        public GameObject GalaxyLayer_LOD3;
        public GameObject ExpanseLayer_LOD3;
        public GameObject UniverseLayer_LOD3;

        public void Initialize(CelestialBodies.CelestialBody map, Network network)
        {
            MapCamera.GetComponent<CameraController>().Initialize(map.Radius);
            InitializeSmallBodyLayer(map);
            InitializeSolarSystemLayers(map);
            InitializeSectorLayers(map);
            InitializeGalaxyLayers(map);
            InitializeExpanseLayer(map);
            InitializeUniverseLayer(map);
        }

        private void InitializeSmallBodyLayer(CelestialBodies.CelestialBody map)
        {
            Quadtree quadtree = new Quadtree(map.Radius, -map.Radius, -map.Radius, map.Radius);
            List<CelestialBodies.Planet> planets = map.GetAllPlanets();
            foreach (CelestialBodies.Planet planet in planets)
            {
                quadtree.Insert(planet);
            }
            List<CelestialBodies.Star> stars = map.GetAllStars();
            foreach (CelestialBodies.Star star in stars)
            {
                quadtree.Insert(star);
            }
            SmallBodyLayer_LOD0.GetComponent<ManyLayerController>().Initialize(quadtree);
        }

        private void InitializeSolarSystemLayers(CelestialBodies.CelestialBody map)
        {
            Quadtree quadtree = new Quadtree(map.Radius, -map.Radius, -map.Radius, map.Radius);
            List<CelestialBodies.SolarSystem> solarSystems = map.GetAllSolarSystems();
            foreach (CelestialBodies.SolarSystem solarSystem in solarSystems)
            {
                quadtree.Insert(solarSystem);
            }
            SolarSystemLayer_LOD0.GetComponent<ManyLayerController>().Initialize(quadtree);
            SolarSystemLayer_LOD1.GetComponent<ManyLayerController>().Initialize(quadtree);
        }

        private void InitializeSectorLayers(CelestialBodies.CelestialBody map)
        {
            Quadtree quadtree = new Quadtree(map.Radius, -map.Radius, -map.Radius, map.Radius);
            List<CelestialBodies.Sector> sectors = map.GetAllSectors();
            foreach (CelestialBodies.Sector sector in sectors)
            {
                quadtree.Insert(sector);
            }
            SectorLayer_LOD1.GetComponent<ManyLayerController>().Initialize(quadtree);
            SectorLayer_LOD2.GetComponent<ManyLayerController>().Initialize(quadtree);
        }

        private void InitializeGalaxyLayers(CelestialBodies.CelestialBody map)
        {
            Quadtree quadtree = new Quadtree(map.Radius, -map.Radius, -map.Radius, map.Radius);
            List<CelestialBodies.Galaxy> galaxies = map.GetAllGalaxies();
            foreach (CelestialBodies.Galaxy galaxy in galaxies)
            {
                quadtree.Insert(galaxy);
            }
            GalaxyLayer_LOD2.GetComponent<ManyLayerController>().Initialize(quadtree);
            GalaxyLayer_LOD3.GetComponent<ManyLayerController>().Initialize(quadtree);
        }

        private void InitializeExpanseLayer(CelestialBodies.CelestialBody map)
        {
            List<CelestialBodies.CelestialBody> layerList = new List<CelestialBodies.CelestialBody>();
            foreach(CelestialBodies.Expanse expanse in map.GetAllExpanses())
            {
                layerList.Add(expanse);
            }
            ExpanseLayer_LOD3.GetComponent<FewLayerController>().Initialize(layerList);
        }

        private void InitializeUniverseLayer(CelestialBodies.CelestialBody map)
        {
            List<CelestialBodies.CelestialBody> layerList = new List<CelestialBodies.CelestialBody>();
            foreach (CelestialBodies.Universe universe in map.GetAllUniverses())
            {
                layerList.Add(universe);
            }
            UniverseLayer_LOD3.GetComponent<FewLayerController>().Initialize(layerList);
        }

        private void Update()
        {
            float cameraSize = MapCamera.GetComponent<Camera>().orthographicSize;
            if (cameraSize <= LOD0to1_THRESHOLD)
            {
                SmallBodyLayer_LOD0.SetActive(true);
                SolarSystemLayer_LOD0.SetActive(true);
                SolarSystemLayer_LOD1.SetActive(false);
                SectorLayer_LOD1.SetActive(true);
                SectorLayer_LOD2.SetActive(false);
                GalaxyLayer_LOD2.SetActive(true);
                GalaxyLayer_LOD3.SetActive(false);
                ExpanseLayer_LOD3.SetActive(true);
                UniverseLayer_LOD3.SetActive(true);
                MapCamera.GetComponent<CameraController>().DisplaySmallGrid = false;
                MapCamera.GetComponent<CameraController>().DisplayLargeGrid = false;
            }
            else if (cameraSize <= LOD1to2_THRESHOLD)
            {
                SmallBodyLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD1.SetActive(true);
                SectorLayer_LOD1.SetActive(true);
                SectorLayer_LOD2.SetActive(false);
                GalaxyLayer_LOD2.SetActive(true);
                GalaxyLayer_LOD3.SetActive(false);
                ExpanseLayer_LOD3.SetActive(true);
                UniverseLayer_LOD3.SetActive(true);
                MapCamera.GetComponent<CameraController>().DisplaySmallGrid = false;
                MapCamera.GetComponent<CameraController>().DisplayLargeGrid = false;
            }
            else if (cameraSize <= LOD2to3_THRESHOLD)
            {
                SmallBodyLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD1.SetActive(false);
                SectorLayer_LOD1.SetActive(false);
                SectorLayer_LOD2.SetActive(true);
                GalaxyLayer_LOD2.SetActive(true);
                GalaxyLayer_LOD3.SetActive(false);
                ExpanseLayer_LOD3.SetActive(true);
                UniverseLayer_LOD3.SetActive(true);
                MapCamera.GetComponent<CameraController>().DisplaySmallGrid = false;
                MapCamera.GetComponent<CameraController>().DisplayLargeGrid = false;
            }
            else
            {
                SmallBodyLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD1.SetActive(false);
                SectorLayer_LOD1.SetActive(false);
                SectorLayer_LOD2.SetActive(false);
                GalaxyLayer_LOD2.SetActive(false);
                GalaxyLayer_LOD3.SetActive(true);
                ExpanseLayer_LOD3.SetActive(true);
                UniverseLayer_LOD3.SetActive(true);
                MapCamera.GetComponent<CameraController>().DisplaySmallGrid = false;
                MapCamera.GetComponent<CameraController>().DisplayLargeGrid = true;

                SmallBodyLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD0.SetActive(false);
                SolarSystemLayer_LOD1.SetActive(false);
                SectorLayer_LOD2.SetActive(false);

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
    }
}
