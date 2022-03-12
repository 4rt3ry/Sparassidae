// References: https://learn-monogame.github.io/tutorial/first-shader/
//             https://gamedev.stackexchange.com/questions/38118/best-way-to-mask-2d-sprites-in-xna

#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

sampler BaseTexture : register(s0);

Texture2D Mask;
sampler MaskTexture {
    Texture = ( Mask );
    addressU = Wrap;
    addressV = Wrap;
};

// Mask/Base Texture Parameters
float MaskLocationX;
float MaskLocationY;
float MaskWidth;
float MaskHeight;
float BaseTextureWidth;
float BaseTextureHeight;

struct VertexInput {
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};
struct PixelInput {
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};

float4 SpritePixelShader(PixelInput p) : COLOR0 {
    //We need to calculate where in terms of percentage to sample from the MaskTexture
    float maskPixelX = p.TexCoord.x * BaseTextureWidth;
    float maskPixelY = p.TexCoord.y * BaseTextureHeight;
    float2 maskCoord = float2((maskPixelX - MaskLocationX) / MaskWidth, (maskPixelY - MaskLocationY) / MaskHeight);
    float4 bitMask = tex2D(MaskTexture, maskCoord);

    float4 tex = tex2D(BaseTexture, p.TexCoord.xy);

    return tex * float4(bitMask.r, bitMask.r, bitMask.r, bitMask.r);
}

technique SpriteBatch {
    pass {
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}