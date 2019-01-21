using UnityEngine;

namespace MapGenerator.Containers
{
    public class Group : Container
    {
        private static readonly int MIN_GALAXIES = 9;
        private static readonly int MAX_GALAXIES = 9;
        public static readonly float MAX_RADIUS = Galaxy.MAX_RADIUS * 4;

        public Group(Vector2 localPosition, int randomSeed, bool root) : base(CelestialBodyType.Group, localPosition, 1f, randomSeed, MAX_RADIUS, root)
        {
            int population;
            if (root)
            {
                population = MAX_GALAXIES;
            }
            else
            {
                population = RNG.Next(MIN_GALAXIES, MAX_GALAXIES + 1);
            }
            CreateGalaxies(population);
            Distribute(true, true);
            CreateSectors(Galaxies.Length * 2);
            Distribute(false, true);
            CreateClouds(Sectors.Length * 2);
            Distribute(false, true);
            CreateSolarSystems(Clouds.Length * 2);
            Distribute(false, true);
            CreateStars(SolarSystems.Length * 2);
            Distribute(false, true);
            FinalizeContainer();
        }
    }
}