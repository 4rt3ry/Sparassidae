

#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

float4x4 ViewProjection;
float2 UVScale;
float2 CameraOffset;
sampler TextureSampler : register(s0);

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

PixelInput SpriteVertexShader(VertexInput v) {
    PixelInput output;

    output.Position = mul(v.Position, ViewProjection);
    output.Color = v.Color;
    output.TexCoord = v.TexCoord;
    output.TexCoord.xy *= UVScale;
    output.TexCoord.xy -= CameraOffset;
    return output;
}
float4 SpritePixelShader(PixelInput p) : COLOR0 {
    float4 diffuse = tex2D(TextureSampler, p.TexCoord.xy);
    return diffuse * p.Color;
}

technique SpriteBatch {
    pass {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}

//#if OPENGL
//#define VS_SHADERMODEL vs_3_0
//#define PS_SHADERMODEL ps_3_0
//#else
//#define VS_SHADERMODEL vs_4_0
//#define PS_SHADERMODEL ps_4_0
//#endif

//sampler BaseTexture : register(s0);
////float2 ViewportSize;
////float Scale;
////float OffsetX;
////float OffsetY;

//struct VertexInput {
//    float4 Position : POSITION0;
//    float4 Color : COLOR0;
//    float4 TexCoord : TEXCOORD0;
//};
//struct PixelInput {
//    float4 Position : SV_Position0;
//    float4 Color : COLOR0;
//    float4 TexCoord : TEXCOORD0;
//};


//PixelInput SpriteVertexShader(VertexInput v) {
//	PixelInput output;

//	output.Position = v.Position;
//	output.Color = v.Color;
//	output.TexCoord = v.TexCoord;
//	//output.TexCoord.xy /= ViewportSize;
//	//output.TexCoord.xy;

//	return output;
//};

//float4 SpritePixelShader(PixelInput p) : COLOR0 {
//	float4 diffuse = tex2D(BaseTexture, p.TexCoord.xy);
//	return diffuse * p.Color;
//};

//technique SpriteBatch {
//	pass {
//		VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
//		PixelShader = compile PS_SHADERMODEL SpritePixelShader();
//	}
//}

//struct VertexInput {
//    float4 Position : POSITION0;
//    float4 Color : COLOR0;
//    float4 TexCoord : TEXCOORD0;
//};

//struct PixelInput {
//    float4 Position : SV_Position0;
//    float4 Color : COLOR0;
//    float4 TexCoord : TEXCOORD0;
//};

//PixelInput SpriteVertexShader(VertexInput v) {
//    PixelInput output;

//    output.Position = mul(v.Position, view_projection);
//    output.Color = v.Color;
//    output.TexCoord = mul(v.TexCoord, uv_transform);
//    return output;
//}
//float4 SpritePixelShader(PixelInput p) : COLOR0 {
//    float4 diffuse = tex2D(BaseTexture, p.TexCoord.xy);

//    // if (p.TexCoord.x < 0 || p.TexCoord.x > 1) {
//    //     discard;
//    // }

//    // if (p.TexCoord.y < 0 || p.TexCoord.y > 1) {
//    //     discard;
//    // }

//    return diffuse * p.Color;
//}

//technique SpriteBatch {
//    pass {
//        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
//        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
//    }
//}