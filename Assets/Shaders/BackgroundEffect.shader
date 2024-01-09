// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/BackgroundEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Texture ("Base Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
            uniform fixed4 _MainTex_TexelSize;
            sampler2D _CameraDepthTexture;
            sampler2D _Texture;

            uniform float _BackgroundScale;
            uniform float _TimeScale;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = col.rrr;

                float nonlin_depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                if (nonlin_depth == 0)
                {
                    float2 screenSize = _MainTex_TexelSize.zw;
                    screenSize /= (screenSize.y * _BackgroundScale);

                    col.rgb = tex2D(_Texture, i.uv * screenSize + _Time.y * _TimeScale);
                }

                return col;
            }
            ENDCG
        }
    }
}
