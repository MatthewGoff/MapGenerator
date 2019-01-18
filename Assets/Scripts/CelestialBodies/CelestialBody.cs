using System.Collections.Generic;
using UnityEngine;

namespace CelestialBodies
{
    public abstract class CelestialBody
    {
        public readonly Vector2 Position;
        public readonly float Radius;

        public readonly Galaxy[] Galaxies;
        public readonly Sector[] Sectors;
        public readonly Cloud[] Clouds;
        public readonly SolarSystem[] SolarSystems;
        public readonly Star[] Stars;
        public readonly Planet[] Planets;

        protected CelestialBody(MapGenerator.Containers.Container container)
        {
            Position = container.GlobalPosition;
            Radius = container.Radius;

            if (container.Galaxies != null)
            {
                Galaxies = new Galaxy[container.Galaxies.Length];
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    Galaxies[i] = new Galaxy(container.Galaxies[i]);
                }
            }
            if (container.Sectors != null)
            {
                Sectors = new Sector[container.Sectors.Length];
                for (int i = 0; i < Sectors.Length; i++)
                {
                    Sectors[i] = new Sector(container.Sectors[i]);
                }
            }
            if (container.Clouds != null)
            {
                Clouds = new Cloud[container.Clouds.Length];
                for (int i = 0; i < Clouds.Length; i++)
                {
                    Clouds[i] = new Cloud(container.Clouds[i]);
                }
            }
            if (container.SolarSystems != null)
            {
                SolarSystems = new SolarSystem[container.SolarSystems.Length];
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    SolarSystems[i] = new SolarSystem(container.SolarSystems[i]);
                }
            }
            if (container.Stars != null)
            {
                Stars = new Star[container.Stars.Length];
                for (int i = 0; i < Stars.Length; i++)
                {
                    Stars[i] = new Star(container.Stars[i]);
                }
            }
            if (container.Planets != null)
            {
                Planets = new Planet[container.Planets.Length];
                for (int i = 0; i < Planets.Length; i++)
                {
                    Planets[i] = new Planet(container.Planets[i]);
                }
            }
        }

        public List<Galaxy> GetAllGalaxies()
        {
            List<Galaxy> galaxies = new List<Galaxy>();
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    galaxies.Add(Galaxies[i]);
                }
            }
            return galaxies;
        }

        public List<Sector> GetAllSectors()
        {
            List<Sector> sectors = new List<Sector>();
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    sectors.AddRange(Galaxies[i].GetAllSectors());
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    sectors.Add(Sectors[i]);
                }
            }
            return sectors;
        }

        public List<Cloud> GetAllClouds()
        {
            List<Cloud> clouds = new List<Cloud>();
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    clouds.AddRange(Galaxies[i].GetAllClouds());
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    clouds.AddRange(Sectors[i].GetAllClouds());
                }
            }
            if (Clouds != null)
            {
                for (int i = 0; i < Clouds.Length; i++)
                {
                    clouds.Add(Clouds[i]);
                }
            }
            return clouds;
        }

        public List<SolarSystem> GetAllSolarSystems()
        {
            List<SolarSystem> solarSystems = new List<SolarSystem>();
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    solarSystems.AddRange(Galaxies[i].GetAllSolarSystems());
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    solarSystems.AddRange(Sectors[i].GetAllSolarSystems());
                }
            }
            if (Clouds != null)
            {
                for (int i = 0; i < Clouds.Length; i++)
                {
                    solarSystems.AddRange(Clouds[i].GetAllSolarSystems());
                }
            }
            if (SolarSystems != null)
            {
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    solarSystems.Add(SolarSystems[i]);
                }
            }
            return solarSystems;
        }

        public List<Star> GetAllStars()
        {
            List<Star> stars = new List<Star>();
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    stars.AddRange(Galaxies[i].GetAllStars());
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    stars.AddRange(Sectors[i].GetAllStars());
                }
            }
            if (Clouds != null)
            {
                for (int i = 0; i < Clouds.Length; i++)
                {
                    stars.AddRange(Clouds[i].GetAllStars());
                }
            }
            if (SolarSystems != null)
            {
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    stars.AddRange(SolarSystems[i].GetAllStars());
                }
            }
            if (Stars != null)
            {
                for (int i = 0; i < Stars.Length; i++)
                {
                    stars.Add(Stars[i]);
                }
            }
            return stars;
        }
        
        public List<Planet> GetAllPlanets()
        {
            List<Planet> planets = new List<Planet>();
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    planets.AddRange(Galaxies[i].GetAllPlanets());
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    planets.AddRange(Sectors[i].GetAllPlanets());
                }
            }
            if (Clouds != null)
            {
                for (int i = 0; i < Clouds.Length; i++)
                {
                    planets.AddRange(Clouds[i].GetAllPlanets());
                }
            }
            if (SolarSystems != null)
            {
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    planets.AddRange(SolarSystems[i].GetAllPlanets());
                }
            }
            if (Planets != null)
            {
                for (int i = 0; i < Planets.Length; i++)
                {
                    planets.Add(Planets[i]);
                }
            }
            return planets;
        }
    }
}