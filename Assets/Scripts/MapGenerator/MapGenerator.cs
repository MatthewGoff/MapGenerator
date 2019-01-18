using UnityEngine;

namespace MapGenerator
{
    public static class MapGenerator
    {
        public static Containers.Container GenerateMap(CelestialBodies.CelestialBodyType mapSize)
        {
            Containers.Container map;
            if (mapSize == CelestialBodies.CelestialBodyType.Galaxy)
            {
                map = new Containers.Galaxy(new Vector2(0f, 0f), true);
            }
            else if (mapSize == CelestialBodies.CelestialBodyType.Sector)
            {
                map = new Containers.Sector(new Vector2(0f, 0f), true);
            }
            else if (mapSize == CelestialBodies.CelestialBodyType.Cloud)
            {
                map = new Containers.Cloud(new Vector2(0f, 0f), true);
            }
            else
            {
                map = new Containers.SolarSystem(new Vector2(0f, 0f), true);
            }

            map.CalculateGlobalPositions(new Vector2(0f, 0f));
            return map;
        }
    }
}