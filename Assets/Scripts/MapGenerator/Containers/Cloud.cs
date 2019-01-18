using UnityEngine;

namespace MapGenerator.Containers
{
    public class Cloud : Container
    {
        private static readonly int MIN_SOLAR_SYSTEMS = 3;
        private static readonly int MAX_SOLAR_SYSTEMS = 9;
        public static readonly float MAX_RADIUS = SolarSystem.MAX_RADIUS * 4;

        public Cloud(Vector2 localPosition, bool maximize = false) : base(localPosition, 1f, MAX_RADIUS)
        {
            int population;
            if (maximize)
            {
                population = MAX_SOLAR_SYSTEMS;
            }
            else
            {
                population = Random.Range(MIN_SOLAR_SYSTEMS, MAX_SOLAR_SYSTEMS + 1);
            }
            CreateSolarSystems(population);
            Distribute(true, true);
            CreateStars(SolarSystems.Length * 2);
            Distribute(false, true);
            FinalizeContainer();
        }
    }
}