Shader "Ben/Outliner"
{
	Properties
	{	
		[Header(Main Properties)]
		[Space(10)]
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap("Normals Map", 2D) = "bump" {}
		_Color("Main Color", Color) = (.5,.5,.5,1)

		[Header(Outline Properties)]
		[Space(10)]
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1,1.1)) = 1
		_OutlineTransparency("Outline Transparency", Range(0,1)) = 0.25
	}


	SubShader
	{
		Tags{"Queue" = "Transparent"}
		LOD 300

		Blend SrcAlpha OneMinusSrcAlpha

		Pass//first Outline Render
		{
			ZWrite Off
			Cull Back
			//ZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"
			#include "UnityPBSLighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : POSITION;
				//float4 color : COLOR;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			float4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineTransparency;
			sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v.vertex.xyz *= (_OutlineWidth -0.03);
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
		
				return o;
			}
		
			half4 frag(v2f i) : COLOR
			{
				_OutlineColor.a = _OutlineTransparency;
				return _OutlineColor;
			}
			ENDCG

		}

		Pass//Second Outline Render
		{
			ZWrite Off
			//sZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : POSITION;
				//float4 color : COLOR;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			float4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineTransparency;
			sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v.vertex.xyz *= _OutlineWidth -.015;
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
		
				return o;
			}
		
			half4 frag(v2f i) : COLOR
			{
				_OutlineColor.a = (_OutlineTransparency - (_OutlineTransparency / 1.5));
				return _OutlineColor;
			}
			ENDCG

		}

		Pass//Second Outline Render
		{
			ZWrite Off
			//sZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : POSITION;
				//float4 color : COLOR;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			float4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineTransparency;
			sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v.vertex.xyz *= _OutlineWidth + .01;
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
		
				return o;
			}
		
			half4 frag(v2f i) : COLOR
			{
				_OutlineColor.a = (_OutlineTransparency - (_OutlineTransparency / 1.1));
				return _OutlineColor;
			}
			ENDCG

		}

		Blend One Zero
		Pass//normal Render
		{
			

			//CGPROGRAM
			//sampler2D _BumpMap;
			//void surf(Input IN, inout SurfaceOutput o)
			//{
			//	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv._BumpMap));
			//}
			//ENDCG
			ZWrite On
			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}
			Lighting On

			SetTexture[_MainTex]//makes it actually display the texture
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
			

			
		}
	}
}



























