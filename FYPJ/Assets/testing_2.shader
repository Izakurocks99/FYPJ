Shader "Example/Diffuse Simple" {
	
	Properties
	{
		 _MyColor ("My colour", Color) = (1, 0, 0, 1) // (R, G, B, A)
	}
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert

	  	half4 _MyColor;


      struct Input {
          float4 color : COLOR;
      };
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = _MyColor.rgb;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }