Shader "CameraFading/CameraFading"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0)
        //_MainTex ("Albedo (RGB)", 2D) = "white" {}
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off Cull Off
        Pass
        {
            Name "CameraFading"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct VS_INPUT
            {
                float4 vertex     : POSITION;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VS_OUTPUT
            {
                float4  vertex      : SV_POSITION;
                float2  uv          : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            
            VS_OUTPUT vert(VS_INPUT input)
            {
                VS_OUTPUT output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                //UNITY_TRANSFER_INSTANCE_ID();

                output.vertex = float4(input.vertex.xyz, 1.0);

            #if UNITY_UV_STARTS_AT_TOP
                output.vertex.y *= -1;
            #endif

                output.uv = input.uv;
                return output;
            }

            //TEXTURE2D_X(_CameraOpaqueTexture);
            //SAMPLER(sampler_CameraOpaqueTexture);

            float4 _Color;
            half4 frag(VS_OUTPUT input) : SV_Target
            {
                return _Color;
            }
            ENDHLSL
        }
    }
}