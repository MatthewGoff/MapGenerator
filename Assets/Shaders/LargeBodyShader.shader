Shader "Custom/LargeBodyShader"
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
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 body;
				float pixelWidth;
				float masterAlpha;
				fixed4 _ExternalColor;
				fixed4 _InternalColor;

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

				fixed antiAlias(float distance, float radius)
				{
					return smoothstep(radius, radius - pixelWidth * 1.5, distance);
				}

				fixed4 frag(fragmentInput fragInput) : SV_Target
				{
					float distance = sqrt(pow(fragInput.worldPos.x - body.x, 2) + pow(fragInput.worldPos.y - body.y, 2));
					fixed alpha = antiAlias(distance, body.z);
					float ratio = distance / body.z;
					fixed4 color = lerp(_InternalColor, _ExternalColor, ratio);
					return fixed4(color.rgb, alpha * masterAlpha);
				}

				ENDCG
			}
		}
}