matrix World;
matrix View;
matrix WorldView;
matrix ViewProjection;
matrix WorldViewProjection;

float Radius;
float3 LightPosition;
float LightIntensity;
float3 LightColour;
float DistanceMax;
float2 ScreenDimensions;
float TanHalfFOV;
float FarClip;

texture RT0Texture;
sampler RT0  : register(s0);

texture RT1Texture;
sampler RT1 : register(s1);

texture RT2Texture;
sampler RT2  : register(s2);

//sampler IntensityMap : register(s2);

struct VS_INPUT
{
	float4 Position : POSITION0;
};

struct VS_OUTPUT
{
	float4 Position		: POSITION0;
	float4 PositionVS	: TEXCOORD0;
	//float3 EyeRayVS		: TEXCOORD0;
};

VS_OUTPUT VS(VS_INPUT Input) 
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	
	//float4 texelToPixelOffset = float4( 0.5f * (1.0f / ScreenDimensions.x), 0.5f * (1.0f / ScreenDimensions.y), 0.0f, 0.0f);

	Output.Position = mul(Input.Position, WorldViewProjection);
	Output.PositionVS = mul(Input.Position, WorldView );
	//Output.EyeRayVS = float3(	Output.Position.x * (ScreenDimensions.x / ScreenDimensions.y),
								//Output.Position.y,
								//TanHalfFOV );
	//Output.EyeRayVS = float3(	Output.Position.x * TanHalfFOV * (ScreenDimensions.x / ScreenDimensions.y),
								//Output.Position.y * TanHalfFOV,
								//Output.Position.w	);
	return Output;
}

float4 PS(VS_OUTPUT Input, float2 ScreenSpace : VPOS ) : COLOR0
{
	// dchapman: Use VPOS semantic to generate texture coordinate for this screen-space pixel - 01/11/2011
	float2 TexCoords = float2( ScreenSpace / ScreenDimensions );

	// dchapman: Reconstruct position from depth stored in the GBuffer - 31/10/2011
	float3 frustumRay = Input.PositionVS.xyz * ( FarClip / -Input.PositionVS.z );
	float depth = tex2D(RT0, TexCoords).r;
	float3 surfacePosVS = frustumRay * depth;

	float3 lightPosVS = mul(float4(LightPosition, 1.0f), View);
	float3 surfaceToLight = lightPosVS - surfacePosVS;

	float distance = length( surfaceToLight );
	if ( distance >= DistanceMax )
		discard;

	float distanceTransformed = distance / ( 1 - pow(distance / DistanceMax, 2) ); 
	float attenuation = (1 / pow( (distanceTransformed / Radius) + 1, 2));

	float3 normal = normalize(tex2D(RT1, TexCoords));
	float3 albedo = tex2D(RT2, TexCoords).xyz;
	//float3 emissive = albedo * tex2D(RT0, Input.TexCoord).a;
	float3 emissive = float3(0,0,0);
	
	float3 toLight = normalize(surfaceToLight);
	float ndotl = max(dot(normal, toLight),0);
	float3 diffuse = ndotl * LightColour * LightIntensity * attenuation;
	
	float3 lightReflect = normalize(reflect( toLight, normal ));
	float3 viewVector = normalize(surfacePosVS);

	float rdotv = max(dot(lightReflect, viewVector), 0);
	float3 specular = rdotv * LightColour * LightIntensity * attenuation;
	
	return float4(max(saturate((diffuse * albedo) + (specular * albedo)), emissive), 1.0f);
}

technique Technique1
{
	pass Pass1
	{
		CullMode = NONE;

		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
	}
}
