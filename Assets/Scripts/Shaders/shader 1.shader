Shader "Unlit/shader 1"
{
    Properties
    {
        _ColorA("ColorA",Color) = (1,1,1,1)
        _ColorB("ColorB",Color) = (1,1,1,1)

        _RepeatRateX("RepeatrateX", float)= 1
        _AmplitudeX("AmplitudeX", float) = 0
        _InverterX("InverterX", float) = 0

         _RepeatRateY("RepeatrateY", float) = 1
        _AmplitudeY("AmplitudeY", float) = 0
        _InverterY("InverterY", float) = 0

        _AmplitudeSpeed("AmplitudeSpeed", float)=0
        _ColorChangeSpeed("ColorChangeSpeed",float)=0
        _HeightMultiplier("HeightMultiplier",float)=0

    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" 
                "Queue" = "Transparent"}

        Blend One    One
        ZWrite Off
        Cull Off

        Pass
        {
            //------------- Unity Shader Lab Code ends here  ----------------//
            //-------------Shader CG Code || HLSL Code start here ----------------//

            CGPROGRAM
            #pragma vertex vertexShader // specifies which function is the vertex shader
            #pragma fragment fragmentShader // specifies which function is the fragment shader

            #include "UnityCG.cginc" // include the Unity built-in CG  shader library



            float4 _ColorA;
            float4 _ColorB;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _RepeatRateX;
            float _AmplitudeX;
            float _InverterX;

            float _RepeatRateY;
            float _AmplitudeY;
            float _InverterY;

            float _AmplitudeSpeed;
            float _ColorChangeSpeed;
            float _HeightMultiplier;

            struct MeshData
            {
                float4 vertex : POSITION; // vertex position in object space
                float2 uv : TEXCOORD0;  // uv coordinates for specefic texture for the mesh 
                float3 normal : NORMAL; 
            };

            struct v2f // this is the vertex to fragment data structure which is used to pass data from vertex shader to fragment shader, it is also the interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : TEXCOORD1;
            };


            float Wave(float2 i)
            {
                float xPattern = sin(i.x * _RepeatRateX);
                float yPattern = cos(i.y * _RepeatRateY);
                float t = sin((xPattern * _InverterX + (_AmplitudeX * _Time.y * _AmplitudeSpeed)) + (yPattern * _InverterY + (_AmplitudeY * _Time.y * _AmplitudeSpeed)));
                return t;
            }


            v2f vertexShader(MeshData v) // this function is the vertex shader which takes the mesh  data as input and returns the mesh  data as output
            {
                v.vertex.x += Wave(v.uv) * _HeightMultiplier;
       
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                                   
                return o;
            }

           


          
            float4 fragmentShader(v2f i) : SV_Target // this tells to output the pixels from the fragment shader to the render target which is the frame buffer
            {
                
                float t = Wave(i.uv);
               
                _ColorA = lerp(_ColorA, _ColorB, t);
                _ColorB = lerp(_ColorB, _ColorA ,t);

                float ColorRateChange =  (cos(2 * _Time.y * _ColorChangeSpeed));
                
                float4 ColorPattern = lerp(_ColorA, _ColorB,ColorRateChange);
                
                return ColorPattern;   
            }
            ENDCG
        }
    } 
}
