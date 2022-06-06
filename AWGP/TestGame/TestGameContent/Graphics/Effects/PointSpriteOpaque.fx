matrix World;
matrix View;
matrix Projection;
matrix WorldView;

float FarClip;

texture AlbedoTexture;
sampler Albedo : register(s0);

texture NormalTexture;
sampler Normal : register(s1);

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4	Position	: POSITION0;
	float	Depth		: TEXCOORD0;
	float2	TexCoord	: TEXCOORD1;
};

struct PixelShaderOutput
{
	float4  RT0 : COLOR0;
	float4  RT1 : COLOR1;
	float4	RT2	: COLOR2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput Input)
{
	VertexShaderOutput Output = (VertexShaderOutput)0;

	float4 viewPosition = mul(Input.Position, WorldView);
	
	Output.Position = mul(viewPosition, Projection);
	Output.Depth = viewPosition.z;
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
	
	//float3 normal = normalize(tex2D(Normal, Input.TexCoord).xyz);
	//float3 normalVS = normalize(mul( float4(normal, 0.0f), View));
	float3 normal = mul( normalize(tex2D(Normal, Input.TexCoord).xyz), WorldView );

	Output.RT0 = float4( -Input.Depth / FarClip, 0.0f, 0.0f, 0.0f ); 
	//Output.RT1 = float4( normalize(tex2D(Normal, Input.TexCoord).xyz), 0.0f );
	//Output.RT1 = float4( float3( -normalVS.x, normalVS.y, -normalVS.z ), 0.0f );
	Output.RT1 = float4( normal, 0.0f );
	Output.RT2 = float4( tex2D( Albedo, Input.TexCoord ).rgb, 0.0f );
	
	return Output;
}

technique Technique1
{
	pass Pass1
	{
		CullMode = CCW;

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
