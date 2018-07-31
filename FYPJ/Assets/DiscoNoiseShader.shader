Shader "Example/DiscoNoiseShader"
{
	Properties
	{
        _AlphaTexture("AlphaTexture",2D) = "white"{}
        _Speed("Speed",Float) = 1
        _Scale("Scale",Float) = 1
        _Treshold("treshold",Float) = 1
        [HDR]_Color("Color",Color) = (1,1,1,1)
        [HDR]_Color1("Color1",Color) = (1,1,1,1)
        _Interpolation("InterPolation",Float) = 1
	}
	SubShader
	{

        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Blend SrcAlpha One
        //ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha 
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 objectpos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
		    float _Treshold;	
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.objectpos = v.vertex;//UnityObjectToWorld(v.vertex);
				o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
            float _Speed;
            float _Scale;
            sampler2D _AlphaTexture;
            float4 _Color;
            float4 _Color1;
            float _Interpolation;
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_AlphaTexture, i.uv);
                float noiseColor = cnoise(i.objectpos*_Scale + _Time.y * _Speed) ;
                fixed4 finalcolor = lerp(_Color,_Color1,_Interpolation);
                finalcolor.w = 1.f;
                fixed4 ret = col.a * finalcolor; 
                //ret *= col.x;
                
                ret = lerp(ret , float4(0,0,0,0), noiseColor < _Treshold);

				return ret;
			}
			ENDCG
		}
	}
}
