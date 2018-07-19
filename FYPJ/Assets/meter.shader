Shader "Custom/meter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ShowPercent("Show",Range(0.0, 1.0)) = 0
		_WaveLenght("Wave Lenght",Float) = 1
		_WaveSpeed("Wave Speed",Float) = 1
		_WaveStrenght("Wave Strenght",Float) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True" }

		// Cull Off
        //Lighting Off
        //ZWrite Off
        //Fog { Mode Off }
        //Blend One OneMinusSrcAlpha



		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha


		//LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
	
			
			#include "UnityCG.cginc"
			#include "shader_noice.cginc" // import noise functions


			struct appdata
			{
				float4 vertex	: POSITION;
				float2 uv		: TEXCOORD0;
			};

			struct v2f
			{
				float2 uv		: TEXCOORD0;
				float4 vertex	: SV_POSITION;
				float3 lerpPos	: TEXCOORD1;
			};

			sampler2D	_MainTex;
			float4		_MainTex_ST;
			float		_ShowPercent;
			float		_WaveLenght;
			float		_WaveSpeed;
			float		_WaveStrenght;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.lerpPos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float3 rpos = i.lerpPos * _WaveLenght + _Time.y * _WaveSpeed;
				float p = cnoise(rpos)* _WaveStrenght + i.uv.y;
				col *= lerp(col.a , 0 ,p > _ShowPercent); //i.uv.y > _ShowPercent);//col.a;

				//col.r = 1;
				//col.a = lerp(col.a , 0 , i.uv.y > _ShowPercent);
				return col;
			}
			ENDCG
		}
	}
}
