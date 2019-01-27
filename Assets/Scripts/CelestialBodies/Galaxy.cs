namespace CelestialBodies
{
    public class Galaxy : CelestialBody
    {
        public Galaxy(MapGenerator.Containers.Galaxy galaxy) : base(galaxy)
        {

        }

        public Galaxy(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}