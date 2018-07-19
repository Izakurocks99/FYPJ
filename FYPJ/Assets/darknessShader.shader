// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

Shader "Custom/darknessShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_dist ("Distance", Float) = 0
		_percent("Percent" , Range(0.0 , 1.0)) = 0
		_Thicc("Thicc",Float) = 1.0
		_EdgeColor("test Color",Color) = (1,0,0,1)
		_WaveLenght("Wave Lenght",Float) = 1
		_WaveStrenght("Wave Strenght",Float) = 1
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "shader_noice.cginc" // import noise functions

			struct appdata
			{
				float4 vertex : POSITION;
				float2  uv : TEXCOORD0;
				float4  ray : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;	
				float4 lerpRay : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float4 _MainTex_TexelSize;
			float _dist;
			float _percent;
			float _Thicc;
			float4 _EdgeColor;
			float _WaveLenght;
			float _WaveStrenght;

			v2f vert (appdata v)
			{
				//v2f o;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv =  v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif		

				o.lerpRay = v.ray;

				return o;
			}

			// float3 _WorldSpaceCameraPos;
			half4 frag (v2f i) : SV_Target
			{
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv));
				float linearDepth = Linear01Depth(rawDepth);
				float4 worldSpaceDir = linearDepth * i.lerpRay; // from camera to plane
				float3 worldSpacePos = _WorldSpaceCameraPos + worldSpaceDir; // w space pos

				float d = distance(worldSpacePos,_WorldSpaceCameraPos);
				
				d += cnoise(worldSpacePos * _WaveLenght) * _WaveStrenght;



				half4 col =	tex2D(_MainTex , i.uv);
				col = lerp( col     ,  _EdgeColor  ,   d >= ((_dist - _Thicc) * _percent));
				col = lerp(col , half4(0,0,0,0) , d >= (_dist * _percent));
				

				return col;	
			}

			ENDCG
		}
	}
}
