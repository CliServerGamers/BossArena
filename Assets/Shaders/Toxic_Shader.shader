Shader "Custom/Toxic_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude("Amplitude", Range(1,100)) = 0.0
        _Frequency("Frequency", Range(1,100)) = 0.0
        _Speed("Speed", Range(1,3)) = 0.0
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
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Amplitude;
                float _Frequency;
                int _Speed;

                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Retreiving Texture Color at each Pixel, then displace the this pixel respective of Wave Properties.
                    fixed4 col = tex2D(_MainTex, i.uv + sin(i.vertex.x/_Frequency + _Time[_Speed]) / _Amplitude);
                    // apply fog
                    /*UNITY_APPLY_FOG(i.fogCoord, col);*/
                    /*col.r = 0.5 - col.r;*/
                    col.g += 0.5;
                    col.b -= 0.4;
                    return col;
                }

            ENDCG
        }
    }
}
