float Radius;
float2 LightPosition;
float LightIntensity;
float3 LightColour;
float DistanceMax;

sampler RT0 : register(s0);
sampler RT1 : register(s1);
//sampler IntensityMap : register(s2);

struct VS_INPUT
{
	float4 Position : POSITION;
};

struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
};

struct PS_INPUT
{
	float2 Position : VPOS;
	float2 TexCoord : TEXCOORD0;
};

VS_OUTPUT VS(VS_INPUT Input) 
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	Output.Position = Input.Position;
	Output.TexCoord = float2((Input.Position.x / 2) + 0.5f, 1.0f - ((Input.Position.y / 2) + 0.5f));
	return Output;
}

float4 PS(PS_INPUT Input) : COLOR0
{
	float distance = length(LightPosition - Input.Position);
	
	if ( distance >= DistanceMax )
		discard;
			
	float distanceTransformed = distance / ( 1 - pow(distance / DistanceMax, 2) ); 
	float attenuation = (1 / pow( (distanceTransformed / Radius) + 1, 2));

	float3 diffuse = LightColour * LightIntensity * attenuation;
	
	return float4(saturate(diffuse * tex2D(RT0, Input.TexCoord).xyz), 1.0f);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
	}
}
