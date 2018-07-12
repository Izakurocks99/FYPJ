// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Example/DomeVol2"
{
	Properties
	{
		_skytex ("sky tex", 2D) = "white" {}
		changeSpeed("change speed", Range(0. , 100.)) = 1. 
		distortionScale("wind distortion", Range(0. , 100.)) = 1.
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" 
		"RenderType"="Transparent"}
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade vertex:vert

		struct Input 
		{
			//float2	uv_skytex;
			float4	UVs;
			float	alphalerp;	
			float2  debug;
		};

	  float changeSpeed;
	  float distortionScale;
	   void vert (inout appdata_full v, out Input o) 
	   {
			float2 distortion = float2(0,0);
			float3 windDir = float3(0,0,-1);
			
			distortion.x = dot(windDir , v.tangent.xyz);
			//distortion.x +=  _Time;

			float3 binormal = cross(v.tangent.xyz,v.normal.xyz);
			distortion.y = dot(windDir , binormal);//* 0.5 + 0.5;	 

			o.debug = distortion;
			

			distortion *= distortionScale;
			

			float freq1 = frac(changeSpeed * _Time.y);
			float freq2 = frac(changeSpeed * _Time.y + 0.5f);
			float2 pos1 = distortion * freq1 * distortionScale;
			float2 pos2 = distortion * freq2 * distortionScale;
			
			const float PI = 3.14159265f;

			o.alphalerp = 1 -sin((PI / 2.f)  * freq1 * 2) ;  // todo test if i need the second alpha value

			o.UVs.xy = v.texcoord.xy + pos1;
			o.UVs.zw = v.texcoord.xy + pos2;


		}

		sampler2D	_skytex;
		void surf (Input IN, inout SurfaceOutput o)	
		{
			float4 first = tex2D(_skytex, IN.UVs.xy ).rgba  ;
			float4 second = tex2D(_skytex, IN.UVs.zw ).rgba ;
			float4 endcolor = lerp(first.rgba, second.rgba, IN.alphalerp);
			o.Albedo.rgb = endcolor.rgb
			;//* 0.0001f + float3( IN.debug.x * 0.001f,IN.debug.y,0);
			o.Alpha = endcolor.a ;//* 0.5;//lerp(org.a, second.a, IN.lerpVal);

		}
		ENDCG

	}
    Fallback "Diffuse"
}
