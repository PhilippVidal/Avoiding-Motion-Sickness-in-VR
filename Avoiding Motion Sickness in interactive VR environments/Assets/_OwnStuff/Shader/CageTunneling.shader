Shader "Tunneling/CageTunneling"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
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
            Name "CageTunneling"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct VS_INPUT
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VS_OUTPUT
            {
                float4  vertex  : SV_POSITION;
                float2  uv      : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            VS_OUTPUT vert(VS_INPUT input)
            {
                VS_OUTPUT output;
                UNITY_SETUP_INSTANCE_ID(input);                 //calculates eye index and instance id 
                //UNITY_INITIALIZE_OUTPUT(VS_OUTPUT, o);            //sets all v2f values to 0 
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);   //tells gpu which eye is being rendered + transfers value to frag shader
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


            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4x4 _RightEyeMatrix;
            float4x4 _LeftEyeMatrix;
            float _Radius;
            float _SmoothingOffset;

            TEXTURE2D_ARRAY(_CageTexture);
            SAMPLER(sampler_CageTexture);

            half4 frag(VS_OUTPUT input) : SV_Target
            {
                //https://docs.unity3d.com/Manual/SinglePassInstancing.html
                //float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.uv);
                
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 screenUV = input.uv.xy;

                // 0.0f - 1.0f => -0.5f - 0.5f             
                screenUV -= 0.5f;

                //-0.5f - 0.5f => -1.0f - 1.0f
                screenUV *= 2.0f;

                //screenUV *= 1.40;

                //Get correct (inverse) matrix for current eye 
                float4x4 eyeMatrix = unity_StereoEyeIndex ? _RightEyeMatrix : _LeftEyeMatrix;

                //Get screen uv coordinate without eye specific changes
                screenUV = mul(eyeMatrix, float4(screenUV, 1.0f, 1.0f)).xy;

                //Calculate distance to "center"
                float distanceSqr = screenUV.x * screenUV.x + screenUV.y * screenUV.y;
                float distance = sqrt(distanceSqr);
                //float distance = sqrt(sqr / _Radius);

                // a, b, c 
                // c < a => 0
                // c > a && c < b => 0 - 1 interpolation
                // c > b => 1
                distance = smoothstep(_Radius - _SmoothingOffset, _Radius, distance);

                //float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy);

                float4 color = SAMPLE_TEXTURE2D_ARRAY(_CageTexture, sampler_CageTexture, UnityStereoTransformScreenSpaceTex(input.uv), unity_StereoEyeIndex);      
                return float4(color.x, color.y, color.z, distance);
            }
            ENDHLSL
        }
    }
}


