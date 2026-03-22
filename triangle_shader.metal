// Generated MSL shader code
// Module: TriangleShader

#include <metal_stdlib>
using namespace metal;

struct TriangleVertex
{
    float2 Position;
    float4 Color;
};

struct TriangleVaryings
{
    float4 Position [[position]];
    float4 Color [[user(locn0)]];
};

struct FragmentOutput
{
    float4 Color [[color(0)]];
};

// Vertex Shader: TriangleVertexShader
[[vertex]] TriangleVaryings vertex_main(device const TriangleVertex* vertices [[buffer(0)]], uint vertexId [[vertex_id]])
{
    TriangleVertex input = vertices[vertexId];
    TriangleVaryings output;
    output.Position = float4(input.Position, 0.0f, 1.0f);
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

