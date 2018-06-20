// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Example/DomeVol2"
{
	Properties
	{
		_skytex ("sky tex", 2D) = "white" {}
		rotation("rotation rate", Range(-2.0 , 2.0)) = 0.1
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
			float2	uv_skytex;
			float2  newUv;
			float2	distortion;
		};

	  float rotation;
	   void vert (inout appdata_full v, out Input o) 
	   {
		//v.vertex.y += 8.f;
			o.uv_skytex = v.texcoord.xy;
			o.newUv =  o.uv_skytex;//v.texcoord.xy - 0.5; // 0.5 ??
			//float2 oldUVs = o.uv_skytex;
			//o.newUv.y += _Time.x * rotation * 0.0001f;
			//o.newUv += v.tangent.xy * 0.00001f;
			float2 distortion = float2(0,0);
			float3 windDir = float3(0,0,1);
			
			distortion.x = dot(windDir , normalize(v.tangent.xyz)) * 0.0001f;
			//distortion.x +=  _Time;

			float3 binormal = cross(v.tangent.xyz,normalize(v.normal.xyz) ) ;
			distortion.y = dot(windDir , binormal) + 1.f;	 
			// normalize(distortion);

			o.distortion = distortion;
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
			o.Albedo.rgb += tex2D(_skytex,IN.newUv + IN.distortion.xy * _Time * 0.02f).rgb * 0.0001f;
			o.Albedo.rg += IN.distortion;
			//o.Albedo.b *= 0.001f;

			//o.Albedo.rg += IN.newUv ;
			o.Alpha =  tex2D(_skytex,IN.newUv).a; 
		}
		ENDCG

	}
    Fallback "Diffuse"
}
