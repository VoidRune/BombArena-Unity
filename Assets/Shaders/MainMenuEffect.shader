// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'

// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable

// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable

// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'

Shader "Hidden/MainMenuEffect"
{
    Properties
    {

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

            // uniform float4x4 _CameraToWorld;
            uniform float4x4 _CameraInverseProjection;
            uniform float4x4 _CameraInverseView;

            uniform float3 _LightDirection;
            uniform float3 _ColourAMix;
            uniform float3 _ColourBMix;
            uniform float _Power;
            uniform float _Epsilon;
            uniform float _MaxDist;
            uniform int _MaxIter;

            // Mandelbulb distance estimation:
            // http://blog.hvidtfeldts.net/index.php/2011/09/distance-estimated-3d-fractals-v-the-mandelbulb-different-de-approximations/
            float2 Mandelbulb(float3 position) {
                float3 z = position;
                float dr = 1.0;
                float r = 0.0;
                int iterations = 0;
                float power = _Power;

                for (int i = 0; i < 15; i++) {
                    iterations = i;
                    r = length(z);

                    if (r > 2) {
                        break;
                    }

                    // convert to polar coordinates
                    float theta = acos(z.z / r);
                    float phi = atan2(z.y, z.x);
                    dr = pow(r, power - 1.0) * power * dr + 1.0;

                    // scale and rotate the point
                    float zr = pow(r, power);
                    theta = theta * power;
                    phi = phi * power;

                    // convert back to cartesian coordinates
                    z = zr * float3(sin(theta) * cos(phi), sin(phi) * sin(theta), cos(theta));
                    z += position;
                }
                float dst = 0.5 * log(r) * r / dr;
                return float2(iterations, dst * 1);
            }

            float3 EstimateNormal(float3 p) {
                float x = Mandelbulb(float3(p.x + _Epsilon, p.y, p.z)).y - Mandelbulb(float3(p.x - _Epsilon, p.y, p.z)).y;
                float y = Mandelbulb(float3(p.x, p.y + _Epsilon, p.z)).y - Mandelbulb(float3(p.x, p.y - _Epsilon, p.z)).y;
                float z = Mandelbulb(float3(p.x, p.y, p.z + _Epsilon)).y - Mandelbulb(float3(p.x, p.y, p.z - _Epsilon)).y;
                return normalize(float3(x, y, z));
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 rayOrigin = _WorldSpaceCameraPos;
                float2 coord = i.uv * 2.0 - 1.0;
                float4 rayTarget = mul(_CameraInverseProjection, float4(coord, 1.0, 1.0));
                float3 rayDirection = mul(_CameraInverseView, float4(normalize(rayTarget.xyz / rayTarget.w), 0.0)).xyz;


                float maxDist = _MaxDist;
                float maxSteps = float(_MaxIter);

                float3 p = rayOrigin;
                float distanceTraveled = 0.0f;

                int steps = 0;
                while(distanceTraveled < maxDist && steps < maxSteps)
                {
                    steps++;
                    float2 value = Mandelbulb(p);
                    float dst = value.y;

                    if (dst <= _Epsilon)
                    {
                        break;
                    }

                    distanceTraveled += dst;
                    p += dst * rayDirection;
                }
                float part = steps / maxSteps;

                float4 finalColor = float4(lerp(_ColourAMix, _ColourBMix, part), 1.0f);

                return finalColor;
            }
            ENDCG
        }
    }
}
