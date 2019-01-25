﻿Shader "Custom/CelestialBodyShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ExternalColor("External Color", Color) = (1, 1, 1, 1)
		_InternalColor("Internal Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers d3d11_9x
			#pragma exclude_renderers d3d9
			#include "UnityCG.cginc"

			// Quality level
			// 2 == high quality
			// 1 == medium quality
			// 0 == low quality
			#define QUALITY_LEVEL 2

			sampler2D _MainTex;
			float4 bodies[1000];
			fixed4 _ExternalColor;
			fixed4 _InternalColor;

			struct fragmentInput
			{
				float3 worldPos : TEXCOORD0;
				float4 screenPos : SV_POSITION;
			};

			fragmentInput vert(appdata_base v)
			{
				fragmentInput o;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.screenPos = UnityObjectToClipPos(v.vertex);

				return o;
			}

			fixed AntiAliasing(float distance, float radius)
			{
				#if QUALITY_LEVEL == 2
					// length derivative, 1.5 pixel smoothstep edge
					float pwidth = length(float2(ddx(distance), ddy(distance)));
					return smoothstep(radius, radius - pwidth * 1.5, distance);
				#elif QUALITY_LEVEL == 1
					// fwidth, 1.5 pixel smoothstep edge
					float pwidth = fwidth(distance);
					return smoothstep(radius, radius - pwidth * 1.5, distance);
				#else
					// fwidth, 1 pixel linear edge
					float pwidth = fwidth(distance);
					return saturate((radius - distance) / pwidth);
				#endif
			}

			fixed4 frag(fragmentInput fragInput) : SV_Target
			{
				[loop]
				for (int i = 0; i < 100; i++)
				{
					float distance = sqrt(pow(fragInput.worldPos.x - bodies[i].x, 2) + pow(fragInput.worldPos.y - bodies[i].y,2));
					fixed alpha = AntiAliasing(distance, bodies[i].z);
					if (alpha > 0)
					{
						float ratio = distance / bodies[i].z;
						fixed4 color = lerp(_InternalColor, _ExternalColor, ratio);
						return fixed4(color.rgb, alpha);
					}
				}
				return fixed4(0, 0, 0, 0);
			}

			ENDCG
		}
	}
}