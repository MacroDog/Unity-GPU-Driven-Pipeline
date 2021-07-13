#ifndef MYRP_LIT_INPUT_INCLUDED
#define MYRP_LIT_INPUT_INCLUDED
#define MAX_VISIBLE_LIGHTS 4
float4x4 unity_MatrixVP;
CBUFFER_START(_LightBuffer)
	float4 _VisibleLightColors[MAX_VISIBLE_LIGHTS];
	float4 _VisibleLightDirections[MAX_VISIBLE_LIGHTS];
CBUFFER_END
CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
	float4x4 unity_worldToObject;
	float4 unity_LODFade;
	real4 unity_WorldTransformParams;
CBUFFER_END
#endif
