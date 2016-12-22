Shader "FireballShaders/CelShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,0)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	    _CelShadingness("CelShadingNess", Range(0,1)) = 0
		_ShineIntensity("BrilliBrilli", Range(0,1)) = 0
		_WaveMotion("Ondicas", Range(0,100)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
        #pragma surface surf CelShadingForward 
        #pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		fixed4 _Color;
		float _ShineIntensity;
		float _CelShadingness;

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			NdotL = smoothstep(0, 1 - _CelShadingness, NdotL);
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
			c.a = s.Alpha;
			return c;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			if (_ShineIntensity > 0) {
				o.Albedo += frac(sin(dot(IN.screenPos.xy, float2(12.9898, 78.233))) * 43758.5453) * _ShineIntensity;
			}
			o.Alpha = c.a;
		}
		ENDCG
	}
	SubShader {
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _WaveMotion;

			void vert(inout appdata_full v) {
				v.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				v.vertex.xyz += v.normal * _WaveMotion;
			}

			fixed4 frag(appdata_full i) : SV_Target {
				fixed4 c = 0;
				// normal is a 3D vector with xyz components; in -1..1
				// range. To display it as color, bring the range into 0..1
				// and put into red, green, blue components
				c.rgb = i.normal * 0.5 + 0.5;
				return c;
			}
			ENDCG
		}
	}
	SubShader {
		Pass {
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			// compile shader into multiple variants, with and without shadows
			// (we don't care about any lightmaps yet, so skip these variants)
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			// shadow helper functions and macros
			#include "AutoLight.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				SHADOW_COORDS(1) // put shadows data into TEXCOORD1
					fixed3 diff : COLOR0;
				fixed3 ambient : COLOR1;
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0.rgb;
				o.ambient = ShadeSH9(half4(worldNormal,1));
				// compute shadows data
				TRANSFER_SHADOW(o)
					return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
				fixed shadow = SHADOW_ATTENUATION(i);
				// darken light's illumination with shadow, keep ambient intact
				fixed3 lighting = i.diff * shadow + i.ambient;
				col.rgb *= lighting;
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
