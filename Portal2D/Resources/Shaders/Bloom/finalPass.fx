//Input variables
float4x4 viewProjection  : ViewProjection;
texture diffuseTexture : Diffuse;
texture BloomRender : Diffuse;

float       fExposure;                          // A user configurable bias to under/over expose the image
float       fGaussianScalar;                    // Used in the post-processing, but also useful here
float       g_rcp_bloom_tex_w;                  // The reciprocal WIDTH of the texture in 'bloom'
float       g_rcp_bloom_tex_h;                  // The reciprocal HEIGHT of the texture in 'bloom'           

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

sampler bloom = sampler_state
{
    texture = <BloomRender>;
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
    // Read the HDR value that was computed as part of the original scene
    float4 c = tex2D( TextureSampler, v.TexCoord );
    
    // Read the luminance value, target the centre of the texture
    // which will map to the only pixel in it!
    //float4 l = tex2D( luminance, float2( 0.5f, 0.5f ) );
        
    // Compute the blur value using a bilinear filter
    // It is worth noting that if the hardware supports linear filtering of a
    // floating point render target that this step can probably be skipped.
    float xWeight = frac( v.TexCoord.x / g_rcp_bloom_tex_w ) - 0.5f;
    float xDir = xWeight;
    xWeight = abs( xWeight );
    xDir /= xWeight;
    xDir *= g_rcp_bloom_tex_w;

    float yWeight = frac( v.TexCoord.y / g_rcp_bloom_tex_h ) - 0.5f;
    float yDir = yWeight;
    yWeight = abs( yWeight );
    yDir /= yWeight;
    yDir *= g_rcp_bloom_tex_h;

    // sample the blur texture for the 4 relevant pixels, weighted accordingly
    float4 b = ((1.0f - xWeight) * (1.0f - yWeight))    * tex2D( bloom, v.TexCoord );  
                  
    b +=       (xWeight * (1.0f - yWeight))             * tex2D( bloom, v.TexCoord + float2( xDir, 0.0f ) );
    b +=       (yWeight * (1.0f - xWeight))             * tex2D( bloom, v.TexCoord + float2( 0.0f, yDir ) );
    b +=       (xWeight * yWeight)                      * tex2D( bloom, v.TexCoord + float2( xDir, yDir ) );
  	
  	//return tex2D( bloom, v.TexCoord );
  	
  	
    // Compute the actual colour:
    float4 final = c + 0.80f * b;
 
            
    // Reinhard's tone mapping equation (See Eqn#3 from 
    // "Photographic Tone Reproduction for Digital Images" for more details) is:
    //
    //      (      (   Lp    ))
    // Lp * (1.0f +(---------))
    //      (      ((Lm * Lm)))
    // -------------------------
    //         1.0f + Lp
    //
    // Lp is the luminance at the given point, this is computed using Eqn#2 from the above paper:
    //
    //        exposure
    //   Lp = -------- * HDRPixelIntensity
    //          l.r
    //
    // The exposure ("key" in the above paper) can be used to adjust the overall "balance" of 
    // the image. "l.r" is the average luminance across the scene, computed via the luminance
    // downsampling process. 'HDRPixelIntensity' is the measured brightness of the current pixel
    // being processed.
    
    //float Lp = (fExposure / l.r) * max( final.r, max( final.g, final.b ) );
    
    // A slight difference is that we have a bloom component in the final image - this is *added* to the 
    // final result, therefore potentially increasing the maximum luminance across the whole image. 
    // For a bright area of the display, this factor should be the integral of the bloom distribution 
    // multipled by the maximum value. The integral of the gaussian distribution between [-1,+1] should 
    // be AT MOST 1.0; but the sample code adds a scalar to the front of this, making it a good enough
    // approximation to the *real* integral.
    
    //float LmSqr = (l.g + fGaussianScalar * l.g) * (l.g + fGaussianScalar * l.g);
    
    // Compute Eqn#3:
    //float toneScalar = ( Lp * ( 1.0f + ( Lp / ( LmSqr ) ) ) ) / ( 1.0f + Lp );
    
    // Tonemap the final outputted pixel:
    //c = final * toneScalar;
    
    // Return the fully composed colour
    final.a = 1.0f;
    return final;
    
}

technique VertBloom
{
   pass p0
   {
        VertexShader = compile vs_2_0 VS_VertBloom();
        PixelShader  = compile ps_2_0 PS_VertBloom();
   }
}