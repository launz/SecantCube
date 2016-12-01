Shader "Hidden/Show Depth"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)

	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}

		ZWrite On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float depth : DEPTH;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;
				return o;
			}
			
			half4 _Color;

//			half4 _OverDrawColor;
//			half4 _OverDrawColorPlus;
//
//			float _BlueMultColor;
//			float _PinkMultColor;
//			float _YellowMultColor;
//
//			float _BluePlusColor;
//			float _PinkPlusColor;
//			float _YellowPlusColor;

			half4 _StartColor;
			half4 _MidColor;
			half4 _EndColor;

			fixed4 frag (v2f i) : SV_Target
			{

			return (max (0, 1 - i.depth * 2) * _StartColor + (0.5 - abs (0.5 - i.depth)) * 2  * _MidColor + max (0, i.depth * 2 - 1) * _EndColor) * _Color;

//				if (i.depth < 0.5f) {
//					float sDepth = i.depth * 2;
//					float sInvert = 1 - sDepth;
//					return (fixed4 (sInvert , sInvert , sInvert , sInvert) * _StartColor + fixed4 (sDepth , sDepth , sDepth , sDepth) * _MidColor) *_Color;
//				}
//
//				float eDepth = (i.depth - 0.5f) * 2;
//				float eInvert = 1 - eDepth;
//				return (fixed4 (eInvert , eInvert , eInvert , eInvert) * _MidColor + fixed4 (eDepth , eDepth , eDepth , eDepth) * _EndColor);
				


//				float sDepth = i.depth * 2;
//				float sInvert = 1 - sDepth;
//				return (fixed4 (sInvert , sInvert , sInvert , sInvert) * _StartColor + fixed4 (sDepth , sDepth , sDepth , sDepth) * _MidColor);

				float invert = 1 - i.depth;
//				return fixed4 (invert * _BlueMultColor + _BluePlusColor, invert * _PinkMultColor + _PinkPlusColor, invert * _YellowMultColor + _YellowPlusColor, invert)  * _Color * _OverDrawColor + _OverDrawColorPlus;

//				return fixed4 (invert , invert , invert , invert)  * _Color * _OverDrawColor + _OverDrawColorPlus;
				return fixed4(invert, invert, invert, invert) * _Color;
			}
			ENDCG
		}
	}


	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
		}

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			half4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
//				return fixed4(1, 1, 1, 1) - (max (0, 1 - i.depth * 2) * _StartColor + (0.5 - abs (0.5 - i.depth)) * 2  * _MidColor + max (0, i.depth * 2 - 1) * _EndColor) * _Color;

			}
			ENDCG
		}
	}

//	SubShader
//    {
//        Tags
//        {
//            "RenderType"="Grass"
//        }
//
//        ZWrite On
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//            };
//
//            struct v2f
//            {
//                float4 vertex : SV_POSITION;
//                float depth : DEPTH;
//            };
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//                o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;
//                return o;
//            }
//            
//            half4 _Color;
//
//            half4 _StartColor;
//            half4 _MidColor;
//            half4 _EndColor;
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//
//                return (max (0, 1 - i.depth * 2) * _StartColor + (0.5 - abs (0.5 - i.depth)) * 2  * _MidColor + max (0, i.depth * 2 - 1) * _EndColor) * _Color;
////
////                float invert = 1 - i.depth;
////                return fixed4(invert, invert, invert, invert) * _Color;
//                return _Color;
//            }
//            ENDCG
//        }
//    }
}
