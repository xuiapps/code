// Generated MSL shader code
// Module: TriangleShader

#include <metal_stdlib>
using namespace metal;

struct TriangleVertex
{
    float2 Position [[user(locn0)]];
    float4 Color [[user(locn1)]];
};

struct TriangleVaryings
{
    float4 Position [[position]];
    float4 Color [[user(locn0)]];
};

struct FragmentOutput
{
    float4 Color [[user(locn0)]];
};

// Vertex Shader: TriangleVertexShader
[[vertex]] TriangleVaryings vertex_main(TriangleVertex input [[stage_in]])
{
    TriangleVaryings output;
    output.Position = float4(input.Position, 0f, 1f);
    output.Color = input.Color;
    return output;
}

// Fragment Shader: TriangleFragmentShader
[[fragment]] FragmentOutput fragment_main(TriangleVaryings input [[stage_in]])
{
    FragmentOutput output;
    output.Color = input.Color;
    return output;
}

