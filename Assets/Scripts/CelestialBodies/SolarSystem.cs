namespace CelestialBodies
{
    public class SolarSystem : CelestialBody
    {
        public SolarSystem(MapGenerator.Containers.SolarSystem solarSystem) : base(solarSystem)
        {

        }

        public SolarSystem(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}