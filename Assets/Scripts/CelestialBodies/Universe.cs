namespace CelestialBodies
{
    public class Universe : CelestialBody
    {
        public Universe(MapGenerator.Containers.Universe universe) : base(universe)
        {

        }

        public Universe(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}