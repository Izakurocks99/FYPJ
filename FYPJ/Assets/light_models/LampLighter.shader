Shader "Example/LampLighter" {
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		Scale ("scale",Range(1,10)) = 1.0
		//_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_ColorModifier("Color Modifier",Range(1.0 , 100.0)) = 20.
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass
		{
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert
			#pragma fragment frag
		//	#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0

			//sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			//half _Glossiness;
			//half _Metallic;
			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			//UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			//UNITY_INSTANCING_BUFFER_END(Props)


				struct appdata_t {
					float4 vertex : POSITION;
					//float3 normal : NORMAL;
					//float2 uv : TEXCOORD0;
				};
	 
				struct v2f {
					float4 vertex : SV_POSITION;
					//float3 worldPos : TEXCOORD1;
					//half2 uv : TEXCOORD0;
					//float3 normal : NORMAL;
				};


			v2f vert (appdata_t v){
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);	//UNITY_MATRIX_MVP *  v.vertex;

				return o;
			}
			fixed4 _Color;
			float _ColorModifier;
			float Scale;
			fixed4 frag (v2f i) : SV_Target{
				float4 col = float4(_Color.x * _ColorModifier,_Color.y * _ColorModifier,_Color.z * _ColorModifier ,1.) * Scale;
				//col += float4(10,10,10,10);
				return col;
			}


			/*void surf (Input IN, inout SurfaceOutputStandard o) {



				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
				c *= _ColorModifier;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}*/
		ENDCG
		}
	}
	FallBack "Diffuse"
}
