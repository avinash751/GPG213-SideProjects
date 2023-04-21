Shader "Custom/OutlineShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Range(0.0, 0.1)) = 0.01
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float depth : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _OutlineColor;
                float _OutlineWidth;
                float4 _CameraDepthTexture;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.depth = ComputeGrabScreenPos(o.vertex).z;
                    return o;
                }

                float4 frag(v2f i) : SV_Target{
                    // Get the screen space depth of the current pixel
                    float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv).r);

                // Calculate the screen space position of the current pixel
                float4 pos = ComputeGrabScreenPos(i.vertex, depth);

                // Calculate the screen space derivative of the current pixel position
                float2 dx = ddx(pos.xy);
                float2 dy = ddy(pos.xy);
                float edge = max(max(abs(dx.x), abs(dy.x)), max(abs(dx.y), abs(dy.y)));

                // Calculate the outline color
                float4 outlineColor = _OutlineColor;

                // Calculate the texture color
                float4 texColor = tex2D(_MainTex, i.uv);

                // Combine the outline and texture colors
                float4 finalColor = lerp(outlineColor, texColor, texColor.a);

                // Apply the outline width
                finalColor.a = saturate((edge - _OutlineWidth) / (2 * _OutlineWidth - edge));

                // Return the final color
                return finalColor;
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}



