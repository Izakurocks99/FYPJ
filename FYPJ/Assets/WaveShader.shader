// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EffectPosition("Effect Pos",Vector) = (0,0,0)
		_Ray("Ray",Float) = 2
		_Thicc("Thicc boi",Float) = 2
		_EffectColor("effect color",Color) = (1,1,1,1)
		_EffectColor2("effect color",Color) = (1,1,1,1)
		_EffectColor3("effect color",Color) = (1,1,1,1)
		
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		Cull Off ZWrite Off ZTest Always

		//LOD 100

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members lerpRay)
//#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 lerpRay : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			//float4 _MainTex_ST;
			sampler2D _CameraDepthTexture;
			float3 _EffectPosition;
			float _Ray;
			float _Thicc;
			float4 	_MainTex_TexelSize;
			half4 _EffectColor;
			half4 _EffectColor2;
			half4 _EffectColor3;

			float4 horizBars(float2 p)
			{
				return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
			}


			v2f vert (appdata v)
			{
				//v2f o;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif		

				o.lerpRay = v.ray;

				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				// sample the texture
				
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv));
				float linearDepth = Linear01Depth(rawDepth);
				float4 worldSpaceDir = linearDepth * i.lerpRay; // from camera to plane
				float3 worldSpacePos = _WorldSpaceCameraPos + worldSpaceDir; // w space pos

				//float3 dist = worldSpacePos - _EffectPosition; 
				

				half4 col = tex2D(_MainTex, i.uv);
				//half4 scol = half4(0,0,0,1);
				float d = distance(worldSpacePos,_EffectPosition);
				//half4 col = lerp(rcol, scol , d < _Ray && d > _Ray - _Thicc);

				half4 efCol = half4(0,0,0,0);

				//if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)

				if(d < _Ray && d > _Ray - _Thicc && linearDepth < 1)
				{
					float diff = 1 - (_Ray - d) / _Thicc;
					//efCol = _EffectColor * diff;

					half4 scannerCol = lerp(_EffectColor, _EffectColor2, diff) + horizBars(i.uv) * _EffectColor3;


					efCol = scannerCol * diff * 0.4;
				}

				//if(d < _Ray)
				//{
				//	return scol;//
				
				//}
				return col + efCol;
				//float rawDepth = tex2D(_CameraDepthTexture,i.uv).r;  

				//float colr =  tex2D(_CameraDepthTexture,i.uv).r; 
				//fixed4 col = tex2D(_MainTex, i.uv)* 0.00000001 + float4(colr,colr,colr,colr) * 10;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
			}
			ENDCG
		}
	}
}