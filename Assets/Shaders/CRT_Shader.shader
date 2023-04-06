Shader "Custom/CRT_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float2 offset = v.uv / 0.40;
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Curvature;
            float _Vignette;

            fixed4 frag(v2f i) : SV_Target
            {
                // Manipulate the UV Coordinate to span from -1 to 1.
                float2 uv = i.uv * 2.0f - 1.0f;
                // Calculate the Offset by Inputed Curvature value against the uv.yx coordinates
                float2 offset = uv.yx / _Curvature;
                // Applies the curvature to the UV Coordinates
                uv = uv + uv * offset * offset;
                // Reset the uv coordinates to 0 to 1
                uv = uv * 0.5f + 0.5f;

                // Grabbing each pixel
                /*
                * Contains R,G,B,A Values to manipulate per pixel.
                */
                fixed4 col = tex2D(_MainTex, uv);

                // Detect texture beyond 0 or 1 and set color to black.
                if (uv.x <= 0.0f || uv.x >= 1.0f || uv.y <= 0.0f || uv.y >= 1.0f) {
                    col = 0;
                }

                // Reset UV Coordiates to -1 to 1.
                uv = uv * 2.0f - 1.0f;

                /*
                * _ScreenParams,  type: float4
                * x is the width of the camera¡¦s target texture in pixels,
                * y is the height of the camera¡¦s target texture in pixels, 
                * z is 1.0 + 1.0/width and w is 1.0 + 1.0/height.
                */
                float2 vignette = _Vignette / _ScreenParams.xy;

                // Smoothstep is like LERP, however, gradually speed up from the start and slow down toward the end
                vignette = smoothstep(0.0f, vignette, 1.0f - abs(uv));

                // Simulates CRT Scanline FX, manipulate the R,G,B Channels
                col.g *= (sin( uv.y * _ScreenParams.y * 2.0f ) + 1.0f) * 0.2f + 1.0f;
                col.rb *= (cos( uv.y * _ScreenParams.y * 2.0f ) + 1.0f) * 0.2f + 1.0f;

                // just invert the colors
                /*col.rgb = 1 - col.rgb;*/
                /*col.r = 1;*/

                return col * vignette.x * vignette.y;
            }
            ENDCG
        }
    }
}
