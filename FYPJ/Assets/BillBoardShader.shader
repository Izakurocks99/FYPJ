// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Custom/BillBoarder" 
{
	Properties
	{
		_Texture1 ("Texture1", 2D) = "white" {}
		_Texture2 ("Texture2", 2D) = "white" {}
		_Texture3 ("Texture3", 2D) = "white" {}
		_Texture4 ("Texture4", 2D) = "white" {}
		_Size ("Size",Float) = 0.02
		_WaveLenght("Wave Lenght",Float) = 10
		_DistortionAmount("distortion",Float) = 0.05
		_Speed("Speed",Float) = 0.0
		_Color1("Color",Color) = (1,1,1,1) 
		_Color2("Color",Color) = (1,1,1,1) 
	}
    SubShader {
    Pass {

			Tags { "RenderType"="Opaque" }
			LOD 200


        //LOD 200
        Cull Off
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
		#pragma geometry geom
		#include "UnityCG.cginc"
		#include "shader_noice.cginc" // import noise functions
        //struct tovert {
         //   float4 v : POSITION;
	//		float2 uv : TEXCOORD0;
      //  };
         
        struct v2g {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 norm : NORMAL;
			//float4 col : COLOR;
        };

		struct g2f
		{
            float4 pos : SV_POSITION;
			float4 col : COLOR;	
		};
			 
        v2g vert(appdata_base v) {
         
            v2g o;
            o.pos = v.vertex;//mul(unity_ObjectToWorld, v.vertex);//v.v;//UnityObjectToClipPos(v.v);
			o.uv = v.texcoord.xy;
			o.norm = v.normal;
			//o.col = v.color;
            return o;
        }
		sampler2D _Texture1;
		sampler2D _Texture2;
		sampler2D _Texture3;
		sampler2D _Texture4;
		float	  lastDigit;
		float	  midDigit;
		float	  firstDigit;
		float	_WaveLenght;
		float	_DistortionAmount;
		float	_Speed;
		float	_Scale1;
		float	_Scale2;
		float4 _Color1;
		float4 _Color2;

		float _Size;
		//https://docs.microsoft.com/en-us/windows/desktop/direct3dhlsl/dx-graphics-hlsl-geometry-shader
		[maxvertexcount(4)]
		void geom(point v2g IN[1],inout TriangleStream<g2f> triStream)//inout TriangleStream<g2f> triStream)
		{
			//g2f p;
			//p.col = float4(1,1,1,1);
			//p.pos = IN[0].pos;//UnityObjectToClipPos(IN[0].pos);
			//pointStream.Append(p);
			/*
			float3 up = float3(0, 1, 0);
			float3 look = _WorldSpaceCameraPos - IN[0].pos;
			look.y = 0;
			look = normalize(look);
			float3 right = cross(up, look);


			*/



			g2f p;
			float2 uv1 = IN[0].uv;
			float2 uv2 = IN[0].uv;
			float2 uv3 = IN[0].uv;
			// frac( _Time.y * 0.2);
			uv1.y +=  0.1 * firstDigit;
			uv2.y +=  0.1 * midDigit;
			uv3.y +=  0.1 * lastDigit;
			float lod = 0;
			//fgsdag
			p.col = tex2Dlod(_Texture1, float4( uv1 , 0, lod)) ;//* linearHeight;//float4(1,0,0,1);
			p.col += tex2Dlod(_Texture2, float4( uv2 , 0, lod)) ;// * linearHeight;//float4(1,0,0,1);
			p.col += tex2Dlod(_Texture3, float4( uv3 , 0, lod)) ;//* linearHeight;//float4(1,0,0,1);
			//p.col += tex2Dlod(_Texture4, float4( uv3 , 0, lod)) ;//* linearHeight;//float4(1,0,0,1);


			float4 v[4];
			float3 noisepos = IN[0].pos;
			noisepos *= _WaveLenght;
			noisepos.x += _Time.y * _Speed; 
			//float linearHeight = ((pnoise( noisepos , float3(1,1,1))  + 1) / 2.f); 
			float linearHeight = ((cnoise( noisepos )  + 1) / 2.f); 
			float height = linearHeight  * _DistortionAmount;





			float3 up = float3(0,1,0);
			float3 right = cross(up, IN[0].norm);//float3(1,0,0);



			float halfS = (0.5f * _Size) +     (p.col.r > 0.1 ? _DistortionAmount : height);//*  (float)LinearRgbToLuminance(saturate(p.col));;




			v[0] = float4(IN[0].pos + halfS * right - halfS * up, 1.0);//1 * _Time.y);//height);
			v[1] = float4(IN[0].pos + halfS * right + halfS * up, 1.0);//1 * _Time.y); //height);
			v[2] = float4(IN[0].pos - halfS * right - halfS * up, 1.0);//1 * _Time.y);// height);
			v[3] = float4(IN[0].pos - halfS * right + halfS * up, 1.0);//1 * _Time.y);// height);

			//v[0].z += height; 
			//v[1].z += height;
			//v[2].z += height;
			//v[3].z += height;

			float4 floorcol = lerp(_Color1,_Color2,linearHeight);
			float rscale = lerp(_Scale1,_Scale2,linearHeight);
			floorcol *= rscale;
			//p.col = lerp(IN[0].col * linearHeight, float4(1,1,1,1), p.col.r > 0.3);
			p.col = lerp(floorcol, float4(1,1,1,1), p.col.r > 0.1);

			p.pos = UnityObjectToClipPos( v[0]);//       UnityObjectToClipPos(v[0]);//mul(vp,v[0]);
			triStream.Append(p);

			p.pos = UnityObjectToClipPos( v[1]);//  UnityObjectToClipPos(v[1]);//mul(vp,v[1]);
			triStream.Append(p);

			p.pos = UnityObjectToClipPos( v[2]);//   UnityObjectToClipPos(v[2]);//mul(vp,v[2]);
			triStream.Append(p);

			p.pos = UnityObjectToClipPos( v[3]);//   UnityObjectToClipPos(v[3]);//mul(vp,v[3]);
			triStream.Append(p);
			
			//float4x4 vp = UnityObjectToClipPos(unity_WorldToObject);

			//p.col = float4(IN[0].uv.x,IN[0].uv.y,0,1);

			//p.pos = IN[0].pos.xyzw ;//- float4(0.03,0,0,0);
			//p.pos = UnityObjectToClipPos(p.pos);
			/*
			p.col = float4(1,1,1,1);
			p.pos = IN[0].pos.xyzw  + float4(0.03,0,0,0);
			p.pos = UnityObjectToClipPos(p.pos);
			pointStream.Append(p);
			
			p.col = float4(1,1,1,1);
			p.pos = IN[0].pos.xyzw  - float4(0,0.03,0,0);
			p.pos = UnityObjectToClipPos(p.pos);
			pointStream.Append(p);
			
			p.col = float4(1,1,1,1);
			p.pos = IN[0].pos.xyzw  + float4(0,0.03,0,0);
			p.pos = UnityObjectToClipPos(p.pos);
			pointStream.Append(p);
			*/
		}
         
        float4 frag(g2f o) : COLOR {
            return o.col;
        }
 
        ENDCG
        } 
    }
 
}