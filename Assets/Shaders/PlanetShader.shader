Shader "Custom/PlanetShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float numberOfPlanets[9];
			float4 planets[900];
			float pixelWidth;

			struct fragmentInput
			{
				float3 worldPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD1;
			};

			fragmentInput vert(appdata_base v)
			{
				fragmentInput o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = float4(v.texcoord.xy, 0, 0);
				return o;
			}

			fixed AntiAliasing(float distance, float radius)
			{
				float pwidth = distance / pixelWidth;
				return smoothstep(radius, radius - pwidth * 1.5, distance);
			}
			
			fixed4 frag(fragmentInput fragInput) : SV_Target
			{
				int xCoord = floor(3 * fragInput.uv.x);
				int yCoord = floor(3 * fragInput.uv.y);
				int offset = (3 * xCoord + yCoord);

				fixed4 returnColor = fixed4(0, 0, 0, 0);
				for (int i = 0; i < 100; i++)
				{
					float4 planet;
					if (i < numberOfPlanets[offset])
					{
						planet = planets[i + (100 * offset)];
					}
					else
					{
						planet = fixed4(0, 0, 0, 0);
					}
					float distance = sqrt(pow(fragInput.worldPos.x - planet.x, 2) + pow(fragInput.worldPos.y - planet.y, 2));
					fixed alpha = AntiAliasing(distance, planet.z);
					if (alpha > returnColor.a)
					{
						float2 texturePosition = float2(fragInput.worldPos.x - planet.x + planet.z, fragInput.worldPos.y - planet.y + planet.z);
						texturePosition /= 2 * planet.z;
						fixed4 color = tex2D(_MainTex, texturePosition);
						returnColor = fixed4(color.rgb, alpha);
					}
				}
				return returnColor;
			}

			ENDCG
		}
	}
}