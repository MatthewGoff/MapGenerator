namespace CelestialBodies
{
    public class Planet : CelestialBody
    {
        public Planet(MapGenerator.Containers.Planet planet) : base(planet)
        {

        }

        public Planet(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}