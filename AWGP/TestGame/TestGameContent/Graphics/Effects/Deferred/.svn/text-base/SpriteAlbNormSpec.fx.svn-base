sampler AlbedoSampler : register(s0);
sampler NormalSampler : register(s1);
sampler SpecularSampler : register(s2);

matrix world;

float depth;

struct PS_OUTPUT
{
	float4 RT0 : COLOR0;
	float4 RT1 : COLOR1;
};

PS_OUTPUT PS(float2 TexCoord : TEXCOORD0)
{
	PS_OUTPUT Output = (PS_OUTPUT)0;
	if ( tex2D( NormalSampler, TexCoord).a <= 0.0f )
		discard;

		//tex2D(AlbedoSampler, TexCoord ).a
	Output.RT0 = float4( tex2D( AlbedoSampler, TexCoord ).xyz, 0.1f );	// Albedo.RGB, Emissive.A
	Output.RT1 = float4( mul( float4(tex2D( NormalSampler, TexCoord ).xyz, 0.0f), world).xyz, depth );	// Normals.RGB, Depth.A

	return Output;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PS();
	}
}
