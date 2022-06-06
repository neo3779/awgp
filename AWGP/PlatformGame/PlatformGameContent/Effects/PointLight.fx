matrix View;
matrix WorldViewProjection;
matrix ProjectionInverse;

float Radius;
float3 LightPosition;
float LightIntensity;
float3 LightColour;
float DistanceMax;
float2 ScreenDimensions;

texture RT0Texture;
sampler RT0  : register(s0);

texture RT1Texture;
sampler RT1 : register(s1);

float4 VS( float4 Position : POSITION0 ) : POSITION0
{
	return mul(Position, WorldViewProjection);
}

float4 PS( float2 ScreenSpace : VPOS ) : COLOR0
{
	// dchapman: Use VPOS semantic to generate texture coordinate for this screen-space pixel - 01/11/2011
	float2 TexCoords = float2( ScreenSpace / ScreenDimensions );

	// dchapman: Reconstruct view-space position from the post-projection depth value stored in the GBuffer - 20/12/2011
	float2 viewportPos = float2(TexCoords.x * 2 - 1, (1-TexCoords.y) * 2 - 1);
	float4 projectedPos = float4( viewportPos.x, viewportPos.y, tex2D(RT0, TexCoords).r, 1.0f );
	float4 surfacePosVS = mul( projectedPos, ProjectionInverse );
	surfacePosVS.xyz /= surfacePosVS.w;

	// dchapman: Calculate the view-space position of the light and calculate the vector from the surface to the light source - 05/01/2012
	float3 lightPosVS = mul(float4(LightPosition, 1.0f), View);
	float3 surfaceToLight = lightPosVS - surfacePosVS;

	// dchapman: Cull any pixels outside the maximum distance for this light, as they will not be affected by it - 05/01/2012
	float distance = length( surfaceToLight );
	if ( distance >= DistanceMax )
		discard;

	// dchapman: Transform the distance such that the attenuation is;
	//			 - Zero at the maximum distance or longer, 
	//			 - One at the radius or smaller, 
	//			 - Exponentially decays with distance between these two points - 05/01/2012
	float distanceTransformed = distance / ( 1 - pow(distance / DistanceMax, 2) ); 
	float attenuation = (1 / pow( (distanceTransformed / Radius) + 1, 2));

	// dchapman: Assume a constant view-space normal, facing towards the camera - 05/01/2012
	float3 normal = float3( 0, 0, 1 );

	// dchapman: Sample the albedo colour of the surface at this pixel - 05/01/2012
	float3 albedo = tex2D(RT1, TexCoords).xyz;
	
	// dchapman: Calculate the direction from surface to light by normalizing that vector - 05/01/2012
	float3 toLight = normalize(surfaceToLight);
	
	// dchapman: Calculate the lambert cosine term for the normal and light vectors - 05/01/2012
	float ndotl = max(dot(normal, toLight),0);

	// dchapman: Calculate the diffuse lighting at this point, modified by the light's colour, intensity and distance attenuation - 05/01/2012
	float3 diffuse = ndotl * LightColour * LightIntensity * attenuation;
	
	// dchapman: Combine these terms into the final lighting result at this surface pixel for this light - 05/01/2012
	return float4( diffuse * albedo, 1.0f );
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
