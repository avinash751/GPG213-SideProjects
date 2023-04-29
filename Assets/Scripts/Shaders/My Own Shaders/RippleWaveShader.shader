Shader "Unlit/RippleWaveShader"
{
    Properties
    {
        _ScalerValue("ScalerValue", float) = 0.0
        _OffsetValue("OffsetValue", float) = 0.0
        _MoveSpeed("MoveSpeed", Range(-2,2)) = 0.0
        _RepeatRate("RepeatRate", float) = 1.0
        _WaveHeigth("WaveHeight",Range(-1,1)) = 0.0
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

             float _ScalerValue;
             float _OffsetValue;
             float _MoveSpeed;
             float _RepeatRate;
             float _WaveHeigth;


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
            };


            float CircularWave(float2 i)
            {
                i = i * _ScalerValue - _OffsetValue;
                float radialLength = length(i); // length of the vector from the center of the texture or mesh to the current pixel

                float t = sin((radialLength + _Time.y * _MoveSpeed) * _RepeatRate); // this is the ripple wave equation

                t *= (1 - radialLength); // this is the falloff equation
                return t;
            }



            v2f vertexShader(MeshData v) // this function is the vertex shader which takes the mesh  data as input and returns the mesh  data as output
            {
                v2f o;
                o.uv = v.uv;
                v.vertex.xy += CircularWave(v.uv) * _WaveHeigth;
                o.vertex = UnityObjectToClipPos(v.vertex);
               
                return o;
            }

            float4 fragmentShader(v2f i) : SV_Target // this tells to output the pixels from the fragment shader to the render target which is the frame buffer
            {
               float t = CircularWave(i.uv);

                return float4(t.xxx , 1);

            }
            ENDCG
        }
    }
}
