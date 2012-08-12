//Input variables
float4x4 viewProjection  : ViewProjection;
texture diffuseTexture : Diffuse;

float VBloomWeights[9];               
float VBloomOffsets[9];                

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

Vertex VS_VertBloom(Vertex v)
{
    Vertex result;
    //  2D vertices are already pre-multiplied times the world matrix.
    result.Position = mul(v.Position, viewProjection);
    result.Color = v.Color;
    result.TexCoord = v.TexCoord;
    return result;
}

float4 PS_VertBloom(Vertex v): COLOR
{    
    float4 color = { 0.0f, 0.0f, 0.0f, 0.0f };
    
    for( int i = 0; i < 9; i++ )
    {
        color += (tex2D( TextureSampler, v.TexCoord + float2( 0.0f, VBloomOffsets[i] ) ) * VBloomWeights[i] );
    }
        
    return float4( color.rgb, 1.0f );
}

technique VertBloom
{
   pass p0
   {
        VertexShader = compile vs_2_0 VS_VertBloom();
        PixelShader  = compile ps_2_0 PS_VertBloom();
   }
}