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
		};

	  float rotation;
	   void vert (inout appdata_full v, out Input o) 
	   {
			o.uv_skytex = v.texcoord.xy;
			o.newUv =  o.uv_skytex;//v.texcoord.xy - 0.5; // 0.5 ??
			o.newUv.y += _Time.x * rotation;
			//o.uvK.x  = cos(_Time.x * speed1 + 0.5) * oldUVs.x + sin(_Time.x * speed1 + 0.5) * oldUVs.y;
		//	o.uvK.y = sin(_Time.x * speed1 + 0.5) * oldUVs.x - cos(_Time.x * speed1 + 0.5) * oldUVs.y;
		}

		sampler2D	_skytex;
		void surf (Input IN, inout SurfaceOutput o)	
		{
			o.Albedo.rgb += tex2D(_skytex,IN.newUv).rgb ;
			o.Alpha =  tex2D(_skytex,IN.newUv).a; 
		}
		ENDCG

	}
    Fallback "Diffuse"
}
