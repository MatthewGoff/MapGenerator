using System.Collections.Generic;
using UnityEngine;

namespace CelestialBodies
{
    public abstract class CelestialBody
    {
        public readonly CelestialBodyType Type;
        public readonly Vector2 Position;
        public readonly float Radius;

        public readonly Expanse[] Expanses;
        public readonly Group[] Groups;
        public readonly Galaxy[] Galaxies;
        public readonly Sector[] Sectors;
        public readonly Cloud[] Clouds;
        public readonly SolarSystem[] SolarSystems;
        public readonly Star[] Stars;
        public readonly Planet[] Planets;

        protected CelestialBody(MapGenerator.Containers.Container container)
        {
            Type = container.Type;
            Position = container.GlobalPosition;
            Radius = container.Radius;
            
            if (container.Expanses != null)
            {
                Expanses = new Expanse[container.Expanses.Length];
                for (int i = 0; i < Expanses.Length; i++)
                {
                    Expanses[i] = new Expanse(container.Expanses[i]);
                }
            }
            if (container.Groups != null)
            {
                Groups = new Group[container.Groups.Length];
                for (int i = 0; i < Groups.Length; i++)
                {
                    Groups[i] = new Group(container.Groups[i]);
                }
            }
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

        /// <summary>
        /// Get a list of all the contents in the body, including the body
        /// itself. Also update the provided dictionary with the largest body
        /// of each type.
        /// </summary>
        /// <param name="largestRadii"></param>
        /// <returns></returns>
        public List<CelestialBody> GetAllContents(Dictionary<CelestialBodyType, float> largestRadii)
        {
            largestRadii[Type] = Mathf.Max(largestRadii[Type], Radius);
            List<CelestialBody> list = new List<CelestialBody>
            {
                this
            };

            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    list.AddRange(Expanses[i].GetAllContents(largestRadii));
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    list.AddRange(Groups[i].GetAllContents(largestRadii));
                }
            }
            if (Galaxies != null)
            {
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    list.AddRange(Galaxies[i].GetAllContents(largestRadii));
                }
            }
            if (Sectors != null)
            {
                for (int i = 0; i < Sectors.Length; i++)
                {
                    list.AddRange(Sectors[i].GetAllContents(largestRadii));
                }
            }
            if (Clouds != null)
            {
                for (int i = 0; i < Clouds.Length; i++)
                {
                    list.AddRange(Clouds[i].GetAllContents(largestRadii));
                }
            }
            if (SolarSystems != null)
            {
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    list.AddRange(SolarSystems[i].GetAllContents(largestRadii));
                }
            }
            if (Stars != null)
            {
                for (int i = 0; i < Stars.Length; i++)
                {
                    largestRadii[CelestialBodyType.Star] = Mathf.Max(largestRadii[CelestialBodyType.Star], Stars[i].Radius);
                    list.Add(Stars[i]);
                }
            }
            if (Planets != null)
            {
                for (int i = 0; i < Planets.Length; i++)
                {
                    largestRadii[CelestialBodyType.Planet] = Mathf.Max(largestRadii[CelestialBodyType.Planet], Planets[i].Radius);
                    list.Add(Planets[i]);
                }
            }

            return list;
        }

        public List<Expanse> GetAllExpanses()
        {
            List<Expanse> expanses = new List<Expanse>();
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    expanses.AddRange(Expanses[i].GetAllExpanses());
                }
            }
            return expanses;
        }

        public List<Group> GetAllGroups()
        {
            List<Group> groups = new List<Group>();
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    groups.AddRange(Expanses[i].GetAllGroups());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    groups.AddRange(Groups[i].GetAllGroups());
                }
            }
            return groups;
        }

        public List<Galaxy> GetAllGalaxies()
        {
            List<Galaxy> galaxies = new List<Galaxy>();
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    galaxies.AddRange(Expanses[i].GetAllGalaxies());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    galaxies.AddRange(Groups[i].GetAllGalaxies());
                }
            }
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
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    sectors.AddRange(Expanses[i].GetAllSectors());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    sectors.AddRange(Groups[i].GetAllSectors());
                }
            }
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
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    clouds.AddRange(Expanses[i].GetAllClouds());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    clouds.AddRange(Groups[i].GetAllClouds());
                }
            }
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
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    solarSystems.AddRange(Expanses[i].GetAllSolarSystems());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    solarSystems.AddRange(Groups[i].GetAllSolarSystems());
                }
            }
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
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    stars.AddRange(Expanses[i].GetAllStars());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    stars.AddRange(Groups[i].GetAllStars());
                }
            }
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
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    planets.AddRange(Expanses[i].GetAllPlanets());
                }
            }
            if (Groups != null)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    planets.AddRange(Groups[i].GetAllPlanets());
                }
            }
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