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
		//v.vertex.y += 8.f;
			//o.uv_skytex = v.texcoord.xy;
			//o.newUv =  o.uv_skytex;//v.texcoord.xy - 0.5; // 0.5 ??
			//float2 oldUVs = o.uv_skytex;
			//o.newUv.y += _Time.x * rotation * 0.0001f;
			//o.newUv += v.tangent.xy * 0.00001f;
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


			//float windscale = windstrenght * _Time;	
			//float windtemp = frac(windscale + 0.5f);
			//windscale = frac(windscale);

			//float2 dist1 = distortion * windtemp;
			//float2 dist2 = distortion * windscale;

			//o.UVs.xy = v.texcoord.xy + dist1;
			//o.UVs.zw = v.texcoord.xy + dist2;


			//float crossfade = cos(windscale * PI) * (0.5f) + 0.5;

			//o.alphalerp = crossfade;





			#if 0
			//float alpha1 = (cos(frac(windstrenght * _Time))/2.f) + 0.5f;
			//float alpha2 = (sin((frac(windstrenght * _Time + 0.5f)))/2.f) + 0.5f;

			//float2 firstPos = v.texcoord.xy + (distortion * distortionScale * frac(windstrenght * _Time));
			//float2 second = v.texcoord.xy + (distortion * distortionScale * (frac(windstrenght * _Time) + 0.5f) );

			//o.alphalerp = alpha1;
			//o.UVs.xy = firstPos;
			//o.UVs.zw = second;
			#endif

			//float cosrange = (cos(windstrenght * _Time) / 2.f) + 0.5f;



			//o.secondUV = o.uv_skytex + (distortion * cosrange);
			//o.lerpVal = cosrange;

			//const float PI = 3.14159265f;
			//o.lerpVal = cos(frac(windstrenght * _Time) * PI) * (-0.5f) + 0.5f;
			// normalize(distortion);

			//float alpha1 = cos(_Time * rotation) / 2.f + 0.5f; // create positive curve
			//float alpha2 = sin(_Time * rotation + 0.5f) / 2.f + 0.5f; // create positive curve and move it bit up

			//o.distortion1 = distortion * rotation * alpha1;
			//o.distortion2 = distortion * rotation * alpha2;
			//o.alpha1 = alpha1;
			//o.alpha2 = alpha2;



			
			//v.vertex.y += 10.f;
			//float strenght = 0.02f;
			//o.newUv += distortion * strenght * _Time;

			//o.newUv.y += -0.1f * _Time;

			//float3 objUp = mul((float3x3)unity_WorldToObject, float3(0,1,0)); // Convert world up to object up so it can be converted to tangent up.
			//TANGENT_SPACE_ROTATION;
			//float3 tangentUp = mul(rotation, objUp);

			//o.newUv.xy += tangentUp.xy;

			// o.newUv.x  = cos(_Time.x * rotation + 0.5) * oldUVs.x + sin(_Time.x * rotation + 0.5) * oldUVs.y;
		 // o.newUv.y = sin(_Time.x * rotation + 0.5) * oldUVs.x - cos(_Time.x * rotation + 0.5) * oldUVs.y;
			//o.uvK.x  = cos(_Time.x * speed1 + 0.5) * oldUVs.x + sin(_Time.x * speed1 + 0.5) * oldUVs.y;
		//	o.uvK.y = sin(_Time.x * speed1 + 0.5) * oldUVs.x - cos(_Time.x * speed1 + 0.5) * oldUVs.y;
		}

		sampler2D	_skytex;
		void surf (Input IN, inout SurfaceOutput o)	
		{
			float4 first = tex2D(_skytex, IN.UVs.xy ).rgba  ;
			float4 second = tex2D(_skytex, IN.UVs.zw ).rgba ;
			float4 endcolor = lerp(first.rgba, second.rgba, IN.alphalerp);
			o.Albedo.rgb = endcolor.rgb
			;//* 0.0001f + float3( IN.debug.x * 0.001f,IN.debug.y,0);
			o.Alpha = endcolor.a;//lerp(org.a, second.a, IN.lerpVal);


			//float4 second = tex2D(_skytex,IN.secondUV).rgba;
			//o.Albedo.rgb += tex2D(_skytex,IN.newUv + IN.distortion1).rgb * IN.alpha1;
			//o.Albedo.rgb += tex2D(_skytex,IN.newUv + IN.distortion2).rgb * IN.alpha2;
			//o.Albedo.rg += IN.distortion;
			//o.Albedo.b *= 0.001f;

			//o.Albedo.rg += IN.newUv ;
		}
		ENDCG

	}
    Fallback "Diffuse"
}
