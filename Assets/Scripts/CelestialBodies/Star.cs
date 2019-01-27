namespace CelestialBodies
{
    public class Star : CelestialBody
    {
        public Star(MapGenerator.Containers.Star star) : base(star)
        {

        }

        public Star(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}