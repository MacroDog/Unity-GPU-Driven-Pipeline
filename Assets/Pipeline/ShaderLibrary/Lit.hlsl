#ifndef MYRP_LIT_INCLUDED
#define MYRP_LIT_INCLUDED
#include "../ShaderLibrary/Common.hlsl"
CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
CBUFFER_END

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	UNITY_DEFINE_INSTANCED_PROP(float4 , _Color)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct VertexInput {
	float4 pos 			: POSITION;
	float3 normal 		: NORMAL;
	float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexOutput 
{
	float4 pos 	: SV_POSITION;
	float3 normal 	:TEXCOORD0;
    float2 uv     	: TEXCOORD1;
	float3 worldpos :TEXCOORD2;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

VertexOutput LitPassVertex (VertexInput i) 
{
	VertexOutput o;
	UNITY_SETUP_INSTANCE_ID(i);
    UNITY_TRANSFER_INSTANCE_ID(i, o);
	float4 worldPos = mul(UNITY_MATRIX_M, float4(i.pos.xyz, 1.0));
	o.pos = mul(UNITY_MATRIX_VP, worldPos);
	o.worldpos = worldPos.xyz;
	o.normal = mul((float3x3)UNITY_MATRIX_M, i.normal);
	o.uv = TRANSFORM_TEX(i.texcoord,_BaseMap);
	return o;
}

float4 LitPassFragment (VertexOutput i) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);
	Surface sur;
	InitLitSurfaceData(i.uv,i.normal, sur);
	// blinphone
	float3 basecolor = UNITY_ACCESS_INSTANCED_PROP(Props,_Color).rgb;
	sur.color *= basecolor;
	float3 diffuselight = 0;
	for(int index=0;index<MAX_VISIBLE_LIGHTS;index++)
	{
		Light light = GetPerObjectLightIndex(index);
		diffuselight += DiffuseLight(light,sur);
	}
	sur.color  = diffuselight*sur.color;
	return  float4 (sur.color,1);
}
#endif