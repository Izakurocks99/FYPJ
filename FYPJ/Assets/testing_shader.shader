Shader "Example/Diffuse Simple" {
    Properties {
      _Texture1 ("Texture", 2D) = "white" {}
      _Texture2 ("Texture", 2D) = "white" {}
      _Texture3 ("Texture", 2D) = "white" {}
      _Texture4 ("Texture", 2D) = "white" {}
	  TexScale1 ("scale 1", Range(0.0,1.0)) = 1
	  TexScale2("scale 2", Range(0.0,1.0)) = 0
	  TexScale3 ("scale 3", Range(0.0,1.0)) = 0
	  TexScale4 ("scale 4", Range(0.0,1.0)) = 0
	  _NormalMap ("NormalMap", 2D) = "bump" {}
	  _Amount ("Extrusion Amount", Range(-10.0,10.0)) = 0

    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
	  Cull Off
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert

	struct Input {
          float2 uv_Texture1;
		  float2 uv_NormalMap;
		  float3 viewDir;

      };
	  float _Amount;
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * _Amount;
      }

	  sampler2D _Texture1;
	  sampler2D _Texture2;
	  sampler2D _Texture3;
	  sampler2D _Texture4;
	  sampler2D _NormalMap;
	  float		TexScale1;
	  float		TexScale2;
	  float		TexScale3;
	  float		TexScale4;

      void surf (Input IN, inout SurfaceOutput o) {
	      //clip (frac((IN.worldPos.y+IN.worldPos.z*0.1) * 5) - 0.5);
          o.Albedo += tex2D(_Texture1,IN.uv_Texture1).rgb * TexScale1;
          o.Albedo += tex2D(_Texture2,IN.uv_Texture1).rgb * TexScale2;
          o.Albedo += tex2D(_Texture3,IN.uv_Texture1).rgb * TexScale3;
          o.Albedo += tex2D(_Texture4,IN.uv_Texture1).rgb * TexScale4;
		  o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_NormalMap));
		  //half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          //o.Emission = _RimColor.rgb * pow (rim, _RimPower);
      }
      ENDCG
    }
    Fallback "Diffuse"
  }
  /*

  Shader "Example/Diffuse Texture" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
      };
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }*/