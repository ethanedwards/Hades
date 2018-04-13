Shader "Custom/Blood2" {
Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _Transparency("Transparency", Range(0.0,1.0)) = 0.25
        _CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
        _Distance("Distance", Float) = 1
        _Amplitude("Amplitude", Float) = 1
        _Speed ("Speed", Float) = 1
        _Amount("Amount", Range(0.0,1.0)) = 1
        _Tim("_Tim", Float) = 0
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            //#include "SimplexNoise3D.hlsl"
            #include "SimplexNoise2D.hlsl"

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
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;
            float _Tim;

            float randomNum(float2 uv){
 
     				float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
     				return abs(noise.x + noise.y) * 0.5;
 			}
            
            v2f vert (appdata v)
            {
                v2f o;
                //v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(max(randomNum(i.uv)*4, 1), 0, 0, 1);
                //col.a = _Transparency;
                float tim = min(_Time.x-_Tim/20.0-0.03, 0.0);
                float rad = tim/5.0;
                float bob = tim;
                float2 seed = float2(i.uv.x+bob, i.uv.y+bob);
                if(pow(pow(i.uv.x-0.5, 2)+pow(i.uv.y-0.5, 2), 0.5)-snoise(seed)/10.0-randomNum(i.uv)/10.0<rad){
                	col.a = 1.0;
                } else{
                	col.a = 0.0;
                }
                //float bob = _Distance+_Time/8.0;
                //seed = float2(i.uv.x+bob, i.uv.y+bob);
                //col.a = snoise(seed);
                //clip(col.r - _CutoutThresh);

                return col;
            }
            ENDCG
        }
    }
}