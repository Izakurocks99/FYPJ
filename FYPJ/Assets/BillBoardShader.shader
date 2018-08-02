Shader "Custom/BillBoarder" 
{
	Properties
	{
		_Texture1 ("Texture1", 2D) = "white" {}
		_Texture2 ("Texture2", 2D) = "white" {}
		_Texture3 ("Texture3", 2D) = "white" {}
		_Texture4 ("Texture4", 2D) = "white" {}
        _Texture5 ("BorderTexture",2D) = "white"{}
        _Texture6 ("BorderTexture",2D) = "white"{}
        _Brightness("Brightness",Float) = 1 
        _Speed ("Speed", Float) = 1
        _Speed2 ("Speed2", Float) = 1
        [HDR]_BorderColor("Color",Color) = (1,0,1,1)
        [HDR]_BorderColor2("Color2",Color) = (1,0,1,1)
	}
    SubShader {
    Pass {

        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		//Blend SrcAlpha One
        Blend SrcAlpha OneMinusSrcAlpha 

        Cull Off
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
		//#pragma geometry geom
		#include "UnityCG.cginc"
		//#include "shader_noice.cginc" // import noise functions TODO 
#if 0
         
        struct v2g {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 norm : NORMAL;
        };

		struct g2f
		{
            float4 pos : SV_POSITION;
			float4 col : COLOR;	
		};
#endif
        struct v2f
        {
			float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };
			 
        v2f vert(appdata_base v) {
         
            v2f OUT;
            //out.pos = v.vertex;//mul(unity_ObjectToWorld, v.vertex);//v.v;//UnityObjectToClipPos(v.v);
            OUT.pos = UnityObjectToClipPos(v.vertex);//mul(unity_ObjectToClip, v.vertex);
			OUT.uv = v.texcoord.xy;
			//out.norm = v.normal;
			//o.col = v.color;
            return OUT;
        }
		sampler2D _Texture1;
		sampler2D _Texture2;
		sampler2D _Texture3;
		sampler2D _Texture4;
        float4    _BorderColor;
        float4    _BorderColor2;
        sampler2D _Texture5;
        sampler2D _Texture6;
        float       _Brightness;
        float       _Speed;
        float       _Speed2;
		float	  lastDigit;
		float	  midDigit;
		float	  firstDigit;
#if 0
		float	_WaveLenght;
		float	_DistortionAmount;
		float	_Speed;
		float	_Scale1;
		float	_Scale2;
		float4 _Color1;
		float4 _Color2;

#endif
        float4 frag(v2f IN) : COLOR {
            
            float yUV = IN.uv.y / 10.0f;
            float2 nuv1 = float2(IN.uv.x,yUV + 0.1 * firstDigit);
            float2 nuv2 = float2(IN.uv.x,yUV + 0.1 * midDigit);
            float2 nuv3 = float2(IN.uv.x,yUV + 0.1 * lastDigit);
            float2 nuv4 = float2(IN.uv.x,yUV);
            float4 col =  tex2D(_Texture1,nuv1); 
            float4 col1 = tex2D(_Texture2,nuv2); 
            float4 col2 = tex2D(_Texture3,nuv3); 
            float4 col3 = tex2D(_Texture4,nuv4); 
            float2 borderUv = IN.uv;//float2(IN.uv.x, IN.uv.y * 10);
            float4 bordercolor = tex2D(_Texture5,IN.uv); 
            float4 bordercolor2 = tex2D(_Texture6,IN.uv); 
            //bordercolor *= bordercolor.a;
            float alpha = bordercolor.a;
            float alpha2 = bordercolor2.a;
            float currentPosition = frac(_Time.y * _Speed);
            float currentPosition2 = frac(_Time.y * _Speed2);
            const float widht = 0.2f;
            const float widht2 = 0.2f;
            
            float colorpos = (bordercolor.x + bordercolor.y + bordercolor.z) / 3.0f;
            float colorpos2 = (bordercolor2.x + bordercolor2.y + bordercolor2.z) / 3.0f;
            
            bordercolor = lerp( float4(0,0,0,0) , _BorderColor , currentPosition < colorpos && 
                                    currentPosition > colorpos - widht);
            bordercolor2 = lerp( float4(0,0,0,0) , _BorderColor2 , currentPosition2 < colorpos2 && 
                                    currentPosition2 > colorpos2 - widht2);


//_BorderColor , float4(0,0,0,0),
                                 //  bordercolor.r < currentPosition
                                   // && bordercolor.r > (currentPosition - widht));
            bordercolor *= alpha;
            bordercolor2 *= alpha2;
            col *= col.a;
            col1 *= col1.a;
            col2 *= col2.a;
            col3 *= col3.a;
            
            float4 finalColor = (col
+ col1 + col2 + col3) * _Brightness; 
               
            float4 finalBorder = bordercolor + bordercolor2;
                
            return finalColor + finalBorder * (finalColor.a - 1) * (-1); //distAdd
        }

        ENDCG
        } 
    }
 
}
