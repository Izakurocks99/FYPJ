Shader "Example/Disintegrate" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emissive ("Emissive",2D) = "white" {}
		_MetallicTex ("Metallic Tex", 2D) = "white" {}
		_RoughnessTex ("Roughness Tex",2D) = "white" {}
		_TreshHold ("Treshold", Range(0,1)) = 0.0
		_EdgeWidth("Edge Width",Range(0 , 0.5)) = 0.1
		[HDR]_EdgeColor("EdgeColor",Color) = (1,1,1)
		NoiseTex("Noise",2D) = "white" {}
	}
	SubShader {
		Tags {
		"RenderType"="Transparent"
		"Queue" = "Transparent" 
		}
		LOD 200
		Blend SrcAlpha One
		//Cull Off
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MetallicTex;
		sampler2D _RoughnessTex;
		sampler2D _Emissive;
		float _TreshHold;
		sampler2D NoiseTex;
		float _EdgeWidth;
		float4 _EdgeColor;
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;//* _Color;
			// Metallic and smoothness come from slider variables
			o.Metallic = tex2D(_MetallicTex, IN.uv_MainTex).r;
			o.Smoothness = tex2D(_RoughnessTex,IN.uv_MainTex).r;
			o.Emission = tex2D(_Emissive,IN.uv_MainTex);

			float val = tex2D(NoiseTex,IN.uv_MainTex).a;
			o.Alpha = lerp(1,0, _TreshHold > val);
			o.Albedo =  lerp(c.rgb ,  _EdgeColor, 
				(_TreshHold + _EdgeWidth) > val ); 
		}
		ENDCG
	}
	FallBack "Diffuse"
}