using CelestialBodies;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        private UniverseRenderer[] UniverseRenderers;
        private ExpanseRenderer[] ExpanseRenderers;
        private GroupRenderer[] GroupRenderers;
        private GalaxyRenderer[] GalaxyRenderers;
        private SectorRenderer[] SectorRenderers;
        private CloudRenderer[] CloudRenderers;
        private SolarSystemRenderer[] SolarSystemRenderers;
        private StarRenderer[] StarRenderers;
        private PlanetRenderer[] PlanetRenderers;

        private bool Initialized = false;

        public void Initialize()
        {
            CelestialBody celestialBody = GameManager.Instance.Map;
            CelestialBodyType mapSize = GameManager.Instance.MapSize;
            CreateGalaxyRenderers(celestialBody);
            CreateSectorRenderers(celestialBody);
            CreateCloudRenderers(celestialBody);
            CreateSolarSystemRenderers(celestialBody);
            CreateStarRenderers(celestialBody);
            CreatePlanetRenderers(celestialBody);

            if (mapSize == CelestialBodyType.Universe)
            {
                UniverseRenderers = new UniverseRenderer[] { new UniverseRenderer((Universe)celestialBody) };
            }
            else if (mapSize == CelestialBodyType.Expanse)
            {
                ExpanseRenderers = new ExpanseRenderer[] { new ExpanseRenderer((Expanse)celestialBody) };
            }
            else if (mapSize == CelestialBodyType.Group)
            {
                GroupRenderers = new GroupRenderer[] { new GroupRenderer((Group)celestialBody) };
            }
            else if (mapSize == CelestialBodyType.Galaxy)
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

            Initialized = true;
        }

        private void CreateExpanseRenderers(CelestialBody celestialBody)
        {
            List<Expanse> expanses = celestialBody.GetAllExpanses();
            ExpanseRenderers = new ExpanseRenderer[expanses.Count];
            for (int i = 0; i < ExpanseRenderers.Length; i++)
            {
                ExpanseRenderers[i] = new ExpanseRenderer(expanses[i]);
            }
        }

        private void CreateGroupRenderers(CelestialBody celestialBody)
        {
            List<Group> groups = celestialBody.GetAllGroups();
            GroupRenderers = new GroupRenderer[groups.Count];
            for (int i = 0; i < GroupRenderers.Length; i++)
            {
                GroupRenderers[i] = new GroupRenderer(groups[i]);
            }
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
            if (!Initialized)
            {
                return;
            }
            for (int i = 0; i < SolarSystemRenderers.Length; i++)
            {
                //SolarSystemRenderers[i].Update();
            }
            for (int i = 0; i < StarRenderers.Length; i++)
            {
                //StarRenderers[i].Update();
            }
            for (int i = 0; i < PlanetRenderers.Length; i++)
            {
                //PlanetRenderers[i].Update();
            }
        }
    }
}