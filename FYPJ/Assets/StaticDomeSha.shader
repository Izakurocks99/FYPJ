Shader "Example/StaticDomeSha" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Alpha("Alpha",Range(0.0,1.0)) = 1.0
	}
	SubShader {
		Tags { "Queue" = "Transparent"
		"RenderType"="Transparent" }
		Cull off

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade 


		sampler2D _MainTex;
		float _Alpha;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);//   * 2;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a * (1  -_Alpha);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
