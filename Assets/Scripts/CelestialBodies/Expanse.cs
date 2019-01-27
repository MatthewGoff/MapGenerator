namespace CelestialBodies
{
    public class Expanse : CelestialBody
    {
        public Expanse(MapGenerator.Containers.Expanse expanse) : base(expanse)
        {

        }

        public Expanse(byte[] bytes, int startIndex) : base(bytes, startIndex)
        {

        }
    }
}