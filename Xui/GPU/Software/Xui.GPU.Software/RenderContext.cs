using System;
using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Software;

/// <summary>
/// Orchestrates the software rendering pipeline.
/// </summary>
/// <remarks>
/// RenderContext manages the rendering state and executes vertex and fragment shaders
/// to produce pixels in a framebuffer.
/// </remarks>
public unsafe class RenderContext
{
    private readonly Framebuffer _framebuffer;
    private Viewport _viewport;
    private PrimitiveTopology _topology;
    private bool _depthTestEnabled;
    private bool _blendEnabled;

    /// <summary>
    /// Gets the framebuffer associated with this render context.
    /// </summary>
    public Framebuffer Framebuffer => _framebuffer;

    /// <summary>
    /// Gets or sets the viewport for coordinate transformation.
    /// </summary>
    public Viewport Viewport
    {
        get => _viewport;
        set => _viewport = value;
    }

    /// <summary>
    /// Gets or sets the primitive topology.
    /// </summary>
    public PrimitiveTopology Topology
    {
        get => _topology;
        set => _topology = value;
    }

    /// <summary>
    /// Gets or sets whether depth testing is enabled.
    /// </summary>
    public bool DepthTestEnabled
    {
        get => _depthTestEnabled;
        set => _depthTestEnabled = value;
    }

    /// <summary>
    /// Gets or sets whether blending is enabled.
    /// </summary>
    public bool BlendEnabled
    {
        get => _blendEnabled;
        set => _blendEnabled = value;
    }

    /// <summary>
    /// Creates a new render context with the specified framebuffer.
    /// </summary>
    /// <param name="framebuffer">The framebuffer to render into.</param>
    public RenderContext(Framebuffer framebuffer)
    {
        _framebuffer = framebuffer;
        _viewport = Viewport.FromFramebuffer(framebuffer);
        _topology = PrimitiveTopology.TriangleList;
        _depthTestEnabled = true;
        _blendEnabled = false;
    }

    /// <summary>
    /// Clears the framebuffer color to the specified value.
    /// </summary>
    /// <param name="color">Color to clear to.</param>
    public void ClearColor(Color4 color)
    {
        var rgba32 = ColorTarget.ToRgba32(color);
        _framebuffer.ClearColor(rgba32);
    }

    /// <summary>
    /// Clears the framebuffer depth to the specified value.
    /// </summary>
    /// <param name="depth">Depth value to clear to (default 1.0).</param>
    public void ClearDepth(float depth = 1.0f)
    {
        if (_framebuffer.HasDepthBuffer)
            _framebuffer.ClearDepth(depth);
    }

    /// <summary>
    /// Renders vertices through the specified vertex and fragment shaders.
    /// </summary>
    /// <typeparam name="TVertex">The vertex input type.</typeparam>
    /// <typeparam name="TVarying">The varying data type (vertex output / fragment input).</typeparam>
    /// <typeparam name="TBindings">The shader bindings type.</typeparam>
    /// <param name="vertices">Source of vertex data.</param>
    /// <param name="vertexShader">Vertex shader implementation.</param>
    /// <param name="fragmentShader">Fragment shader implementation.</param>
    /// <param name="bindings">Shader bindings (uniforms, textures, etc.).</param>
    public void Draw<TVertex, TVarying, TBindings>(
        VertexSource<TVertex> vertices,
        IVertexShader<TVertex, TVarying, TBindings> vertexShader,
        IFragmentShader<TVarying, FragmentOutput, TBindings> fragmentShader,
        TBindings bindings)
        where TVertex : unmanaged
        where TVarying : unmanaged
        where TBindings : unmanaged
    {
        if (_topology != PrimitiveTopology.TriangleList)
            throw new NotImplementedException("Only TriangleList topology is currently supported");

        // Process triangles
        int triangleCount = vertices.Count / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int baseIndex = i * 3;
            
            // Get input vertices
            var v0 = vertices[baseIndex + 0];
            var v1 = vertices[baseIndex + 1];
            var v2 = vertices[baseIndex + 2];

            // Execute vertex shader for each vertex
            var varying0 = vertexShader.Execute(v0, bindings);
            var varying1 = vertexShader.Execute(v1, bindings);
            var varying2 = vertexShader.Execute(v2, bindings);

            // Rasterize and shade the triangle
            RasterizeTriangle(varying0, varying1, varying2, fragmentShader, bindings);
        }
    }

    /// <summary>
    /// Rasterizes a triangle and executes the fragment shader for each pixel.
    /// </summary>
    private void RasterizeTriangle<TVarying, TBindings>(
        TVarying varying0,
        TVarying varying1,
        TVarying varying2,
        IFragmentShader<TVarying, FragmentOutput, TBindings> fragmentShader,
        TBindings bindings)
        where TVarying : unmanaged
        where TBindings : unmanaged
    {
        // Extract positions from varyings
        // For now, we assume the first field of TVarying is a Float4 position
        // This is a temporary implementation - proper implementation will be in Phase 9
        var pos0 = GetPositionFromVarying(varying0);
        var pos1 = GetPositionFromVarying(varying1);
        var pos2 = GetPositionFromVarying(varying2);

        // Transform to screen space
        _viewport.Transform(pos0.X, pos0.Y, pos0.Z, out float x0, out float y0, out float z0);
        _viewport.Transform(pos1.X, pos1.Y, pos1.Z, out float x1, out float y1, out float z1);
        _viewport.Transform(pos2.X, pos2.Y, pos2.Z, out float x2, out float y2, out float z2);

        // Compute bounding box
        int minX = (int)Math.Max(0, Math.Floor(Math.Min(Math.Min(x0, x1), x2)));
        int maxX = (int)Math.Min(_framebuffer.Width - 1, Math.Ceiling(Math.Max(Math.Max(x0, x1), x2)));
        int minY = (int)Math.Max(0, Math.Floor(Math.Min(Math.Min(y0, y1), y2)));
        int maxY = (int)Math.Min(_framebuffer.Height - 1, Math.Ceiling(Math.Max(Math.Max(y0, y1), y2)));

        // Rasterize each pixel in the bounding box
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                float px = x + 0.5f;
                float py = y + 0.5f;

                // Compute barycentric coordinates
                if (ComputeBarycentricCoordinates(px, py, x0, y0, x1, y1, x2, y2, 
                    out float w0, out float w1, out float w2))
                {
                    // Interpolate depth
                    float depth = w0 * z0 + w1 * z1 + w2 * z2;

                    // Depth test
                    if (_depthTestEnabled && _framebuffer.HasDepthBuffer)
                    {
                        float currentDepth = _framebuffer.GetDepth(x, y);
                        if (depth >= currentDepth)
                            continue; // Failed depth test
                    }

                    // Interpolate varyings
                    var interpolated = InterpolateVarying(varying0, varying1, varying2, w0, w1, w2);

                    // Execute fragment shader
                    var output = fragmentShader.Execute(interpolated, bindings);

                    // Write color
                    var colorRgba32 = ColorTarget.ToRgba32(output.Color);
                    
                    if (_blendEnabled)
                    {
                        var existing = _framebuffer.GetColor(x, y);
                        colorRgba32 = ColorTarget.BlendRgba32(colorRgba32, existing);
                    }

                    _framebuffer.SetColor(x, y, colorRgba32);

                    // Write depth
                    if (_depthTestEnabled && _framebuffer.HasDepthBuffer)
                        _framebuffer.SetDepth(x, y, depth);
                }
            }
        }
    }

    /// <summary>
    /// Computes barycentric coordinates for a point relative to a triangle.
    /// </summary>
    /// <returns>True if the point is inside the triangle.</returns>
    private static bool ComputeBarycentricCoordinates(
        float px, float py,
        float x0, float y0,
        float x1, float y1,
        float x2, float y2,
        out float w0, out float w1, out float w2)
    {
        // Edge function approach
        float area = (y1 - y2) * (x0 - x2) + (x2 - x1) * (y0 - y2);
        
        if (Math.Abs(area) < 1e-6f)
        {
            // Degenerate triangle
            w0 = w1 = w2 = 0;
            return false;
        }

        float invArea = 1.0f / area;

        w0 = ((y1 - y2) * (px - x2) + (x2 - x1) * (py - y2)) * invArea;
        w1 = ((y2 - y0) * (px - x2) + (x0 - x2) * (py - y2)) * invArea;
        w2 = 1.0f - w0 - w1;

        // Point is inside if all weights are non-negative
        return w0 >= 0 && w1 >= 0 && w2 >= 0;
    }

    /// <summary>
    /// Extracts the position from a varying structure.
    /// </summary>
    /// <remarks>
    /// This is a temporary helper that assumes the first field is Float4 position.
    /// In a complete implementation, this would use reflection or source generation.
    /// </remarks>
    private static Float4 GetPositionFromVarying<TVarying>(TVarying varying) where TVarying : unmanaged
    {
        // Use unsafe pointer cast to read first Float4 field
        Float4* ptr = (Float4*)&varying;
        return *ptr;
    }

    /// <summary>
    /// Interpolates varying values using barycentric coordinates.
    /// </summary>
    /// <remarks>
    /// This is a simplified implementation that performs linear interpolation.
    /// A complete implementation would handle perspective-correct interpolation
    /// and use proper field-by-field interpolation.
    /// </remarks>
    private static TVarying InterpolateVarying<TVarying>(
        TVarying v0, TVarying v1, TVarying v2,
        float w0, float w1, float w2) where TVarying : unmanaged
    {
        // For now, use a simple linear interpolation on the raw bytes
        // This works for simple cases but isn't perspective-correct
        
        int size = sizeof(TVarying);
        TVarying result = default;
        
        byte* p0 = (byte*)&v0;
        byte* p1 = (byte*)&v1;
        byte* p2 = (byte*)&v2;
        byte* pResult = (byte*)&result;

        // Interpolate as floats (assumes varying is composed of floats)
        int floatCount = size / sizeof(float);
        for (int i = 0; i < floatCount; i++)
        {
            float* f0 = (float*)(p0 + i * sizeof(float));
            float* f1 = (float*)(p1 + i * sizeof(float));
            float* f2 = (float*)(p2 + i * sizeof(float));
            float* fResult = (float*)(pResult + i * sizeof(float));
            
            *fResult = (*f0) * w0 + (*f1) * w1 + (*f2) * w2;
        }

        return result;
    }
}
