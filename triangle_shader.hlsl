// Generated HLSL shader code
// Module: TriangleShader

struct TriangleVertex
{
    float2 Position : TEXCOORD0;
    float4 Color : TEXCOORD1;
};

struct TriangleVaryings
{
    float4 Position : SV_POSITION;
    float4 Color : TEXCOORD0;
};

struct FragmentOutput
{
    float4 Color : TEXCOORD0;
};

// Vertex Shader: TriangleVertexShader
TriangleVaryings VSMain(TriangleVertex input)
{
    TriangleVaryings output;
    output.Position = float4(input.Position, 0f, 1f);
    output.Color = input.Color;
    return output;
}

// Pixel Shader: TriangleFragmentShader
FragmentOutput PSMain(TriangleVaryings input)
{
    FragmentOutput output;
    output.Color = input.Color;
    return output;
}

