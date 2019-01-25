using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapRendering
{
    public class StarsSpriteController : MonoBehaviour
    {
        List<CelestialBodies.Star> Stars;
        public void Initialize(List<CelestialBodies.Star> stars)
        {
            Stars = stars;
        }

        private void LateUpdate()
        {
            Vector4[] starVectors = new Vector4[Stars.Count];
            for (int i = 0; i < starVectors.Length; i++)
            {
                starVectors[i] = new Vector4(Stars[i].Position.x, Stars[i].Position.y, Stars[i].Radius, 0f);
            }
            GetComponent<SpriteRenderer>().material.SetVectorArray("bodies", starVectors);
        }
    }
}