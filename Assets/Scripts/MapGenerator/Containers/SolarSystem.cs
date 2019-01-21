using UnityEngine;

namespace MapGenerator.Containers
{
    public class SolarSystem : Container
    {
        private static readonly int MIN_PLANETS = 3;
        private static readonly int MAX_PLANETS = 9;
        public static readonly float MAX_RADIUS = Planet.MAX_RADIUS * 6;

        public SolarSystem(Vector2 position, int randomSeed, bool root) : base(CelestialBodyType.SolarSystem, position, 5f, randomSeed, MAX_RADIUS, root)
        {
            Stars = new Star[] { new Star(new Vector2(0, 0), RNG.Next(), true, false) };
            Quadtree.Insert(Stars[0]);
            SmallestContainerRadius = Mathf.Min(SmallestContainerRadius, Stars[0].Radius);

            int population;
            if (root)
            {
                population = MAX_PLANETS;
            }
            else
            {
                population = RNG.Next(MIN_PLANETS, MAX_PLANETS + 1);
            }
            CreatePlanets(population);
            Distribute(false, false);
            FinalizeContainer();
        }
    }
}