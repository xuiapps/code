---
title: Rendering a Mesh
description: Set up a GPU rendering pipeline to draw 3D geometry in a Xui view.
---

# Rendering a Mesh

This guide walks through rendering a 3D mesh using the Xui GPU pipeline — from defining vertices to displaying pixels on screen.

## 1. Define the Vertex Format

```csharp
public struct MeshVertex
{
    public Float3 Position;
    public Color4 Color;
}
```

## 2. Define the Shader Program

```csharp
[ShaderProgram]
public partial struct MeshShader
{
    public struct Varyings
    {
        [BuiltIn(BuiltIn.Position)]
        public Float4 Position;
        [Location(0)]
        public Color4 Color;
    }

    public struct Bindings
    {
        public Float4x4 MVP;
    }

    [VertexShader]
    public struct VS : IVertexShader<MeshVertex, Varyings, Bindings>
    {
        public Varyings Execute(MeshVertex input, in Bindings bindings)
        {
            return new Varyings
            {
                Position = bindings.MVP * new Float4(input.Position, F32.One),
                Color = input.Color,
            };
        }
    }

    [FragmentShader]
    public struct FS : IFragmentShader<Varyings, FragmentOutput, Bindings>
    {
        public FragmentOutput Execute(Varyings input, in Bindings bindings)
        {
            return new FragmentOutput { Color = input.Color };
        }
    }
}
```

## 3. Build the Mesh Data

```csharp
var vertices = new MeshVertex[]
{
    new() { Position = new Float3(-1, -1, 0), Color = Colors.Red },
    new() { Position = new Float3( 1, -1, 0), Color = Colors.Green },
    new() { Position = new Float3( 0,  1, 0), Color = Colors.Blue },
};
```

## 4. Get the GPU Device

In a Xui `View`, the GPU device and shader backend are available via service resolution:

```csharp
var gpuDevice = this.GetRequiredService<IGpuDevice>();
var shaderBackend = this.GetRequiredService<IShaderBackend>();
```

The platform provides the appropriate implementation:
- **macOS/iOS**: Metal backend
- **Windows**: Direct3D 11 backend

## 5. Compile Shaders and Create Resources

```csharp
// Get pre-compiled shader source (generated at build time)
var source = MeshShader.GenerateShaderSource(shaderBackend);

// Compile for the GPU
var vertexShader = gpuDevice.CompileVertexShader(
    source, MeshShader.GetVertexEntry(shaderBackend.Name));
var fragmentShader = gpuDevice.CompileFragmentShader(
    source, MeshShader.GetFragmentEntry(shaderBackend.Name));

// Create a render target
var renderTarget = gpuDevice.CreateRenderTarget(width, height);
```

## 6. Record and Execute Draw Commands

```csharp
using var cmd = gpuDevice.CreateCommandList();

// Begin render pass with clear color
cmd.BeginRenderPass(renderTarget, new Color4(0.1f, 0.1f, 0.1f, 1.0f));

// Set pipeline state
cmd.SetPipeline(new GpuPipelineDesc
{
    VertexShader = vertexShader,
    FragmentShader = fragmentShader,
    DepthTestEnabled = true,
    DepthWriteEnabled = true,
    // CullMode defaults to GpuCullMode.Back
});

// Upload vertex data and uniforms
var mvp = ComputeMvpMatrix();
fixed (MeshVertex* pVerts = vertices)
{
    cmd.SetVertexBuffer(pVerts, new GpuVertexBufferDesc
    {
        Stride = Marshal.SizeOf<MeshVertex>(),
        VertexCount = vertices.Length
    });
    cmd.SetConstantBuffer(&mvp, sizeof(Float4x4));
    cmd.Draw(vertices.Length);
}

cmd.EndRenderPass();
cmd.Commit();
```

## 7. Read Back Pixels

The render target stores the result as a GPU texture. Read it back to CPU memory for display:

```csharp
var pixels = new byte[width * height * 4];
renderTarget.ReadbackPixelsBgra(pixels);

// Upload to a platform IImage for 2D canvas display
platformImage.LoadPixels(width, height, pixels);
context.DrawImage(platformImage, destRect);
```

## MVP Matrix Helpers

`Float4x4` provides common matrix creation methods:

```csharp
var model = Float4x4.CreateRotationY(angle) * Float4x4.CreateScale(0.5f);
var view = Float4x4.CreateLookAt(eye, target, up);
var projection = Float4x4.CreatePerspective(fovY, aspectRatio, near, far);
var mvp = projection * view * model;
```

## Pipeline State

`GpuPipelineDesc` controls rendering behavior:

| Property | Default | Description |
|----------|---------|-------------|
| `DepthTestEnabled` | `true` | Compare fragment depth against depth buffer |
| `DepthWriteEnabled` | `true` | Write fragment depth to depth buffer |
| `CullMode` | `GpuCullMode.Back` | Cull back-facing triangles |

`GpuCullMode` values: `None`, `Back`, `Front`.

## Software Renderer

The same mesh and shaders work on the CPU for testing:

```csharp
using var fb = new Framebuffer(width, height, withDepthBuffer: true);
var ctx = new RenderContext(fb);
ctx.DepthTestEnabled = true;

fixed (MeshVertex* pVerts = vertices)
{
    var source = new VertexSource<MeshVertex>(pVerts, vertices.Length);
    ctx.Draw(source, new MeshShader.VS(), new MeshShader.FS(), bindings);
}
```

The software renderer executes the C# `Execute` methods directly on the CPU, with proper rasterization, depth testing, back-face culling, and fragment quad grouping for derivative support (`ddx`/`ddy`).
