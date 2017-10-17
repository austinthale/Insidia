Shader "Custom/Energy" {
	Properties {
[Header(Textures and color)]
        [Space(10)]
        _MainTex ("Fog texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Color ("Color", color) = (1., 1., 1., 1.)
        //[Space()]

		[Header(Glitching Properties)]
		[Space(10)]

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
 
        [Header(Wispy Properties)]
        [Space(10)]
        _ScrollDirX ("X Axis Movement", Range(-1., 1.)) = 1.
        _ScrollDirY ("Y Axis Movement", Range(-1., 1.)) = 1.
		//[PowerSlider(3.0)]
        _Speed ("Speed", Range(-5,5)) = 1.
        _Distance ("Fading distance", Range(0, 10.)) = 1.
		_RedReduce("Fix Red", Range (0,5)) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull off
 
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
 
            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 vertCol : COLOR0;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Distance;
            sampler2D _Mask;
            float _Speed;
            fixed _ScrollDirX;
            fixed _ScrollDirY;
            fixed4 _Color;
			float _RedReduce;

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
 
            v2f vert(appdata_full v)
            {
                v2f o;
				v.vertex.x += sin(_Time.y * _XSpeed + v.vertex.y * _XAmplitude) * _XDistance * _XAmount;
				v.vertex.y += sin(_Time.y * _YSpeed + v.vertex.y * _YAmplitude) * _YDistance * _YAmount;
				v.vertex.z += sin(_Time.y * _ZSpeed + v.vertex.y * _ZAmplitude) * _ZDistance * _ZAmount;
				
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv2 = v.texcoord;
                o.vertCol = v.color;
                return o;
            }
			
 
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv + fixed2(-_ScrollDirX, -_ScrollDirY) * _Speed * _Time.x;
                fixed4 col = tex2D(_MainTex, uv) * _Color * i.vertCol;
                col.a *= tex2D(_Mask, i.uv2).r;
                col.a *= 1 - ((i.pos.z / i.pos.w) * _Distance);
				clip(col.b - (i.pos.z / i.pos.w) * _RedReduce);
                return col;
            }
            ENDCG
        }
    }
}