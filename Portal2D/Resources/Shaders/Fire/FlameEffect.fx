float4x4 viewProjection  : ViewProjection;
texture Flame_Tex : Diffuse;
texture Noise_Tex;

struct Vertex
{
    float4 Position: POSITION;
    float4 Color: COLOR;
    float2 TexCoord : TEXCOORD0;
};

Vertex VS_main(Vertex v)
{
    Vertex result;
    //  2D vertices are already pre-multiplied times the world matrix.
    result.Position = mul(v.Position, viewProjection);
    result.Color = v.Color;
    result.TexCoord = v.TexCoord;

    return result;
}

float time;
float sideFade = 18.60;
float sideFadeSharpness = 0.01;
float wobbleScale = 0.03;
float burnSpeed = 0.44;
float randomnessRate = 0.29;
float yFade = 0.80;
float xScale = 2.07;
float yScale = 0.51;

sampler Flame = sampler_state
{
   Texture = (Flame_Tex);
   ADDRESSU = CLAMP;
   ADDRESSV = CLAMP;
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
   MIPFILTER = LINEAR;
};

sampler Noise = sampler_state
{
   Texture = (Noise_Tex);
   ADDRESSU = WRAP;
   ADDRESSV = WRAP;
   ADDRESSW = WRAP;
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
   MIPFILTER = LINEAR;
};

float4 PS_main(float2 texCoord: TEXCOORD0) : COLOR 
{
   // Wobble for the noise to get a more realistic appearance
   float wobbX = 2 * cos(6 * texCoord.x + time);
   float wobbY = 7 * (1 - texCoord.y) * (1 - texCoord.y);
   float wobble = sin(time + wobbX + wobbY);


   float3 coord;
   // Wobble more in the flames than at the base
   coord.x = xScale * texCoord.x + wobbleScale * (texCoord.y + 1) * wobble;
   // Create an upwards movement
   coord.y = yScale * texCoord.y - burnSpeed * time;
   // Move in Z to get some randomness
   coord.z = randomnessRate * time;
   float noisy = tex3D(Noise, coord);

   // Define the shape of the fire
   float t = sideFadeSharpness * (1 - sideFade * texCoord.x * texCoord.x);

   // Get the color out of it all
   float heat = saturate(t + noisy - yFade * texCoord.y);
   float4 flame = tex1D(Flame, heat);

   return flame;
}


//--------------------------------------------------------------//
// Technique Section for Fire Effects
//--------------------------------------------------------------//
technique Inferno
{
   pass Single_Pass
   {     
      VertexShader = compile vs_1_1 VS_main();
      PixelShader = compile ps_2_0 PS_main();
   }

}

