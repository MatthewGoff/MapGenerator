using UnityEngine;

namespace MapGenerator.Containers
{
    public class Star : Container
    {
        public static readonly float MIN_RADIUS = 0.5f;
        public static readonly float MAX_RADIUS = 2f;

        public Star(Vector2 localPosition, bool immovable) : base(localPosition, Random.Range(MIN_RADIUS, MAX_RADIUS), MAX_RADIUS, immovable)
        {

        }
    }
}