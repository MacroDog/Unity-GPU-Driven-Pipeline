#ifndef MYRP_SHADOW_CASTER_INCLUDED
#define MYRP_SHADOW_CASTER_INCLUDED
#define MAX_VISIBLE_LIGHTS 4
TEXTURE2D(_BaseMap);            SAMPLER(sampler_BaseMap);
CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
CBUFFER_END

UNITY_INSTANCING_BUFFER_START(Props)
	UNITY_DEFINE_INSTANCED_PROP(float4 , _Color)
UNITY_INSTANCING_BUFFER_END(Props)

struct VertexInput {
	float4 pos 			: POSITION;
};

struct VertexOutput {
	float4 pos 	: SV_POSITION;
};

struct Light
{
	//direction or position
    half3   direction;
    half3   color;
    half    distanceAttenuation;
    half    shadowAttenuation;
};
// TEXTURE2D(_MainTex);            SAMPLER(sampler_MainTex);


VertexOutput ShadowCasterPassVertex (VertexInput i) {
	VertexOutput o;
	UNITY_SETUP_INSTANCE_ID(i);
	float4 worldPos = mul(UNITY_MATRIX_M, float4(i.pos.xyz, 1.0));
	o.pos = mul(UNITY_MATRIX_VP, worldPos);
	#if UNITY_REVERSED_Z
		o.pos.z = min(o.pos.z,o.pos.w * UNITY_NEAR_CLIP_VALUE);
	#else
		o.pos.z = max(o.pos.z,o.pos.w * UNITY_NEAR_CLIP_VALUE);
	#endif
	return o;
}

float4 ShadowCasterPassFragment (VertexOutput i) : SV_TARGET {
	return  0;
}
#endif