Shader "Custom/Beam"
{
    Properties
    {
        [Header(Beam)]
        [KeywordEnum(XPOSITIVE, XNEGATIVE, YPOSITIVE, YNEGATIVE)] _Direction("Direction", Int) = 0
        _Length("Length", Float) = 1.0

        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _EdgeLength("Edge Length", Range(0.0, 0.5)) = 0.3
        _WaveSpeed("Wave Speed", Float) = 1.0
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        [HideInInspector]
        _BaseMap("Albedo", 2D) = "white" {}
        [HideInInspector]
        _PI("PI", Float) = 3.141593
        [HideInInspector]
        _E("E", Float) = 2.718282

    }

    SubShader
    {
        Tags{"RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline"} 
        LOD 100

        Pass
        {
            Name "Forward"
            Tags{"LightMode" = "UniversalForward"}

            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF

            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #pragma multi_compile _DIRECTION_XPOSITIVE _DIRECTION_XNEGATIVE _DIRECTION_YPOSITIVE _DIRECTION_YNEGATIVE


            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : INTERP0;
                float3 normalWS : INTERP1;
                float3 tangentWS : INTERP2;
                float3 viewDirectionWS : INTERP3;
                float2 baseUV : INTERP4;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);


            float4 _BaseMap_ST;
            float _Length;
            float4 _BaseColor;
            float4 _EdgeColor;
            float _EdgeLength;
            float _WaveSpeed;
            float _Cutoff;
            float _PI;
            float _E;


            float rand1d(float x)
            {
                return sin(3.7 * x + 0.3) + sin(6.1 * x + 0.2);
            }

            Varyings vert (const Attributes input)
            {
                Varyings output = (Varyings)0;
                 const VertexPositionInputs vertexData = GetVertexPositionInputs( input.positionOS.xyz );

                output.positionHCS = vertexData.positionCS;
                output.positionWS = vertexData.positionWS;

                const VertexNormalInputs normalData = GetVertexNormalInputs( input.normalOS.xyz, input.tangentOS );
                output.normalWS = normalData.normalWS.xyz;
                output.tangentWS = normalData.tangentWS.xyz;
                output.baseUV = TRANSFORM_TEX(input.texcoord, _BaseMap);

                return output;
            }


            float4 frag(const Varyings input) : SV_Target
            {
                float t = _Time.z * _WaveSpeed;

                #ifdef _DIRECTION_XPOSITIVE
                float thickness = sin(input.baseUV.x * _PI);
                thickness *= saturate(min(log(1 + input.baseUV.y * _Length), 1));
                float edge = sin(input.baseUV.x * _PI) + _EdgeLength * rand1d(input.baseUV.y * 2 + t);
                edge *= saturate(min(log(1 + input.baseUV.y * _Length), 1));

                #elif _DIRECTION_XNEGATIVE
                float thickness = sin(input.baseUV.x * _PI);
                thickness *= saturate(min(log(1 + (1 - input.baseUV.y) * _Length), 1));
                float edge = sin(input.baseUV.x * _PI) + _EdgeLength * rand1d(input.baseUV.y * 2 + t);
                edge *= saturate(min(log(1 + (1 - input.baseUV.y) * _Length), 1));

                #elif _DIRECTION_YPOSITIVE
                float thickness = sin(input.baseUV.y * _PI);
                thickness *= saturate(min(log(1 + input.baseUV.x * _Length), 1));
                float edge = sin(input.baseUV.y * _PI) + _EdgeLength * rand1d(input.baseUV.x * 2 + t);
                edge *= saturate(min(log(1 + input.baseUV.x * _Length), 1));

                #elif _DIRECTION_YNEGATIVE
                float thickness = sin(input.baseUV.y * _PI);
                thickness *= saturate(min(log(1 + (1 - input.baseUV.x) * _Length), 1));
                float edge = sin(input.baseUV.y * _PI) + _EdgeLength * rand1d(input.baseUV.x * 2 + t);
                edge *= saturate(min(log(1 + (1 - input.baseUV.x) * _Length), 1));

                #else
                float thickness = 0.0;
                float edge = 0.0;
                #endif

                float4 color = saturate(_BaseColor * thickness + _EdgeColor * edge);

                AlphaDiscard( color.a, _Cutoff );

                return color;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
