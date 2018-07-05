Shader "Custom/luminanceShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	CGINCLUDE
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			struct Interpolators
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			Interpolators VertexProgram(appdata_full v)
			{
				Interpolators i;
				i.uv.xy = (float2)v.texcoord.xy;
				i.pos = (float4)UnityObjectToClipPos(v.vertex);
				return i;
			}
	ENDCG
	SubShader
	{
		Cull Off
		ZTest Always
		ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram

			float4 FragmentProgram(Interpolators i) : SV_Target
			{
				float4 sample = (float4)tex2D(_MainTex, i.uv);
				sample.a = (float)LinearRgbToLuminance(saturate(sample.rgb));
				return sample;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}