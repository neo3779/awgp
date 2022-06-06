matrix WorldViewProjection;

texture AlbedoTexture;
sampler Albedo : register(s0);

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4	Position	: POSITION0;
	float2	Depth		: TEXCOORD0;
	float2	TexCoord	: TEXCOORD1;
};

struct PixelShaderOutput
{
	float4  RT0 : COLOR0;
	float4  RT1 : COLOR1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput Input)
{
	VertexShaderOutput Output = (VertexShaderOutput)0;
	
	Output.Position = mul(Input.Position, WorldViewProjection);
	Output.Depth.xy = Output.Position.zw;
	Output.TexCoord = Input.TexCoord;

	return Output;
}

PixelShaderOutput PixelShaderFunction(VertexShaderOutput Input)
{
	PixelShaderOutput Output = (PixelShaderOutput)0;
	
	if ( tex2D( Albedo, Input.TexCoord ).a < 0.5f )
	{
		discard;
	}
	
	Output.RT0 = float4( Input.Depth.x / Input.Depth.y, 0.0f, 0.0f, 0.0f ); 
	Output.RT1 = float4( tex2D( Albedo, Input.TexCoord ).rgb, 0.0f );
	
	return Output;
}

technique Technique1
{
	pass Pass1
	{
		CullMode = CCW;
		ZWriteEnable = true;
		ZEnable = true;

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
