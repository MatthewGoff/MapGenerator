using UnityEngine;

namespace MapGenerator.Containers
{
    public class Planet : Container
    {
        public static readonly float MIN_RADIUS = 0.5f;
        public static readonly float MAX_RADIUS = 2.0f;

        public Planet(Vector2 localPosition) : base(localPosition, Random.Range(0.5f, 2.0f), MAX_RADIUS)
        {

        }
    }
}