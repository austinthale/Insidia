Shader "Ben/Glitcher"
{
	Properties
	{
		[Header(Main Properties)]
		[Space(10)]
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		
		[Header(Glitching Properties)]
		[Space(10)]
		_Transparency("Transparency", Range(0,1)) = 0.25
		_CutoutThresh("Cutout Threshold", Range (0,1)) = 0.2
		[Header(X Axis)]
		_XDistance("X Distance", Range (0,20)) = 1
		_XAmplitude("X Amplitude", Range (0,10)) = 1
		_XSpeed("X Speed", Range (0,25)) = 1
		_XAmount("X Amount", Range (0,1)) = 1
		[Header(Y Axis)]
		_YDistance("Y Distance", Range (0,2)) = 1
		_YAmplitude("Y Amplitude", Range (0,10)) = 1
		_YSpeed("Y Speed", Range (0,25)) = 1
		_YAmount("Y Amount", Range (0,1)) = 1
		[Header(Z Axis)]
		_ZDistance("Z Distance", Range (0,2)) = 1
		_ZAmplitude("Z Amplitude", Range (0,10)) = 1
		_ZSpeed("Z Speed", Range (0,25)) = 1
		_ZAmount("Z Amount", Range (0,1)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;
			float _Transparency;
			float _CutoutThresh;
			float _XDistance;
			float _XAmplitude;
			float _XSpeed;
			float _XAmount;
			float _YDistance;
			float _YAmplitude;
			float _YSpeed;
			float _YAmount;
			float _ZDistance;
			float _ZAmplitude;
			float _ZSpeed;
			float _ZAmount;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.x += sin(_Time.y * _XSpeed + v.vertex.y * _XAmplitude) * _XDistance * _XAmount;
				v.vertex.y += sin(_Time.y * _YSpeed + v.vertex.y * _YAmplitude) * _YDistance * _YAmount;
				v.vertex.z += sin(_Time.y * _ZSpeed + v.vertex.y * _ZAmplitude) * _ZDistance * _ZAmount;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
				col.a = _Transparency;
				clip(col.r - _CutoutThresh);
				return col;
			}
			ENDCG
		}
	}
}
