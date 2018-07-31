Shader "Example/DiscoBloom" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Normal ("Normal texture", 2D) = "white" {}
		_EmissiveTex("Emissive texture",2D) = "white"{}
		_EmissiveTexAdd("Emissive texture add",2D) = "white" {}
		_EmissiveTexAdd2("Emissive texture add 2",2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Roughness ("Roughness", Range(0,1)) = 0.5
		_EmissiveScale("Emisissive scale", Range(0.0 , 200.0)) = 0.0
		_LerpVal("Lerp",Range(0.0,1.0)) = 0
		//[HDR]col("ffff",Color) = (.34, .85, .92, 1) 
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _EmissiveTex;
		sampler2D _EmissiveTexAdd;
		sampler2D _EmissiveTexAdd2;
		float	  _Roughness;
		float	  _EmissiveScale;
		float	  _LerpVal;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb ;
			o.Emission = tex2D(_EmissiveTex,IN.uv_MainTex).rgb * 0.00001 + lerp(tex2D(_EmissiveTexAdd,IN.uv_MainTex).rgb * _EmissiveScale * (1 -_LerpVal),
			tex2D(_EmissiveTexAdd2,IN.uv_MainTex).rgb * _EmissiveScale * _LerpVal,_LerpVal);
			// Metallic and smoothness come from slider variables
			o.Metallic = 0.f;
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal (tex2D (_Normal, IN.uv_MainTex));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
