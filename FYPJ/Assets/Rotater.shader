Shader "Example/Rotater" {
    Properties {
      //_Background ("BackTexture", 2D) = "white" {}
      //_Mountains ("MountainsTexture", 2D) = "white" {}
      _CurrentTexture ("Color (RGB) Alpha (A)", 2D) = "white" {}
      _NextTexture ("Color (RGB) Alpha (A)", 2D) = "white" {}
	  _Amount ("Extrusion Amount", Range(-10.0,10.0)) = 0
	  _Rotation ("Rotation", Range(0.0,10.0))= 0
	  CurrentScale ("CurrentScale",Range(0.0 , 1.0)) = 1
	  NextScale ("NextScale",Range(0.0 , 1.0)) = 0

    }
    SubShader {
      Tags {"Queue" = "Transparent" 
	  "RenderType"="Transparent"}
	  Cull Off
      CGPROGRAM
      #pragma surface surf Lambert alpha:fade vertex:vert

	struct Input {
          float2 uv_CurrentTexture;
      };

	  float _Amount;
	  float _Rotation;
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * _Amount;
		  float cosr = cos(_Rotation);
		  float sinr = sin(_Rotation);

		  float x = v.vertex.x * cosr + v.vertex.z * sinr;
		  float z = v.vertex.x * (-sinr) + v.vertex.z * cosr;
		  v.vertex.x = x;
		  v.vertex.z = z;
      }

	  //sampler2D _Background;
	  //sampler2D _Mountains;
	  sampler2D _CurrentTexture;
	  sampler2D _NextTexture;
	  float CurrentScale;
	  float NextScale;

      void surf (Input IN, inout SurfaceOutput o) {
		
		  o.Albedo += tex2D(_CurrentTexture,IN.uv_CurrentTexture) * CurrentScale;
		  o.Albedo += tex2D(_NextTexture,IN.uv_CurrentTexture) * NextScale;
	      o.Alpha =  tex2D(_NextTexture,IN.uv_CurrentTexture).a; 
	      //clip (frac((IN.worldPos.y+IN.worldPos.z*0.1) * 5) - 0.5);
		   //float4 res = float4(0,0,0,0);
		   //res = tex2D(_Clouds,IN.uv_Clouds);
		  // res = lerp( tex2D(_Background,IN.uv_Background),
				//tex2D(_clouds,in.uv_background), 0.8);
		   //res = lerp(res,tex2D(_Mountains,IN.uv_Background), 1);

			//o.Albedo = res;
			//o.Alpha = tex2D(_Clouds,IN.uv_Clouds).a;
			//TODO KIERRA NAITA EES TAAS KIITOS
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