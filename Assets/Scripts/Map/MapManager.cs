using System.Collections.Generic;
using UnityEngine;
using CelestialBodies;

namespace Map
{
    public class MapManager : MonoBehaviour
    {

        private GalaxyRenderer[] GalaxyRenderers;
        private SectorRenderer[] SectorRenderers;
        private CloudRenderer[] CloudRenderers;
        private SolarSystemRenderer[] SolarSystemRenderers;
        private StarRenderer[] StarRenderers;
        private PlanetRenderer[] PlanetRenderers;

        public void Initialize()
        {
            CelestialBody celestialBody = GameManager.Instance.Map;
            CelestialBodyType mapSize = GameManager.Instance.MapSize;
            if (mapSize == CelestialBodyType.Galaxy)
            {
                GalaxyRenderers = new GalaxyRenderer[] { new GalaxyRenderer((Galaxy)celestialBody) };
            }
            else if (mapSize == CelestialBodyType.Sector)
            {
                SectorRenderers = new SectorRenderer[] { new SectorRenderer((Sector)celestialBody) };
            }
            else if (mapSize == CelestialBodyType.Cloud)
            {
                CloudRenderers = new CloudRenderer[] { new CloudRenderer((Cloud)celestialBody) };
            }
            else if (mapSize == CelestialBodyType.SolarSystem)
            {
                SolarSystemRenderers = new SolarSystemRenderer[] { new SolarSystemRenderer((SolarSystem)celestialBody) };
            }
            CreateGalaxyRenderers(celestialBody);
            CreateSectorRenderers(celestialBody);
            CreateCloudRenderers(celestialBody);
            CreateSolarSystemRenderers(celestialBody);
            CreateStarRenderers(celestialBody);
            CreatePlanetRenderers(celestialBody);
        }

        private void CreateGalaxyRenderers(CelestialBody celestialBody)
        {
            List<Galaxy> galaxies = celestialBody.GetAllGalaxies();
            GalaxyRenderers = new GalaxyRenderer[galaxies.Count];
            for (int i = 0; i < GalaxyRenderers.Length; i++)
            {
                GalaxyRenderers[i] = new GalaxyRenderer(galaxies[i]);
            }
        }

        private void CreateSectorRenderers(CelestialBody celestialBody)
        {
            List<Sector> sectors = celestialBody.GetAllSectors();
            SectorRenderers = new SectorRenderer[sectors.Count];
            for (int i = 0; i < SectorRenderers.Length; i++)
            {
                SectorRenderers[i] = new SectorRenderer(sectors[i]);
            }
        }

        private void CreateCloudRenderers(CelestialBody celestialBody)
        {
            List<Cloud> clouds = celestialBody.GetAllClouds();
            CloudRenderers = new CloudRenderer[clouds.Count];
            for (int i = 0; i < CloudRenderers.Length; i++)
            {
                CloudRenderers[i] = new CloudRenderer(clouds[i]);
            }
        }

        private void CreateSolarSystemRenderers(CelestialBody celestialBody)
        {
            List<SolarSystem> solarSystems = celestialBody.GetAllSolarSystems();
            SolarSystemRenderers = new SolarSystemRenderer[solarSystems.Count];
            for (int i = 0; i < SolarSystemRenderers.Length; i++)
            {
                SolarSystemRenderers[i] = new SolarSystemRenderer(solarSystems[i]);
            }
        }

        private void CreateStarRenderers(CelestialBody celestialBody)
        {
            List<Star> stars = celestialBody.GetAllStars();
            StarRenderers = new StarRenderer[stars.Count];
            for (int i = 0; i < StarRenderers.Length; i++)
            {
                StarRenderers[i] = new StarRenderer(stars[i]);
            }
        }

        private void CreatePlanetRenderers(CelestialBody celestialBody)
        {
            List<Planet> planets = celestialBody.GetAllPlanets();
            PlanetRenderers = new PlanetRenderer[planets.Count];
            for (int i = 0; i < PlanetRenderers.Length; i++)
            {
                PlanetRenderers[i] = new PlanetRenderer(planets[i]);
            }
        }

        private void Update()
        {
            for (int i = 0; i < GalaxyRenderers.Length; i++)
            {
                //GalaxyRenderers[i].Update();
            }
        }
    }
}