Shader "Custom/SpriteShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
        };

        float rand(float x, float y){
				return frac(sin(x*12.9898 + y*78.233)*43758.5453);
		}

		float random(float3 co)
 		{
     		return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
 		}

        void vert (inout appdata_full v, out Input o)
        {
        	float3 seed = float3(v.vertex.x, v.vertex.y, (float) _Time);
        	float r = random(seed);

        	//float2 ofs = float2(r,r);
            //v.vertex.xy *= _Flip.xy;
            //v.vertex.xy += ofs/10.0;

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
        	float3 seed = float3(IN.uv_MainTex.x, IN.uv_MainTex.y, (float) _Time);
            fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
            if(random(seed)>0.1){
            	o.Albedo = 0;
            } else{
            	o.Albedo = c.rgb * c.a;
            }
            o.Albedo = c.rgb*c.a;

            o.Alpha = c.a*0.5;
        }
        ENDCG
    }

Fallback "Transparent/VertexLit"
}


