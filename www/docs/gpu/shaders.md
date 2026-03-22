---
title: Writing Shaders
description: Define GPU shaders as C# structs and compile them to HLSL/MSL at build time.
---

# Writing Shaders

Shaders in Xui are written as C# structs that implement `IVertexShader<>` or `IFragmentShader<>`. A Roslyn source generator reads the `Execute` method at build time and produces native HLSL and MSL shader code.

## Defining a Shader Program

Mark a struct with `[ShaderProgram]` and define nested vertex and fragment stages:

```csharp
using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Attributes;
using Xui.GPU.Shaders.Types;

[ShaderProgram]
public partial struct MyShader
{
    // Data passed from vertex to fragment stage
    public struct Varyings
    {
        [BuiltIn(BuiltIn.Position)]
        public Float4 Position;

        [Location(0)]
        public Color4 Color;
    }

    // Uniform data shared across all vertices/fragments
    public struct Bindings
    {
        public Float4x4 MVP;
    }

    // Vertex shader — runs once per vertex
    [VertexShader]
    public struct VertexStage : IVertexShader<MyVertex, Varyings, Bindings>
    {
        public Varyings Execute(MyVertex input, in Bindings bindings)
        {
            var pos4 = new Float4(input.Position, F32.One);
            return new Varyings
            {
                Position = bindings.MVP * pos4,
                Color = input.Color,
            };
        }
    }

    // Fragment shader — runs once per pixel
    [FragmentShader]
    public struct FragmentStage : IFragmentShader<Varyings, FragmentOutput, Bindings>
    {
        public FragmentOutput Execute(Varyings input, in Bindings bindings)
        {
            return new FragmentOutput { Color = input.Color };
        }
    }
}
```

## Vertex Input Struct

Define the vertex data layout as a plain struct:

```csharp
public struct MyVertex
{
    public Float3 Position;
    public Color4 Color;
}
```

The source generator auto-assigns `[[attribute(N)]]` (Metal) and `TEXCOORD` semantics (HLSL) based on field order.

## Attributes

| Attribute | Applies To | Purpose |
|-----------|-----------|---------|
| `[ShaderProgram]` | Struct | Marks for source generation |
| `[VertexShader]` | Nested struct | Identifies vertex stage |
| `[FragmentShader]` | Nested struct | Identifies fragment stage |
| `[BuiltIn(BuiltIn.Position)]` | Field | Maps to `SV_POSITION` / `[[position]]` |
| `[Location(N)]` | Field | Sets the interpolation location index |

## What the Source Generator Produces

The `partial struct` gets a generated companion with:

- `GenerateShaderSource(IShaderBackend backend)` — returns the pre-compiled HLSL or MSL string
- `GetVertexEntry(string backendName)` — returns `"VSMain"` or `"vertex_main"`
- `GetFragmentEntry(string backendName)` — returns `"PSMain"` or `"fragment_main"`

These are compile-time constants — no runtime IR construction or code generation.

## Supported C# Constructs in Shaders

The source generator walks the `Execute` method body. Supported constructs:

- Variable declarations (`var x = ...`)
- Field access (`input.Position`, `bindings.MVP`)
- Constructors (`new Float4(...)`)
- Binary operators (`MVP * pos4`, `a + b`)
- Struct initializers (`new Varyings { Position = ..., Color = ... }`)
- Return statements
- Static constants (`F32.Zero`, `F32.One`)
- Shader intrinsics (`Shader.Clamp`, `Shader.Normalize`, etc.)

Unsupported: loops, conditionals (if/else — coming soon), method calls to non-intrinsics, try/catch, async.

## Software Execution

The same shader structs run on the CPU via the software renderer. The `Execute` method is called directly — no code generation needed:

```csharp
var vs = new MyShader.VertexStage();
var fs = new MyShader.FragmentStage();
renderContext.Draw(vertexSource, vs, fs, bindings);
```

This enables headless GPU pipeline testing on CI machines without GPU hardware.
