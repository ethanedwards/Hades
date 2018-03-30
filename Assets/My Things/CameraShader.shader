Shader "Hidden/CameraShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CurTex ("Texture", 2D) = "white" {}
		_YPos ("_YPos", Range(0.0, 1)) = 1.0
		_XPos ("_XPos", Range(0.0, 1)) = 1.0
		_Random1 ("_Random1", Range(0.0, 1)) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

		float3 rgb_to_hsv_no_clip(float3 RGB)
        {
                float3 HSV;
           
         float minChannel, maxChannel;
         if (RGB.x > RGB.y) {
          maxChannel = RGB.x;
          minChannel = RGB.y;
         }
         else {
          maxChannel = RGB.y;
          minChannel = RGB.x;
         }
         
         if (RGB.z > maxChannel) maxChannel = RGB.z;
         if (RGB.z < minChannel) minChannel = RGB.z;
           
                HSV.xy = 0;
                HSV.z = maxChannel;
                float delta = maxChannel - minChannel;             //Delta RGB value
                if (delta != 0) {                    // If gray, leave H  S at zero
                   HSV.y = delta / HSV.z;
                   float3 delRGB;
                   delRGB = (HSV.zzz - RGB + 3*delta) / (6.0*delta);
                   if      ( RGB.x == HSV.z ) HSV.x = delRGB.z - delRGB.y;
                   else if ( RGB.y == HSV.z ) HSV.x = ( 1.0/3.0) + delRGB.x - delRGB.z;
                   else if ( RGB.z == HSV.z ) HSV.x = ( 2.0/3.0) + delRGB.y - delRGB.x;
                }
                return (HSV);
        }
 
        float3 hsv_to_rgb(float3 HSV)
        {
                float3 RGB = HSV.z;
           
                   float var_h = HSV.x * 6;
                   float var_i = floor(var_h);   // Or ... var_i = floor( var_h )
                   float var_1 = HSV.z * (1.0 - HSV.y);
                   float var_2 = HSV.z * (1.0 - HSV.y * (var_h-var_i));
                   float var_3 = HSV.z * (1.0 - HSV.y * (1-(var_h-var_i)));
                   if      (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
                   else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
                   else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
                   else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
                   else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
                   else                 { RGB = float3(HSV.z, var_1, var_2); }
           
           return (RGB);
        }







			
			sampler2D _MainTex;
			sampler2D _CurTex;
			float _YPos;
			float _XPos;
			float _Random1;
			float randomNum(float2 uv){
 
     				float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
     				return abs(noise.x + noise.y) * 0.5;
 			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//_YPos = 0;
				// just invert the colors


				float avg = (col.r+col.g+col.b)/3.0;
				float rA = avg-col.r;
				float gA = avg-col.g;
				float bA = avg-col.b;

				fixed4 bw = fixed4(col.r+rA, col.g+gA, col.b+bA, col.a);
				bw.g = col.g;

				return bw;
				//return col;
			}
			ENDCG
		}
	}
}