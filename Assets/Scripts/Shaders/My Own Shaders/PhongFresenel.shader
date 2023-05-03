Shader "Unlit/PhongFresenel"
{
    Properties
    {
        _Gloss ("Gloss",float) = 0.5
        _EnergyConservation ("Energy Conservation",Range(0,1)) = 0
        _SurfaceColor ("Surface Color", Color) = (1,1,1,1)

        _FresenelThresold ("Fresenel Thresold",Range(0,1)) = 0.5
        [HDR]_FresenelColor ("Fresenel Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }



        Pass
        {
            //------------- Unity Shader Lab Code ends here  ----------------//
            //-------------Shader CG Code || HLSL Code start here ----------------//

            CGPROGRAM
            #pragma vertex vertexShader // specifies which function is the vertex shader
            #pragma fragment fragmentShader // specifies which function is the fragment shader

            #include "UnityCG.cginc" // include the Unity built-in CG  shader library
            #include "UnityLightingCommon.cginc"




            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Gloss;
            float4 _SurfaceColor;
            float _EnergyConservation;
            float _FresenelThresold;
            float4 _FresenelColor;

            struct MeshData // this contains all data related to the mesh itself like vertex position, uv coordinates, normals, tangents, etc
            {
                float4 vertex : POSITION; // vertex position in object space
                float2 uv : TEXCOORD0;  // uv coordinates for specefic texture for the mesh 
                float3 normal : NORMAL; // normal vector for the vertex of a mesh
                float4 worldPosition : TEXCOORD3;
            };

            struct v2f // this is the vertex to fragment data structure which is used to pass data from vertex shader to fragment shader, it is also the interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION; // clip space position
                float3 viewDirection : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 worldPosition : TEXCOORD3;
            };



            v2f vertexShader(MeshData v) // this function is the vertex shader which takes the mesh  data as input and returns the mesh  data as output
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex); // this is the world space position of the mesh vertices
                o.viewDirection = _WorldSpaceCameraPos-o.worldPosition   ; // this is the view direction of  the vertex from the  camera  in world space
                o.worldNormal = UnityObjectToWorldNormal(v.normal); // this is the normal vector of the mesh vertices respective  to  world space
              

                return o;
            }

            float4 fragmentShader(v2f i) : SV_Target // this tells to output the pixels from the fragment shader to the render target which is the frame buffer
            {
                 float3 normalVector = normalize(i.worldNormal);
                 float3 viewDirection = i.viewDirection;
                 float3 lightVector = _WorldSpaceLightPos0.xyz;

                 float lambertion = max(dot(normalVector, lightVector), 0);
                 float3 DiffuseLighting = lambertion  * _LightColor0.xyz * _SurfaceColor; // lambertion shading

                 // Specular ;ighting 

                 float3 halfVector = normalize(viewDirection + lightVector); // this is the half vector between the view direction and the light direction
                 float3 specularLight = saturate(dot(halfVector,normalVector)); //blinn phong specular lighting
                 specularLight = pow(specularLight,_Gloss) * _EnergyConservation; // this is the specular light intensity raised to the power of glossiness
                 specularLight = specularLight * _LightColor0.xyz; // this is the specular light intensity multiplied by the light color

                 float3 fresnel =  saturate(_FresenelThresold - dot(viewDirection,normalVector)) * _FresenelColor.xyz; // this is the fresnel t

                 return float4(DiffuseLighting + specularLight + fresnel, 1);
              
               
  
            }
            ENDCG
        }
    }
}

