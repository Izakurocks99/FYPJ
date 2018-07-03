Shader "Custom/CustomFXAA"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}

	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;

		//struct VertexData {
		//	float4 vertex : POSITION;//POSITION;
		//	float2 uv : TEXCOORD0;
		//};

		struct Interpolators {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};
		//
		Interpolators VertexProgram (appdata_full v) {
			Interpolators i;
			i.uv.xy = v.texcoord.xy;
			i.pos = UnityObjectToClipPos(v.vertex);
			return i;
		}

		float4 Sample (float2 uv) 
		{
			return tex2Dlod(_MainTex,  float4(uv, 0, 0));
		}

		float SampleLuminance (float2 uv) 
		{
			#if defined(LUMINANCE_GREEN) // if we use green channel lumiance
				return Sample(uv).g;
			#else
				return Sample(uv).a ;
			#endif
		}

		struct LuminanceData
		{
			float m, n, e, s, w; // 5 texels near by
			float ne, nw, se, sw; // cross texels

			float highest, lowest, contrast;
		};
		float SampleLuminance (float2 uv, float uOffset, float vOffset) 
		{
			uv += _MainTex_TexelSize * float2(uOffset, vOffset);
			return SampleLuminance(uv);
		}
		LuminanceData SampleLuminanceNeighborhood (float2 uv) 
		{
			LuminanceData l;
			l.m = SampleLuminance(uv);
			l.n = SampleLuminance(uv ,0,  1);
			l.e = SampleLuminance(uv , 1,  0);
			l.s = SampleLuminance(uv , 0, -1);
			l.w = SampleLuminance(uv ,-1,  0);

			l.ne = SampleLuminance(uv,  1,  1);
			l.nw = SampleLuminance(uv, -1,  1);
			l.se = SampleLuminance(uv,  1, -1);
			l.sw = SampleLuminance(uv, -1, -1);

			l.highest = max(max(max(max(l.n, l.e), l.s), l.w), l.m);
			l.lowest = min(min(min(min(l.n, l.e), l.s), l.w), l.m);
			l.contrast = l.highest - l.lowest;


			return l;
		}

		// The minimum amount of local contrast required to apply algorithm.
		//   0.333 - too little (faster)
		//   0.250 - low quality
		//   0.166 - default
		//   0.125 - high quality 
		//   0.063 - overkill (slower)
		float _RelativeThreshold;

		float _ContrastThreshold;

		// Choose the amount of sub-pixel aliasing removal.
		// This can effect sharpness.
		//   1.00 - upper limit (softer)
		//   0.75 - default amount of filtering
		//   0.50 - lower limit (sharper, less sub-pixel aliasing removal)
		//   0.25 - almost off
		//   0.00 - completely off
		float _SubpixelBlending;

		bool ShouldSkipPixel (LuminanceData l) 
		{
			float threshold = max(_ContrastThreshold, _RelativeThreshold * l.highest);
			return l.contrast < threshold;
		}

		float DeterminePixelBlendFactor (LuminanceData l) 
		{
			float filter = 2 * (l.n + l.e + l.s + l.w);
			filter += l.ne + l.nw + l.se + l.sw;
			filter *= 1.0 / 12;
			filter = abs(filter - l.m);
			filter = saturate(filter / l.contrast);
			float blendFactor = smoothstep(0, 1, filter); // for hashness
			return blendFactor * blendFactor * _SubpixelBlending;
		}

		struct EdgeData 
		{
			bool isHorizontal; 
			float pixelStep; // how large step
			float oppositeLuminance, gradient;

		};

		EdgeData DetermineEdge (LuminanceData l) {
			EdgeData e;
			float horizontal =
				abs(l.n + l.s - 2 * l.m) * 2 +
				abs(l.ne + l.se - 2 * l.e) +
				abs(l.nw + l.sw - 2 * l.w);
			float vertical =
				abs(l.e + l.w - 2 * l.m) * 2 +
				abs(l.ne + l.nw - 2 * l.n) +
				abs(l.se + l.sw - 2 * l.s);
			e.isHorizontal = horizontal >= vertical; // horizontal or vertical edge?

			float pLuminance = e.isHorizontal ? l.n : l.e;
			float nLuminance = e.isHorizontal ? l.s : l.w;
			float pGradient = abs(pLuminance - l.m);
			float nGradient = abs(nLuminance - l.m);


			e.pixelStep = e.isHorizontal ? _MainTex_TexelSize.y : _MainTex_TexelSize.x;

			

			if (pGradient < nGradient) { // negate if positive
				e.pixelStep = -e.pixelStep;
				e.oppositeLuminance = nLuminance;
				e.gradient = nGradient;
			}
			else {
				e.oppositeLuminance = pLuminance;
				e.gradient = pGradient;
			}



			return e;
		}

		float DetermineEdgeBlendFactor (LuminanceData l, EdgeData e, float2 uv) {
			float2 uvEdge = uv;
			float2 edgeStep;
			if (e.isHorizontal) {
				uvEdge.y += e.pixelStep * 0.5;
				edgeStep = float2(_MainTex_TexelSize.x, 0);

			}
			else {
				uvEdge.x += e.pixelStep * 0.5;
				edgeStep = float2(0, _MainTex_TexelSize.y);
			}

			float edgeLuminance = (l.m + e.oppositeLuminance) * 0.5;
			float gradientThreshold = e.gradient * 0.25;
			
			float2 puv = uvEdge + edgeStep;
			float pLuminanceDelta = SampleLuminance(puv) - edgeLuminance;
			bool pAtEnd = abs(pLuminanceDelta) >= gradientThreshold;
			
			UNITY_UNROLL
			for (int i = 0; i < 9 && !pAtEnd; i++) {
				puv += edgeStep;
				pLuminanceDelta = SampleLuminance(puv) - edgeLuminance;
				pAtEnd = abs(pLuminanceDelta) >= gradientThreshold;
			}

			float2 nuv = uvEdge - edgeStep;
			float nLuminanceDelta = SampleLuminance(nuv) - edgeLuminance;
			bool nAtEnd = abs(nLuminanceDelta) >= gradientThreshold;

			UNITY_UNROLL
			for (int i = 0; i < 9 && !nAtEnd; i++) {
				nuv -= edgeStep;
				nLuminanceDelta = SampleLuminance(nuv) - edgeLuminance;
				nAtEnd = abs(nLuminanceDelta) >= gradientThreshold;
			}

			float pDistance, nDistance;
			if (e.isHorizontal) {
				pDistance = puv.x - uv.x;
				nDistance = uv.x - nuv.x;
			}

			else {
				pDistance = puv.y - uv.y;
				nDistance = uv.y - nuv.y;
			}
			
			float shortestDistance;
			bool deltaSign;
			if (pDistance <= nDistance) {
				shortestDistance = pDistance;
				deltaSign = pLuminanceDelta >= 0;
			}
			else {
				shortestDistance = nDistance;
				deltaSign = nLuminanceDelta >= 0;
			}
			if (deltaSign == (l.m - edgeLuminance >= 0)) {
				return 0;
			}
			return 0.5 - shortestDistance / (pDistance + nDistance);

			//return pAtEnd;




			//return e.gradient;
		}


		float4 ApplyFXAA (float2 uv) 
		{
			LuminanceData l = SampleLuminanceNeighborhood(uv); //SampleLuminance(uv);
			if(ShouldSkipPixel(l)) return Sample(uv);
			float pixelBlend = DeterminePixelBlendFactor(l);
			EdgeData e = DetermineEdge(l);
			//return DetermineEdgeBlendFactor(l, e, uv) - pixelBlend;
			float edgeBlend = DetermineEdgeBlendFactor(l, e, uv);
				float finalBlend = max(pixelBlend, edgeBlend);

			if (e.isHorizontal) {
				uv.y += e.pixelStep * finalBlend;
			}
			else {
				uv.x += e.pixelStep * finalBlend;
			}
			return float4(Sample(uv).rgb, l.m);





			//return e.isHorizontal? float4(1, 0, 0, 0) : 1; // return middle one 
		}
	ENDCG

	SubShader {
		Cull Off
		ZTest Always
		ZWrite Off

		Pass // 1 fxaa pass
		{
			CGPROGRAM
			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram
			#pragma multi_compile _ LUMINANCE_GREEN


			float4 FragmentProgram (Interpolators i) : SV_Target 
			{
				return ApplyFXAA(i.uv);
			}
			ENDCG

		}

	}	
	Fallback "Diffuse"

}
