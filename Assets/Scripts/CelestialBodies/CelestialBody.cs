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
        public readonly Galaxy[] Galaxies;
        public readonly Sector[] Sectors;
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
    }
}