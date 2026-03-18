# Xui.GPU - Native Rendering Pipeline

A shader-like programming system for Xui where programs are authored in C#, executable through a CPU software renderer, and translatable at build time into backend-native shader languages (HLSL, Metal, Vulkan).

## Architecture

The system is organized into five major layers:

### 1. Core Projects (Authoring Layer)
- **Xui.GPU** - Main public API and core abstractions
- **Xui.GPU.Shaders** - Shader DSL types (F32, Float2, Float4, etc.) and stage interfaces
- **Xui.GPU.Resources** - Resource types (Uniform, Texture2D, Sampler)
- **Xui.GPU.Pipelines** - Pipeline descriptors and state management
- **Xui.GPU.Tests** - Unit tests for core functionality

### 2. Build-time Tooling (Semantic Analysis & Code Generation)
- **Xui.GPU.Analyzers** - Roslyn analyzers for shader validation (future)
- **Xui.GPU.Generator** - Source generator for metadata and backend code (future)
- **Xui.GPU.IR** - Backend-neutral intermediate representation (future)

### 3. Backend Emission
- **Xui.GPU.Backends** - Backend abstraction (future)
- **Xui.GPU.Backends.Hlsl** - HLSL code generation (future)
- **Xui.GPU.Backends.Metal** - Metal Shading Language generation (future)
- **Xui.GPU.Backends.Vulkan** - Vulkan-targeted generation (future)

### 4. Software Renderer (Reference Implementation)
- **Xui.GPU.Software** - CPU-based software rendering runtime
- **Xui.GPU.Software.Raster** - Triangle rasterization engine
- **Xui.GPU.Software.Execution** - Fragment quad execution for derivatives

### 5. Hardware Backends
- **Xui.GPU.Hardware** - Hardware backend abstraction (future)
- **Xui.GPU.Hardware.D3D12** - Direct3D 12 backend (future)
- **Xui.GPU.Hardware.Metal** - Metal backend (future)
- **Xui.GPU.Hardware.Vulkan** - Vulkan backend (future)

## Samples

- **Xui.GPU.Samples.Triangle** - MVP demo: Draw a triangle to RGBA bitmap with depth buffer

## Current Status

**Phase 0-4, 8-11 Complete**: Full working software renderer with MVP triangle demo! 🎉

- ✅ Directory structure created
- ✅ Project files configured and added to solution
- ✅ Basic shader scalar types (F32, I32, U32, Bool)
- ✅ Vector types (Float2, Float3, Float4, Int2)
- ✅ Matrix type (Float4x4)
- ✅ Color4 type
- ✅ Shader stage interfaces (IVertexShader, IFragmentShader)
- ✅ Basic attributes (ShaderProgram, VertexShader, FragmentShader, Location, BuiltIn)
- ✅ Resource types (Uniform<T>, Texture2D<T>, Sampler)
- ✅ Complete operator support for all types
- ✅ Comprehensive shader intrinsics (math, trigonometry, vectors, textures)
- ✅ **Software renderer infrastructure** (Framebuffer, ColorTarget, Viewport, RenderContext)
- ✅ **Triangle rasterization** (edge functions, barycentric coordinates, scan conversion)
- ✅ **Fragment execution** (varying interpolation, depth testing, alpha blending)
- ✅ **Working triangle demo** (renders beautiful RGB triangle to PNG)
- ✅ All projects build successfully
- ✅ **Unit tests: 93/93 passing** (66 original + 27 new software renderer tests)
- ✅ Visual validation: Triangle renders correctly with smooth color interpolation

## Next Steps

See [plans/xui-gpu-rendering-pipeline.md](/plans/xui-gpu-rendering-pipeline.md) for the complete implementation roadmap.

**Current Milestone: MVP Complete! 🎉**

The software renderer is fully functional with a working triangle demo. Next phases focus on build-time tooling and additional features:

**Future Phases:**
- Phase 2: Extended shader attributes (interpolation qualifiers: Flat, Linear, Perspective)
- Phase 5-7: IR, analyzers, and source generators for shader validation and code generation
- Phase 12: HLSL backend for DirectX 12 support
- Phase 13: Pipeline composition and validation
- Phase 14-16: Advanced features, Metal backend, hardware backends

## Key Design Principles

1. **Explicit Shader DSL Types** - Use F32, Float2, etc., not plain C# float
2. **Fragment Quads for Derivatives** - Fragment values execute as 2x2 quads for ddx/ddy support
3. **Strict Subset Enforcement** - Roslyn analyzer will enforce translatable C# subset
4. **IR as Semantic Bridge** - Roslyn → IR → Backends (not direct HLSL emission)
5. **Type-Driven Pipeline Linkage** - Use generics and attributes, not string names

## Building and Running

```bash
# Build all GPU projects
dotnet build Xui/GPU/Core/Xui.GPU/Xui.GPU.csproj

# Run tests (93 tests)
dotnet test Xui/GPU/Core/Xui.GPU.Tests/Xui.GPU.Tests.csproj

# Run triangle sample - renders RGB triangle to PNG
dotnet run --project Xui/GPU/Samples/Xui.GPU.Samples.Triangle/Xui.GPU.Samples.Triangle.csproj
# Output: triangle_output.png (512x512 rendered triangle with smooth color interpolation)
```

### Triangle Sample Output
The triangle demo produces a beautiful 512x512 PNG image showing:
- Red vertex at top
- Green vertex at bottom-left  
- Blue vertex at bottom-right
- Smooth color interpolation across the triangle surface using barycentric coordinates
- Dark gray background
- Demonstrates the complete Xui.GPU pipeline from C# shaders to pixels

## Documentation

- Implementation plan: [plans/xui-gpu-rendering-pipeline.md](/plans/xui-gpu-rendering-pipeline.md)
- RFC: See problem statement in the plan document

## License

Copyright 2025 Xui Apps. See LICENSE.md in repository root.
