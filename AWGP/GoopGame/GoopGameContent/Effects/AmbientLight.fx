float LightIntensity;
float3 LightColour;

texture RT1Texture;
sampler RT1	: register(s0);

struct VS_INPUT
{
	float4 Position : POSITION0;
	float2 TexCoord	: TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 Position	: POSITION0;
	float2 TexCoord	: TEXCOORD0;
};

VS_OUTPUT VS(VS_INPUT Input) 
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	
	Output.Position = Input.Position;
	Output.TexCoord = Input.TexCoord;

	return Output;
}

float4 PS(VS_OUTPUT Input, float2 ScreenSpace : VPOS ) : COLOR0
{
	float3 albedo = tex2D(RT1, Input.TexCoord).xyz;
	return float4( albedo * LightColour * LightIntensity , 1.0f );
}

technique Technique1
{
	pass Pass1
	{
		CullMode = NONE;
		ZWriteEnable = false;

		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
	}
}