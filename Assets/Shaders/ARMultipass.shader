Shader "Custom/ARMultipass"
{
	Properties
	{
		_Color ("_Color", Color) = (1,1,1,1)
    	_textureY ("TextureY", 2D) = "white" {}
        _textureCbCr ("TextureCbCr", 2D) = "black" {}
        _MainTex ("Texture", 2D) = "white" {}
		_CurTex ("Texture", 2D) = "white" {}
		_YPos ("_YPos", Range(0.0, 1)) = 1.0
		_XPos ("_XPos", Range(0.0, 1)) = 1.0
		_Random1 ("_Random1", Range(0.0, 1)) = 1.0
		_ModFade ("_ModFade", Range(0.0, 1)) = 0.0
		_WhiteFade ("_WhiteFade", Range(0.0, 1)) = 0.0
	}
	SubShader
	{
		Cull Off
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
            ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			fixed4 _Color;
			float4x4 _DisplayTransform;
			float _Random1;
			float _ModFade;
			float _WhiteFade;

			struct Vertex
			{
				float4 position : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct TexCoordInOut
			{
				float4 position : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			TexCoordInOut vert (Vertex vertex)
			{
				TexCoordInOut o;
				o.position = UnityObjectToClipPos(vertex.position); 

				float texX = vertex.texcoord.x;
				float texY = vertex.texcoord.y;
				
				o.texcoord.x = (_DisplayTransform[0].x * texX + _DisplayTransform[1].x * (texY) + _DisplayTransform[2].x);
 			 	o.texcoord.y = (_DisplayTransform[0].y * texX + _DisplayTransform[1].y * (texY) + (_DisplayTransform[2].y));
	            
				return o;
			}
			
            // samplers
            sampler2D _textureY;
            sampler2D _textureCbCr;

            float randomNum(float2 uv){
 
     				float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
     				return abs(noise.x + noise.y) * 0.5;
 			}

            //hsv nonsense
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
        //end nonsense




			fixed4 frag (TexCoordInOut i) : SV_Target
			{
				// sample the texture
                float2 texcoord = i.texcoord;
                float y = tex2D(_textureY, texcoord).r;
                float4 ycbcr = float4(y, tex2D(_textureCbCr, texcoord).rg, 1.0);

				const float4x4 ycbcrToRGBTransform = float4x4(
						float4(1.0, +0.0000, +1.4020, -0.7010),
						float4(1.0, -0.3441, -0.7141, +0.5291),
						float4(1.0, +1.7720, +0.0000, -0.8860),
						float4(0.0, +0.0000, +0.0000, +1.0000)
					);

                fixed4 col = mul(ycbcrToRGBTransform, ycbcr);




                float avg = (col.r+col.g+col.b)/3.0;
				float rA = avg-col.r;
				float gA = avg-col.g;
				float bA = avg-col.b;

				fixed4 bw = fixed4(col.r+rA, col.g+gA, col.b+bA, col.a);

				if(col.r>0.7){
					bw.g=bw.g+2/10.0;
				}
				if(col.g>0.7){
					bw.g=bw.b+2/10.0;
				}
				if(col.b>0.7){
					//bw.r=bw.r+2/10.0;
				}
				float3 hsvp = float3(bw.r, bw.g, bw.b);

				float3 hsv = rgb_to_hsv_no_clip(hsvp);

				if(col.g>col.r){
					hsv.r = 0.6;
				} else{
					hsv.g = hsv.g-0.3;
				}

				float3 res = hsv_to_rgb(hsv);
				bw.r = res.r;
				bw.g = res.g;
				bw.b = res.b;

				//tint
                bw = bw * _Color;

				//return (3*bw+col)/4;
				fixed4 combo = col*(1.0-_ModFade)+bw*_ModFade;
				return combo;

			}
			ENDCG
		}
	}
}