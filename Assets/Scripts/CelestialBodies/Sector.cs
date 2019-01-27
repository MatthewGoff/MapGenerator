using UnityEngine;

namespace CelestialBodies
{
    public class Sector : CelestialBody
    {
        public Sector(MapGenerator.Containers.Sector sector) : base(sector)
        {

        }

        public Sector(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}