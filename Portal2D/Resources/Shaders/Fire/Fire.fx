//**************************************************************//
//  Effect File exported by RenderMonkey 1.6
//
//  - Although many improvements were made to RenderMonkey FX  
//    file export, there are still situations that may cause   
//    compilation problems once the file is exported, such as  
//    occasional naming conflicts for methods, since FX format 
//    does not support any notions of name spaces. You need to 
//    try to create workspaces in such a way as to minimize    
//    potential naming conflicts on export.                    
//    
//  - Note that to minimize resulting name collisions in the FX 
//    file, RenderMonkey will mangle names for passes, shaders  
//    and function names as necessary to reduce name conflicts. 
//**************************************************************//

//--------------------------------------------------------------//
// Fire Effects
//--------------------------------------------------------------//
//--------------------------------------------------------------//
// Inferno
//--------------------------------------------------------------//
//--------------------------------------------------------------//
// Single Pass
//--------------------------------------------------------------//
string Fire_Effects_Inferno_Single_Pass_ScreenAlignedQuad : ModelData = "D:/Program Files/ATI Research Inc/RenderMonkey 1.6/Examples/Media/Models/ScreenAlignedQuad.3ds";

struct VS_OUTPUT 
{
   float4 Pos: POSITION;
   float2 texCoord: TEXCOORD0;
};

VS_OUTPUT Fire_Effects_Inferno_Single_Pass_Vertex_Shader_main(float4 Pos: POSITION)
{
   VS_OUTPUT Out;

   // Clean up inaccuracies
   Pos.xy = sign(Pos.xy);

   Out.Pos = float4(Pos.xy, 0, 1);
   // Image-space
   Out.texCoord = Pos.xy;

   return Out;
}











float time_0_X : Time0_X;
float sideFade
<
   string UIName = "sideFade";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 30.00;
> = 18.60;
float sideFadeSharpness
<
   string UIName = "sideFadeSharpness";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 1.00;
> = 0.05;
float wobbleScale
<
   string UIName = "wobbleScale";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 0.10;
> = 0.03;
float burnSpeed
<
   string UIName = "burnSpeed";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 1.00;
> = 0.44;
float randomnessRate
<
   string UIName = "randomnessRate";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 1.00;
> = 0.29;
float yFade
<
   string UIName = "yFade";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 2.00;
> = 0.80;
float xScale
<
   string UIName = "xScale";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 3.00;
> = 2.07;
float yScale
<
   string UIName = "yScale";
   string UIWidget = "Numeric";
   bool UIVisible = " true";
   float UIMin = 0.00;
   float UIMax = 1.50;
> = 0.51;

texture Flame_Tex
<
   string ResourceName = "D:\Program Files\ATI Research Inc\RenderMonkey 1.6\Examples\Media\Textures\Flame.tga";
>;
sampler Flame = sampler_state
{
   Texture = (Flame_Tex);
   ADDRESSU = CLAMP;
   ADDRESSV = CLAMP;
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
   MIPFILTER = LINEAR;
};
texture Noise_Tex
<
   string ResourceName = "D:\Program Files\ATI Research Inc\RenderMonkey 1.6\Examples\Media\Textures\NoiseVolume.dds";
>;
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

float4 Fire_Effects_Inferno_Single_Pass_Pixel_Shader_main(float2 texCoord: TEXCOORD0) : COLOR 
{
   // Wobble for the noise to get a more realistic appearance
   float wobbX = 2 * cos(6 * texCoord.x + time_0_X);
   float wobbY = 7 * (1 - texCoord.y) * (1 - texCoord.y);
   float wobble = sin(time_0_X + wobbX + wobbY);
   // Alternative approach
   //   float wobble = 9 * (2 * tex3D(Noise, float3(texCoord * 0.4, 0.2 * time_0_X)) - 1);

   float3 coord;
   // Wobble more in the flames than at the base
   coord.x = xScale * texCoord.x + wobbleScale * (texCoord.y + 1) * wobble;
   // Create an upwards movement
   coord.y = yScale * texCoord.y - burnSpeed * time_0_X;
   // Move in Z to get some randomness
   coord.z = randomnessRate * time_0_X;
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
      CULLMODE = NONE;

      VertexShader = compile vs_1_1 Fire_Effects_Inferno_Single_Pass_Vertex_Shader_main();
      PixelShader = compile ps_2_0 Fire_Effects_Inferno_Single_Pass_Pixel_Shader_main();
   }

}

