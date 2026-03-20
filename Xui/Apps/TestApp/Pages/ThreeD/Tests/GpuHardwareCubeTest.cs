using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.DI;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.GPU.Hardware;
using Xui.GPU.IR;
using Xui.GPU.Shaders.Types;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.ThreeD.Tests;

/// <summary>
/// A 300x300 3D surface showing a rotating cube rendered via hardware GPU acceleration.
/// Uses Direct3D 11 on Windows, Metal on macOS/iOS, and falls back to software on other platforms.
/// Features click-and-drag rotation interaction.
/// </summary>
public class GpuHardwareCubeTest : View
{
    private NFloat rotationX = 0.3f;
    private NFloat rotationY = 0.3f;
    private Point? dragStart = null;
    private NFloat dragStartRotX = 0;
    private NFloat dragStartRotY = 0;

    // GPU resources
    private IGpuDevice? _gpuDevice;
    private IGpuRenderTarget? _renderTarget;
    private IGpuVertexShader? _vertexShader;
    private IGpuFragmentShader? _fragmentShader;
    private bool _gpuAvailable = false;
    private string _backendName = "None";

    // Display resources
    private IImage? _platformImage;
    private byte[]? _bgraBuffer;
    private bool _initialized = false;

    public GpuHardwareCubeTest()
    {
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType == PointerType.Mouse)
        {
            if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
            {
                this.CapturePointer(e.PointerId);
                dragStart = e.State.Position;
                dragStartRotX = rotationX;
                dragStartRotY = rotationY;
            }
            else if (e.Type == PointerEventType.Move && dragStart != null)
            {
                var delta = e.State.Position - dragStart.Value;
                rotationY = dragStartRotY + delta.X * 0.01f;
                rotationX = dragStartRotX + delta.Y * 0.01f;
                this.InvalidateRender();
            }
            else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
            {
                this.ReleasePointer(e.PointerId);
                dragStart = null;
            }
        }

        base.OnPointerEvent(ref e, phase);
    }

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        return new Size(300, 300);
    }

    protected override void RenderCore(IContext context)
    {
        // Initialize resources if needed
        if (!_initialized)
        {
            _bgraBuffer = new byte[300 * 300 * 4];
            _platformImage = this.GetService<IImage>();
            TryInitializeGpu();
            _initialized = true;
        }

        byte[]? pixels = null;

        if (_gpuAvailable && _gpuDevice != null && _renderTarget != null)
        {
            // Hardware GPU rendering path
            pixels = RenderWithGpu();
        }

        if (pixels == null)
        {
            // Fallback: software rendering
            pixels = RenderWithSoftware();
        }

        // Upload pixels to platform image and draw
        _platformImage?.LoadPixels(300, 300, pixels);
        var destRect = new Rect(this.Frame.X, this.Frame.Y, 300, 300);
        if (_platformImage != null)
            context.DrawImage(_platformImage, destRect);

        // Draw border
        context.SetStroke(Black);
        context.StrokeRect(destRect);

        // Draw backend label
        context.SetFill(new Color(0x00, 0x00, 0x00, 0xCC));
        context.SetFont(new Font(11, new[] { "Inter" }));
        context.FillText(
            $"GPU: {_backendName}",
            new Point(this.Frame.X + 6, this.Frame.Y + 16));
    }

    private void TryInitializeGpu()
    {
        // Resolve IGpuDevice via DI - provided by the platform window (D3D11 on Windows, Metal on macOS/iOS).
        _gpuDevice = this.GetService<IGpuDevice>();
        if (_gpuDevice == null)
        {
            _gpuAvailable = false;
            _backendName = "Software (fallback)";
            return;
        }

        // Resolve IShaderBackend via DI - provided by the platform window (HlslCodeGenerator on
        // Windows, MslCodeGenerator on macOS/iOS).
        var shaderBackend = this.GetService<Xui.GPU.Backends.IShaderBackend>();
        if (shaderBackend == null)
        {
            _gpuAvailable = false;
            _backendName = "Software (no shader backend)";
            return;
        }

        try
        {
            _backendName = _gpuDevice.BackendName;

            // Generate shader source via the platform-provided backend (IR → HLSL or MSL)
            var module = CreateCubeShaderModule();
            var shaderSource = shaderBackend.GenerateCode(module);

            // Determine entry point names from the backend
            string vsEntry = shaderBackend.Name == "MSL" ? "vertex_main" : "VSMain";
            string fsEntry = shaderBackend.Name == "MSL" ? "fragment_main" : "PSMain";

            _vertexShader = _gpuDevice.CompileVertexShader(shaderSource, vsEntry);
            _fragmentShader = _gpuDevice.CompileFragmentShader(shaderSource, fsEntry);

            _renderTarget = _gpuDevice.CreateRenderTarget(300, 300);
            _gpuAvailable = true;
        }
        catch (Exception ex)
        {
            // Hardware init failed, fall back to software
            _gpuAvailable = false;
            _backendName = $"Software (GPU init failed: {ex.Message[..Math.Min(ex.Message.Length, 40)]})";

            _vertexShader?.Dispose();
            _vertexShader = null;
            _fragmentShader?.Dispose();
            _fragmentShader = null;
            _renderTarget?.Dispose();
            _renderTarget = null;
            // NOTE: _gpuDevice is owned by the platform window via DI - do NOT dispose it here.
            _gpuDevice = null;
        }
    }

    private unsafe byte[] RenderWithGpu()
    {
        try
        {
            using var cmd = _gpuDevice!.CreateCommandList();

            // Begin render pass - clear to gray
            cmd.BeginRenderPass(_renderTarget!, new GpuClearColor(0.502f, 0.502f, 0.502f, 1.0f));

            // Set pipeline
            var pipeline = new GpuPipelineDesc
            {
                VertexShader = _vertexShader,
                FragmentShader = _fragmentShader,
                DepthTestEnabled = true,
                DepthWriteEnabled = true,
            };
            cmd.SetPipeline(pipeline);

            // Create cube vertices using the GPU shader pipeline
            var vertices = CreateCubeVertices();

            // Set constant buffer with MVP matrix
            var mvp = ComputeMvpMatrix();
            fixed (GpuCubeVertex* pVerts = vertices)
            {
                cmd.SetVertexBuffer(pVerts, new GpuVertexBufferDesc
                {
                    Stride = System.Runtime.InteropServices.Marshal.SizeOf<GpuCubeVertex>(),
                    VertexCount = vertices.Length
                });
                cmd.SetConstantBuffer(&mvp, sizeof(Float4x4));
                cmd.Draw(vertices.Length);
            }

            cmd.EndRenderPass();
            cmd.Commit();

            // Read back pixels (BGRA format)
            _renderTarget!.ReadbackPixelsBgra(_bgraBuffer!);
            return _bgraBuffer!;
        }
        catch
        {
            // GPU rendering failed, fall back to software
            _gpuAvailable = false;
            _backendName += " (render failed)";
            return RenderWithSoftware();
        }
    }

    private unsafe byte[] RenderWithSoftware()
    {
        // Simple software rendering fallback
        using var fb = new Xui.GPU.Software.Framebuffer(300, 300, withDepthBuffer: true);

        fb.ClearColor(0xFF808080); // Gray background
        fb.ClearDepth(1.0f);

        var cubeVerts = CreateCubeVertices();
        var mvp = ComputeMvpMatrix();

        var renderContext = new Xui.GPU.Software.RenderContext(fb);
        renderContext.DepthTestEnabled = true;

        fixed (GpuCubeVertex* pVerts = cubeVerts)
        {
            var vertexSource = new Xui.GPU.Software.VertexSource<GpuCubeVertex>(pVerts, cubeVerts.Length);
            var vs = new GpuCubeVertexShader();
            var fs = new GpuCubeFragmentShader();
            renderContext.Draw(vertexSource, vs, fs, new GpuCubeBindings { MVP = mvp });
        }

        // Convert to BGRA
        ConvertToBgra(fb, _bgraBuffer!);
        return _bgraBuffer!;
    }

    private static unsafe void ConvertToBgra(Xui.GPU.Software.Framebuffer fb, byte[] bgra)
    {
        uint* src = fb.ColorData;
        int count = fb.Width * fb.Height;
        for (int i = 0; i < count; i++)
        {
            uint px = src[i];
            bgra[i * 4 + 0] = (byte)(px >> 8);   // B
            bgra[i * 4 + 1] = (byte)(px >> 16);  // G
            bgra[i * 4 + 2] = (byte)(px >> 24);  // R
            bgra[i * 4 + 3] = (byte)(px);        // A
        }
    }

    private Float4x4 ComputeMvpMatrix()
    {
        float rotX = (float)rotationX;
        float rotY = (float)rotationY;

        const float cubeScale = 0.6f;
        var scaleMatrix = new Float4x4(
            new Float4(new F32(cubeScale), F32.Zero, F32.Zero, F32.Zero),
            new Float4(F32.Zero, new F32(cubeScale), F32.Zero, F32.Zero),
            new Float4(F32.Zero, F32.Zero, new F32(cubeScale), F32.Zero),
            new Float4(F32.Zero, F32.Zero, F32.Zero, F32.One)
        );

        var modelMatrix = Float4x4.CreateRotationY(new F32(rotY)) *
                         Float4x4.CreateRotationX(new F32(rotX * 0.5f)) *
                         scaleMatrix;

        var viewMatrix = Float4x4.CreateLookAt(
            new Float3(F32.Zero, F32.Zero, new F32(5.0f)),
            new Float3(F32.Zero, F32.Zero, F32.Zero),
            new Float3(F32.Zero, F32.One, F32.Zero)
        );

        var projectionMatrix = Float4x4.CreatePerspective(
            new F32(MathF.PI / 4.0f),
            new F32(1.0f),  // square aspect ratio (300x300)
            new F32(0.1f),
            new F32(100.0f)
        );

        return projectionMatrix * viewMatrix * modelMatrix;
    }

    private static GpuCubeVertex[] CreateCubeVertices()
    {
        var positions = new Float3[]
        {
            new Float3(new F32(-1), new F32(-1), new F32(-1)),
            new Float3(new F32( 1), new F32(-1), new F32(-1)),
            new Float3(new F32( 1), new F32( 1), new F32(-1)),
            new Float3(new F32(-1), new F32( 1), new F32(-1)),
            new Float3(new F32(-1), new F32(-1), new F32( 1)),
            new Float3(new F32( 1), new F32(-1), new F32( 1)),
            new Float3(new F32( 1), new F32( 1), new F32( 1)),
            new Float3(new F32(-1), new F32( 1), new F32( 1)),
        };

        var red     = new Color4(new F32(1), F32.Zero, F32.Zero, F32.One);
        var green   = new Color4(F32.Zero, new F32(1), F32.Zero, F32.One);
        var blue    = new Color4(F32.Zero, F32.Zero, new F32(1), F32.One);
        var yellow  = new Color4(new F32(1), new F32(1), F32.Zero, F32.One);
        var magenta = new Color4(new F32(1), F32.Zero, new F32(1), F32.One);
        var cyan    = new Color4(F32.Zero, new F32(1), new F32(1), F32.One);

        var verts = new List<GpuCubeVertex>();
        AddQuad(verts, positions[4], positions[5], positions[6], positions[7], red);
        AddQuad(verts, positions[1], positions[0], positions[3], positions[2], green);
        AddQuad(verts, positions[3], positions[7], positions[6], positions[2], blue);
        AddQuad(verts, positions[4], positions[0], positions[1], positions[5], yellow);
        AddQuad(verts, positions[5], positions[1], positions[2], positions[6], magenta);
        AddQuad(verts, positions[0], positions[4], positions[7], positions[3], cyan);
        return verts.ToArray();
    }

    private static void AddQuad(List<GpuCubeVertex> verts, Float3 p0, Float3 p1, Float3 p2, Float3 p3, Color4 color)
    {
        verts.Add(new GpuCubeVertex { Position = p0, Color = color });
        verts.Add(new GpuCubeVertex { Position = p1, Color = color });
        verts.Add(new GpuCubeVertex { Position = p2, Color = color });
        verts.Add(new GpuCubeVertex { Position = p0, Color = color });
        verts.Add(new GpuCubeVertex { Position = p2, Color = color });
        verts.Add(new GpuCubeVertex { Position = p3, Color = color });
    }

    /// <summary>
    /// Creates the cube shader IR module used to generate HLSL/MSL source code.
    /// </summary>
    private static IrShaderModule CreateCubeShaderModule()
    {
        var module = new IrShaderModule { Name = "CubeShader" };

        var f32 = new IrScalarType(ScalarKind.F32);
        var float3 = new IrVectorType(f32, 3);
        var float4 = new IrVectorType(f32, 4);
        var float4x4 = new IrMatrixType(f32, 4, 4);

        // CubeVertex input struct: Position (float3) + Color (float4)
        var cubeVertex = new IrStructType("CubeVertex");
        cubeVertex.Fields.Add(new IrStructField { Name = "Position", Type = float3, Decorations = { new IrLocationDecoration(0) } });
        cubeVertex.Fields.Add(new IrStructField { Name = "Color",    Type = float4, Decorations = { new IrLocationDecoration(1) } });
        module.Structs.Add(new IrStructDecl(cubeVertex));

        // CubeVaryings: Position (SV_POSITION) + Color (location 0)
        var cubeVaryings = new IrStructType("CubeVaryings");
        cubeVaryings.Fields.Add(new IrStructField { Name = "Position", Type = float4, Decorations = { new IrBuiltInDecoration(BuiltInSemantic.Position) } });
        cubeVaryings.Fields.Add(new IrStructField { Name = "Color",    Type = float4, Decorations = { new IrLocationDecoration(0) } });
        module.Structs.Add(new IrStructDecl(cubeVaryings));

        // CubeBindings: MVP matrix (constant buffer binding 0)
        var cubeBindings = new IrStructType("CubeBindings");
        cubeBindings.Fields.Add(new IrStructField { Name = "MVP", Type = float4x4, Decorations = { new IrBindingDecoration(0, 0) } });
        module.Structs.Add(new IrStructDecl(cubeBindings));

        // FragmentOutput: Color (location 0)
        var fragOutput = new IrStructType("FragmentOutput");
        fragOutput.Fields.Add(new IrStructField { Name = "Color", Type = float4, Decorations = { new IrLocationDecoration(0) } });
        module.Structs.Add(new IrStructDecl(fragOutput));

        // Vertex shader body:
        //   CubeVaryings output;
        //   output.Position = MVP * float4(input.Position, 1.0);
        //   output.Color = input.Color;
        //   return output;
        var vsBody = new IrBlock();
        var varyingsType = cubeVaryings;
        vsBody.Statements.Add(new IrVarDecl("output", varyingsType));

        var inputParam = new IrParameter("input", cubeVertex);
        var bindingsParam = new IrParameter("bindings", cubeBindings);

        var inputPos = new IrFieldAccess(inputParam, "Position", float3);
        var one = new IrConstant(f32, 1.0f);
        var pos4 = new IrConstructor(float4, new List<IrExpression> { inputPos, one });
        var mvpField = new IrFieldAccess(bindingsParam, "MVP", float4x4);
        var clipPos = new IrBinaryOp(BinaryOperator.Multiply, mvpField, pos4, float4);

        var outputParam = new IrParameter("output", varyingsType);
        vsBody.Statements.Add(new IrAssignment(new IrFieldAccess(outputParam, "Position", float4), clipPos));
        vsBody.Statements.Add(new IrAssignment(
            new IrFieldAccess(outputParam, "Color", float4),
            new IrFieldAccess(inputParam, "Color", float4)));
        vsBody.Statements.Add(new IrReturn(outputParam));

        module.VertexStage = new IrVertexStage
        {
            Name = "CubeVertexShader",
            InputType = cubeVertex,
            OutputType = cubeVaryings,
            BindingsType = cubeBindings,
            Body = vsBody
        };

        // Fragment shader body:
        //   FragmentOutput output;
        //   output.Color = input.Color;
        //   return output;
        var fsBody = new IrBlock();
        fsBody.Statements.Add(new IrVarDecl("output", fragOutput));
        var fsInput = new IrParameter("input", cubeVaryings);
        var fsOutput = new IrParameter("output", fragOutput);
        fsBody.Statements.Add(new IrAssignment(
            new IrFieldAccess(fsOutput, "Color", float4),
            new IrFieldAccess(fsInput, "Color", float4)));
        fsBody.Statements.Add(new IrReturn(fsOutput));

        module.FragmentStage = new IrFragmentStage
        {
            Name = "CubeFragmentShader",
            InputType = cubeVaryings,
            OutputType = fragOutput,
            Body = fsBody
        };

        return module;
    }
}

// === Shader structs for the cube GPU rendering ===

/// <summary>Vertex input for the GPU cube shader.</summary>
public struct GpuCubeVertex
{
    public Float3 Position;
    public Color4 Color;
}

/// <summary>Varyings for the GPU cube shader.</summary>
public struct GpuCubeVaryings
{
    public Float4 Position;
    public Color4 Color;
}

/// <summary>Constant bindings for the GPU cube shader.</summary>
public struct GpuCubeBindings
{
    public Float4x4 MVP;
}

/// <summary>Vertex shader for the GPU cube (software fallback).</summary>
public readonly struct GpuCubeVertexShader
    : Xui.GPU.Shaders.IVertexShader<GpuCubeVertex, GpuCubeVaryings, GpuCubeBindings>
{
    public GpuCubeVaryings Execute(GpuCubeVertex input, in GpuCubeBindings bindings)
    {
        var pos4 = new Float4(input.Position, F32.One);
        return new GpuCubeVaryings
        {
            Position = bindings.MVP * pos4,
            Color = input.Color
        };
    }
}

/// <summary>Fragment shader for the GPU cube (software fallback).</summary>
public readonly struct GpuCubeFragmentShader
    : Xui.GPU.Shaders.IFragmentShader<GpuCubeVaryings, Xui.GPU.Shaders.FragmentOutput, GpuCubeBindings>
{
    public Xui.GPU.Shaders.FragmentOutput Execute(GpuCubeVaryings input, in GpuCubeBindings bindings)
    {
        return new Xui.GPU.Shaders.FragmentOutput { Color = input.Color };
    }
}
