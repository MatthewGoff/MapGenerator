using System;
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

        protected CelestialBody(byte[] bytes, int startIndex)
        {
            int offset = 4;
            Type = (CelestialBodyType)BitConverter.ToInt32(bytes, startIndex + offset);
            offset += 4;
            float x = BitConverter.ToSingle(bytes, startIndex + offset);
            offset += 4;
            float y = BitConverter.ToSingle(bytes, startIndex + offset);
            offset += 4;
            Position = new Vector2(x, y);
            Radius = BitConverter.ToSingle(bytes, startIndex + offset);
            offset += 4;

            Expanses = DeserializeExpanses(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);
            Galaxies = DeserializeGalaxies(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);
            Sectors = DeserializeSectors(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);
            SolarSystems = DeserializeSolarSystems(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);
            Stars = DeserializeStars(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);
            Planets = DeserializePlanets(bytes, startIndex + offset);
        }

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
            else
            {
                Expanses = new Expanse[0];
            }
            if (container.Galaxies != null)
            {
                Galaxies = new Galaxy[container.Galaxies.Length];
                for (int i = 0; i < Galaxies.Length; i++)
                {
                    Galaxies[i] = new Galaxy(container.Galaxies[i]);
                }
            }
            else
            {
                Galaxies = new Galaxy[0];
            }
            if (container.Sectors != null)
            {
                Sectors = new Sector[container.Sectors.Length];
                for (int i = 0; i < Sectors.Length; i++)
                {
                    Sectors[i] = new Sector(container.Sectors[i]);
                }
            }
            else
            {
                Sectors = new Sector[0];
            }
            if (container.SolarSystems != null)
            {
                SolarSystems = new SolarSystem[container.SolarSystems.Length];
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    SolarSystems[i] = new SolarSystem(container.SolarSystems[i]);
                }
            }
            else
            {
                SolarSystems = new SolarSystem[0];
            }
            if (container.Stars != null)
            {
                Stars = new Star[container.Stars.Length];
                for (int i = 0; i < Stars.Length; i++)
                {
                    Stars[i] = new Star(container.Stars[i]);
                }
            }
            else
            {
                Stars = new Star[0];
            }
            if (container.Planets != null)
            {
                Planets = new Planet[container.Planets.Length];
                for (int i = 0; i < Planets.Length; i++)
                {
                    Planets[i] = new Planet(container.Planets[i]);
                }
            }
            else
            {
                Planets = new Planet[0];
            }
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

        public List<SolarSystem> GetAllSolarSystems()
        {
            List<SolarSystem> solarSystems = new List<SolarSystem>();
            if (Type == CelestialBodyType.SolarSystem)
            {
                solarSystems.Add((SolarSystem)this);
            }
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    solarSystems.AddRange(Expanses[i].GetAllSolarSystems());
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
            if (SolarSystems != null)
            {
                for (int i = 0; i < SolarSystems.Length; i++)
                {
                    solarSystems.Add(SolarSystems[i]);
                }
            }
            return solarSystems;
        }

        public List<Sector> GetAllSectors()
        {
            List<Sector> sectors = new List<Sector>();
            if (Type == CelestialBodyType.Sector)
            {
                sectors.Add((Sector)this);
            }
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    sectors.AddRange(Expanses[i].GetAllSectors());
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

        public List<Galaxy> GetAllGalaxies()
        {
            List<Galaxy> galaxies = new List<Galaxy>();
            if (Type == CelestialBodyType.Galaxy)
            {
                galaxies.Add((Galaxy)this);
            }
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    galaxies.AddRange(Expanses[i].GetAllGalaxies());
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

        public List<Expanse> GetAllExpanses()
        {
            List<Expanse> expanses = new List<Expanse>();
            if (Type == CelestialBodyType.Expanse)
            {
                expanses.Add((Expanse)this);
            }
            if (Expanses != null)
            {
                for (int i = 0; i < Expanses.Length; i++)
                {
                    expanses.Add(Expanses[i]);
                }
            }
            return expanses;
        }

        public List<Universe> GetAllUniverses()
        {
            List<Universe> universes = new List<Universe>();
            if (Type == CelestialBodyType.Universe)
            {
                universes.Add((Universe)this);
            }
            return universes;
        }

        public Util.LinkedList<byte> Serialize()
        {
            Util.LinkedList<byte> bytes = new Util.LinkedList<byte>();
            bytes.Append(BitConverter.GetBytes((int)Type));
            bytes.Append(BitConverter.GetBytes(Position.x));
            bytes.Append(BitConverter.GetBytes(Position.y));
            bytes.Append(BitConverter.GetBytes(Radius));

            bytes.Append(SerializeCelestialBodies(Expanses));
            bytes.Append(SerializeCelestialBodies(Galaxies));
            bytes.Append(SerializeCelestialBodies(Sectors));
            bytes.Append(SerializeCelestialBodies(SolarSystems));
            bytes.Append(SerializeCelestialBodies(Stars));
            bytes.Append(SerializeCelestialBodies(Planets));

            bytes.Prepend(BitConverter.GetBytes(bytes.Length + 4));
            return bytes;
        }

        private Util.LinkedList<byte> SerializeCelestialBodies(CelestialBody[] bodies)
        {
            Util.LinkedList<byte> bytes = new Util.LinkedList<byte>();
            foreach (CelestialBody body in bodies)
            {
                bytes.Append(body.Serialize());
            }
            bytes.Prepend(BitConverter.GetBytes(bodies.Length));
            bytes.Prepend(BitConverter.GetBytes(bytes.Length + 4));
            return bytes;
        }

        private Expanse[] DeserializeExpanses(byte[] bytes, int startIndex)
        {
            int offset = 4;
            Expanse[] expanses = new Expanse[BitConverter.ToInt32(bytes, startIndex + offset)];
            offset += 4;

            for (int i = 0; i < expanses.Length; i++)
            {
                expanses[i] = new Expanse(bytes, startIndex + offset);
                offset += BitConverter.ToInt32(bytes, startIndex + offset);
            }

            return expanses;
        }

        private Galaxy[] DeserializeGalaxies(byte[] bytes, int startIndex)
        {
            int offset = 4;
            Galaxy[] galaxies = new Galaxy[BitConverter.ToInt32(bytes, startIndex + offset)];
            offset += 4;

            for (int i = 0; i < galaxies.Length; i++)
            {
                galaxies[i] = new Galaxy(bytes, startIndex + offset);
                offset += BitConverter.ToInt32(bytes, startIndex + offset);
            }

            return galaxies;
        }

        private Sector[] DeserializeSectors(byte[] bytes, int startIndex)
        {
            int offset = 4;
            Sector[] sectors = new Sector[BitConverter.ToInt32(bytes, startIndex + offset)];
            offset += 4;

            for (int i = 0; i < sectors.Length; i++)
            {
                sectors[i] = new Sector(bytes, startIndex + offset);
                offset += BitConverter.ToInt32(bytes, startIndex + offset);
            }

            return sectors;
        }

        private SolarSystem[] DeserializeSolarSystems(byte[] bytes, int startIndex)
        {
            int offset = 4;
            SolarSystem[] solarSystems = new SolarSystem[BitConverter.ToInt32(bytes, startIndex + offset)];
            offset += 4;

            for (int i = 0; i < solarSystems.Length; i++)
            {
                solarSystems[i] = new SolarSystem(bytes, startIndex + offset);
                offset += BitConverter.ToInt32(bytes, startIndex + offset);
            }

            return solarSystems;
        }

        private Star[] DeserializeStars(byte[] bytes, int startIndex)
        {
            int offset = 4;
            Star[] stars = new Star[BitConverter.ToInt32(bytes, startIndex + offset)];
            offset += 4;

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new Star(bytes, startIndex + offset);
                offset += BitConverter.ToInt32(bytes, startIndex + offset);
            }

            return stars;
        }

        private Planet[] DeserializePlanets(byte[] bytes, int startIndex)
        {
            int offset = 4;
            Planet[] planets = new Planet[BitConverter.ToInt32(bytes, startIndex + offset)];
            offset += 4;

            for (int i = 0; i < planets.Length; i++)
            {
                planets[i] = new Planet(bytes, startIndex + offset);
                offset += BitConverter.ToInt32(bytes, startIndex + offset);
            }

            return planets;
        }
    }
}