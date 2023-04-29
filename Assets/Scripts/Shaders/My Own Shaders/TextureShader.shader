Shader "Unlit/TextureShader"
{
    Properties
    {
        _MainTex("texture1",2D) = "white" {}
        _TextureToBlendTo( "textureToBlend", 2D ) = "white" {}
        _Noise("Noise",2D) = "white" {}
        _BlendAmount("BlendAmount",Range(0,1)) = 0.0
       
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Noise;
            sampler2D _TextureToBlendTo;
            float _BlendAmount;

            struct MeshData // this contains all data related to the mesh itself like vertex position, uv coordinates, normals, tangents, etc
            {
                float4 vertex : POSITION; // vertex position in object space
                float2 uv : TEXCOORD0;  // uv coordinates for specefic texture for the mesh 
                float3 normal : NORMAL; // normal vector for the vertex of a mesh
            };

            struct v2f // this is the vertex to fragment data structure which is used to pass data from vertex shader to fragment shader, it is also the interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION; // clip space position
                float4 worldPosition : TEXCOORD1;
            };



            v2f vertexShader(MeshData v) // this function is the vertex shader which takes the mesh  data as input and returns the mesh  data as output
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex); // this is the world space position of the vertex
                return o;
            }

            float4 fragmentShader(v2f i) : SV_Target // this tells to output the pixels from the fragment shader to the render target which is the frame buffer
            {
                float2 topDownWorldCoordinates = i.worldPosition.xz;
                float4 texture1= tex2D(_MainTex, topDownWorldCoordinates);
                float4 texture2 =  tex2D(_TextureToBlendTo, i.uv);
                float noise = tex2D(_Noise, i.uv).x;
                float4 Color = lerp(texture1, texture2,noise * _BlendAmount);
                return  Color;
            }
            ENDCG
        }
               
    }   
}
