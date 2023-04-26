Shader "Unlit/Phong"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        SpecularColor("SpecularColor", Color) = (1, 1, 1, 1)
        Glossiness("Glossiness", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 viewDirection : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDirection : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 BaseColor;
            float Glossiness;
            float4 SpecularColor;
            

            v2f vert (appdata v)
            {
                v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.viewDirection = WorldSpaceViewDir(v.vertex);
                return o;
            }



            fixed4 frag(v2f i) : SV_Target
            {

                // -------------- Diffuse light  --------------
                float3 normalisedNormalum = normalize(i.worldNormal);
                float4 ambientColor = BaseColor * UNITY_LIGHTMODEL_AMBIENT;
                float directionIntensity = max(dot(_WorldSpaceLightPos0, normalisedNormal),0);
                float4 diffuseLight = fixed4(_LightColor0.rgb * BaseColor.rgb * directionIntensity, 1);

               
                // -------------- Specular light  --------------
                float3 viewDirection = normalize(i.viewDirection);
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDirection);
                float NdotH =  dot(normalisedNormal, halfVector);
                float4 specularLight  = SpecularColor *  pow(NdotH, Glossiness * Glossiness);
                fixed4 light = diffuseLight + ambientColor * specularLight;

                return light;

            }
            ENDCG
        }
    }
}
