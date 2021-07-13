Shader "SRP/Lit"
{
    Properties
    {
        _Color ("Color",Color)=(1,1,1,1)
        _BaseMap ("BaseMap",2D)="white"{}
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode" = "SRPDefaultUnlit"}
            HLSLPROGRAM
            #pragma target 3.5
            #pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling
            #pragma vertex LitPassVertex
			#pragma fragment LitPassFragment
            #include "../ShaderLibrary/Surface.hlsl"
			#include "../ShaderLibrary/Common.hlsl"
            #include "../ShaderLibrary/UnityInput.hlsl"
            #include "../ShaderLibrary/Lighting.hlsl"
			#include "../ShaderLibrary/Lit.hlsl"
			

            ENDHLSL
        }

        Pass
        {
            Tags {"LightMode" = "ShadowCaster"}
            HLSLPROGRAM
            #pragma target 3.5
            #pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling
            #pragma vertex ShadowCasterPassVertex
			#pragma fragment ShadowCasterPassFragment
			#include "../ShaderLibrary/Common.hlsl"
			#include "../ShaderLibrary/ShadowCaster.hlsl"
            ENDHLSL
        }
    }
}
