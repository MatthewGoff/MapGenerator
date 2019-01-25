using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering
{
    public class PlanetsSpriteController : MonoBehaviour
    {
        List<CelestialBodies.Planet> Planets;
        public void Initialize(List<CelestialBodies.Planet> planets)
        {
            Planets = planets;
        }

        private void LateUpdate()
        {
            Vector4[] planetVectors = new Vector4[Planets.Count];
            for (int i = 0; i < planetVectors.Length; i++)
            {
                planetVectors[i] = new Vector4(Planets[i].Position.x, Planets[i].Position.y, Planets[i].Radius, 0f);
            }
            GetComponent<SpriteRenderer>().material.SetVectorArray("planets", planetVectors);
        }
    }
}