//Input variables
float4x4 viewProjection  : ViewProjection;
texture diffuseTexture : Diffuse;
float4 tcDownSampleOffsets[16];

sampler TextureSampler = sampler_state
{
    texture = <diffuseTexture>;
    AddressU  = CLAMP;
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = POINT;
    MINFILTER = POINT;
    MAGFILTER = POINT;
};

struct Vertex
{
    float4 Position: POSITION;
    float4 Color: COLOR;
    float2 TexCoord : TEXCOORD0;
};

Vertex VS_BrightPass(Vertex v)
{
    Vertex result;
    //  2D vertices are already pre-multiplied times the world matrix.
    result.Position = mul(v.Position, viewProjection);
    result.Color = v.Color;
    result.TexCoord = v.TexCoord;
    return result;
}

float4 PS_BrightPass(Vertex v): COLOR
{    
    float4 average = { 0.0f, 0.0f, 0.0f, 0.0f };

    for( int i = 0; i < 16; i++ )
    {
        average += tex2D( TextureSampler, v.TexCoord + float2(tcDownSampleOffsets[i].x, tcDownSampleOffsets[i].y) );
    }
        
    average *= ( 1.0f / 16.0f );

    return average;
}

technique BrightPass
{
   pass p0
   {
        VertexShader = compile vs_2_0 VS_BrightPass();
        PixelShader  = compile ps_2_0 PS_BrightPass();
   }
}