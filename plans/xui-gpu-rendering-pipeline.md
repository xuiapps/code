# Xui.GPU Native Rendering Pipeline Implementation Plan

## Overview

This plan breaks down the implementation of the Xui.GPU native rendering pipeline into manageable, independently completable chunks. The system will enable shader-like programs authored in C#, executable through a CPU software renderer, and translatable at build time into backend-native shader languages.

## Architecture Summary

The rendering pipeline consists of five major layers:
1. **Authoring layer** - Public C# surface for shader types and stage interfaces
2. **Semantic analysis layer** - Roslyn analyzer for validation
3. **Intermediate representation layer** - Backend-neutral IR
4. **Runtime execution layer** - Hardware GPU and software CPU paths
5. **Backend emission layer** - Translation to HLSL, MSL, etc.

## Project Structure

### Core Projects
- `Xui.GPU` - Main public API and core abstractions
- `Xui.GPU.Shaders` - Shader DSL types and interfaces
- `Xui.GPU.Shaders.Types` - Shader type system (F32, Float2, Vec2, etc.)
- `Xui.GPU.Shaders.Attributes` - Attributes for metadata
- `Xui.GPU.Resources` - Resource types (textures, samplers, uniforms)
- `Xui.GPU.Pipelines` - Pipeline descriptors and state

### Build-time Tooling
- `Xui.GPU.Analyzers` - Roslyn analyzers for shader validation
- `Xui.GPU.Generator` - Source generator for metadata and backend code
- `Xui.GPU.IR` - Intermediate representation
- `Xui.GPU.Backends` - Backend abstraction
- `Xui.GPU.Backends.Hlsl` - HLSL code generation
- `Xui.GPU.Backends.Metal` - Metal Shading Language generation
- `Xui.GPU.Backends.Vulkan` - Vulkan-targeted generation

### Runtime Execution
- `Xui.GPU.Software` - Software rendering runtime
- `Xui.GPU.Software.Raster` - Rasterization engine
- `Xui.GPU.Software.Execution` - Fragment quad execution
- `Xui.GPU.Hardware` - Hardware backend abstraction
- `Xui.GPU.Hardware.D3D12` - Direct3D 12 backend
- `Xui.GPU.Hardware.Metal` - Metal backend
- `Xui.GPU.Hardware.Vulkan` - Vulkan backend

### Tests
- `Xui.GPU.Tests` - Unit tests for core functionality
- `Xui.GPU.Software.Tests` - Software renderer tests
- `Xui.GPU.Analyzers.Tests` - Analyzer tests

### Demo/Sample Apps
- `Xui.GPU.Samples.Triangle` - MVP: Draw a triangle to RGBA bitmap

## Implementation Checklist

### Phase 0: Planning and Infrastructure (This Session)
- [x] Create this plan document
- [x] Create directory structure for all projects
- [x] Add projects to Xui.sln
- [x] Set up basic project files with dependencies
- [x] Create placeholder namespaces and folder structure
- [x] Initial commit of skeleton structure

### Phase 1: Core Type System (Milestone 1a)
- [x] Implement shader scalar types (F32, I32, U32, Bool)
- [x] Implement vector types (Vec2<T>, Vec3<T>, Vec4<T>)
- [x] Create convenience aliases (Float2, Float3, Float4)
- [x] Implement Color4 type
- [x] Add matrix types (Mat2<T>, Mat3<T>, Mat4<T>, Float4x4)
- [x] Add operator overloads for shader math
- [x] Add unit tests for type system
- [x] Document public API with XML comments

### Phase 2: Shader Interfaces and Attributes (Milestone 1b)
- [x] Define IShaderStage base interface
- [x] Define IVertexShader<TInput, TOutput, TBindings> interface
- [x] Define IFragmentShader<TInput, TOutput, TBindings> interface
- [x] Implement shader attributes ([ShaderProgram], [VertexShader], [FragmentShader])
- [x] Implement location attributes ([Location(n)], [BuiltIn(semantic)])
- [x] Implement binding attributes ([Group(n)], [Binding(n)])
- [x] Add interpolation attributes (Flat, Linear, Perspective)
- [x] Document attribute usage patterns

### Phase 3: Resource Types (Milestone 1c)
- [x] Implement Uniform<T> wrapper type
- [x] Implement Texture2D<TPixel> type
- [x] Implement Sampler type
- [x] Define resource access methods (Sample, Load)
- [x] Add resource capability flags
- [x] Add unit tests for resource types

### Phase 4: Shader Intrinsics (Milestone 1d)
- [x] Create Shader static class for intrinsics
- [x] Implement basic math intrinsics (Clamp, Saturate, Lerp, Min, Max, Abs)
- [x] Implement trigonometric intrinsics (Sin, Cos, Tan)
- [x] Implement vector intrinsics (Dot, Length, Normalize, Cross)
- [x] Implement derivative intrinsics (Ddx, Ddy) - fragment only
- [x] Implement texture sampling intrinsics
- [x] Document stage restrictions for each intrinsic

### Phase 5: Intermediate Representation (Milestone 2a)
- [x] Design IR node base types
- [x] Implement type nodes (ScalarType, VectorType, MatrixType, StructType)
- [x] Implement expression nodes (Constant, Parameter, Field, BinaryOp, UnaryOp)
- [x] Implement statement nodes (VarDecl, Assignment, Return, If, Block)
- [x] Implement decoration nodes (Location, BuiltIn, Binding, Interpolation)
- [x] Implement module and stage declaration nodes
- [x] Add IR serialization for debugging
- [x] Add unit tests for IR construction

### Phase 6: Roslyn Analyzer (Milestone 2b)
- [ ] Set up analyzer project infrastructure
- [ ] Implement stage structure validation
- [ ] Implement allowed type checking
- [ ] Implement allowed syntax checking (no try/catch, no async, etc.)
- [ ] Implement intrinsic call validation
- [ ] Implement stage restriction validation (derivatives in fragment only)
- [ ] Implement resource access validation
- [ ] Create diagnostic codes (XUIGPU001-XUIGPU030)
- [ ] Add analyzer tests with valid and invalid shader examples
- [ ] Document all diagnostic codes

### Phase 7: Source Generator Basics (Milestone 2c)
- [ ] Set up generator project infrastructure
- [ ] Implement shader discovery (find [ShaderProgram] types)
- [ ] Implement metadata generation for discovered shaders
- [ ] Generate partial type augmentation with metadata properties
- [ ] Add generator tests
- [ ] Document generated code patterns

### Phase 8: Software Renderer - Core Infrastructure (Milestone 3a)
- [x] Design internal execution types (CpuScalarF32, CpuQuadF32)
- [x] Implement Framebuffer and ColorTarget
- [x] Implement VertexSource<T> for vertex data
- [x] Implement basic primitive assembly
- [x] Implement viewport transform
- [x] Add unit tests for infrastructure

### Phase 9: Software Renderer - Rasterization (Milestone 3b)
- [x] Implement triangle rasterization (scan conversion)
- [x] Implement barycentric coordinate calculation
- [x] Implement edge function tests
- [x] Implement per-pixel coverage determination
- [x] Add depth buffer support
- [x] Add unit tests with known triangle outputs

### Phase 10: Software Renderer - Fragment Execution (Milestone 3c)
- [x] Implement fragment quad grouping (2x2 pixels)
- [x] Implement varying interpolation for quads
- [x] Implement derivative calculation (ddx, ddy)
- [x] Implement fragment shader execution
- [x] Implement blend operations
- [x] Add unit tests for fragment execution

### Phase 11: MVP Triangle Demo (Milestone 3d)
- [x] Create Xui.GPU.Samples.Triangle project
- [x] Define simple vertex structure with position and color
- [x] Author vertex shader in C#
- [x] Author fragment shader in C#
- [x] Set up pipeline descriptor
- [x] Render triangle to RGBA bitmap
- [x] Save output as PNG
- [x] Add depth/stencil buffer
- [x] Verify output visually
- [x] Document the demo

### Phase 12: HLSL Backend (Milestone 4a)
- [x] Implement IR to HLSL translator
- [x] Map shader types to HLSL types
- [x] Generate HLSL vertex shader entry point
- [x] Generate HLSL fragment (pixel) shader entry point
- [x] Handle resource bindings in HLSL
- [x] Handle built-in semantics (SV_Position, etc.)
- [x] Generate metadata for compiled shaders
- [x] Add unit tests comparing HLSL output

### Phase 13: Pipeline Composition (Milestone 4b)
- [x] Implement RenderPipeline<TVertex, TFragment, ...> type
- [x] Implement PipelineDescriptor with state (topology, blend, etc.)
- [x] Implement pipeline validation
- [x] Verify vertex output matches fragment input
- [x] Add pipeline creation APIs
- [x] Add unit tests for pipeline composition

### Phase 14: Extended Math and Intrinsics (Milestone 5a)
- [x] Add swizzle support for vectors
- [x] Add more texture sampling modes
- [x] Add more intrinsic functions (Pow, Exp, Log, Sqrt)
- [x] Add interpolation control
- [x] Add more blend modes
- [x] Add unit tests

### Phase 15a: DirectX Hardware Backend (Milestone 6a)
- [x] Implement HLSL code generator (Phase 12 - already complete)
- [x] Create `Xui.GPU.Hardware` abstract interface (`IGpuDevice`, `IGpuRenderTarget`, `IGpuCommandList`, etc.)
- [x] Create `Xui.GPU.Hardware.D3D11` project (Windows-only, net10.0-windows)
- [x] Implement `D3D11GpuDevice` - creates hardware D3D11 device
- [x] Implement `D3D11RenderTarget` - offscreen render target with CPU readback
- [x] Implement `D3D11CommandList` - records draw calls (vertex buffers, constant buffers, draw)
- [x] Implement `D3D11VertexShader` / `D3D11FragmentShader` - compiled from HLSL bytecode
- [x] Implement `D3DCompiler` - compiles HLSL to bytecode via `d3dcompiler_47.dll`
- [x] Implement `D3D11Native` - P/Invoke wrappers for D3D11 COM API

### Phase 15b: Metal Hardware Backend (Milestone 6b)
- [x] Create `Xui.GPU.Backends.Metal` project - MSL code generator
- [x] Implement `MslCodeGenerator` - translates IR to Metal Shading Language
- [x] Map shader types to MSL types (float, float2, float4, etc.)
- [x] Map shader intrinsics to MSL functions (mix vs lerp, dfdx/dfdy vs ddx/ddy, fract vs frac)
- [x] Generate MSL vertex shader with `[[vertex]]` attribute and `[[stage_in]]`
- [x] Generate MSL fragment shader with `[[fragment]]` attribute
- [x] Handle MSL field attributes (`[[position]]`, `[[user(locnN)]]`)
- [x] Add 11 unit tests comparing MSL output
- [x] Create `Xui.GPU.Hardware.Metal` project (macOS/iOS, net10.0-macos/ios)
- [x] Implement `MetalGpuDevice` - creates Metal device via `MTLCreateSystemDefaultDevice`
- [x] Implement `MetalRenderTarget` - offscreen MTLTexture with `getBytes` readback
- [x] Implement `MetalCommandList` - uses MTLCommandBuffer and MTLRenderCommandEncoder
- [x] Implement `MetalVertexShader` / `MetalFragmentShader` - compiled from MSL via `newLibraryWithSource:`
- [x] Implement `MetalNative` - Objective-C runtime P/Invoke wrappers for Metal API
- [x] Add MSL generation sample (`Xui.GPU.Samples.MslGen`) showing side-by-side HLSL vs MSL output

### Phase 15c: Vulkan Backend (Future)
- [ ] Implement IR to SPIR-V / GLSL translator
- [ ] Create `Xui.GPU.Hardware.Vulkan` project
- [ ] Implement Vulkan device, render target, command list

### Phase 16: TestApp Integration - Hardware GPU Demo
- [x] Add `IGpuDevice`, `IGpuRenderTarget`, `IGpuCommandList` to `Xui.GPU.Hardware`
- [x] Add hardware backend project references to TestApp conditionally (Windows/macOS/iOS)
- [x] Create `GpuDeviceFactory` - platform-aware factory (D3D11 on Windows, Metal on macOS/iOS)
- [x] Create `GpuHardwareCubeTest` - 3D rotating cube using hardware GPU
  - [x] Uses hardware GPU when available (D3D11/Metal)
  - [x] Falls back to software rendering on other platforms
  - [x] Displays current GPU backend name as overlay text
  - [x] Click-and-drag rotation interaction
  - [x] Uses IR → HLSL/MSL code generation for shader source
- [x] Add "GPU Hardware Cube" entry to TestApp 3D examples list

### Phase 17: Advanced Features (Future)
- [ ] Add compute shader support
- [ ] Add storage buffer support
- [ ] Add storage texture support
- [ ] Add instancing support
- [ ] Implement Vulkan backend (Phase 15c)
- [ ] Add multisampling support
- [ ] Add advanced blend modes
- [ ] Optimize IR passes
- [ ] Add shader hot reload support

## Key Design Decisions

### 1. Explicit Shader DSL Types
Use `F32`, `Float2`, etc., rather than plain C# `float` to maintain control over semantic meaning and execution.

### 2. Fragment Quads for Derivatives
Fragment values are executed as 2x2 quads internally to support derivative operations (ddx, ddy).

### 3. Strict Subset Enforcement
Roslyn analyzer enforces a limited, translatable C# subset. No arbitrary C# is allowed in shader code.

### 4. IR as Semantic Bridge
Roslyn → IR → Backends provides better semantic control than direct Roslyn → HLSL translation.

### 5. Type-Driven Pipeline Linkage
Use generics and attributes instead of string-based shader linkage.

## Dependencies and Prerequisites

- .NET 10.0 SDK
- Roslyn API for analyzers and source generators
- Existing Xui.Core types (Point, Size, Color, etc.)
- Existing Xui.Runtime.Software for bitmap rendering infrastructure

## Testing Strategy

1. **Unit Tests**: Each component has unit tests validating its contract
2. **Component Tests**: Integration tests for shader → IR → backend pipelines
3. **Visual Tests**: Render known shapes and compare against reference images
4. **Analyzer Tests**: Valid and invalid shader code samples with expected diagnostics
5. **Performance Tests**: Benchmark software renderer and compare to GPU backends

## Success Criteria for MVP

The MVP (Triangle Demo) is considered successful when:

1. ✅ A triangle can be defined with vertex positions and colors in C#
2. ✅ Vertex shader can transform positions
3. ✅ Fragment shader can output interpolated colors
4. ✅ Software renderer produces correct RGBA bitmap output
5. ✅ Depth buffer works correctly
6. ✅ Analyzer rejects invalid shader code with clear diagnostics
7. ✅ Generated HLSL compiles without errors
8. ✅ All unit tests pass

## Open Questions

1. Should vector types be fully generic (Vec2<F32>) or use aliases (Float2) as primary API?
2. Should source generator emit only source code or also compile backend artifacts?
3. Should CPU execution run authored C# directly or lower to IR first?
4. When should Vulkan/WGSL support be added vs. HLSL-first approach?
5. Should first version support only affine interpolation or implement perspective-correct immediately?

## Notes for Implementation

- Start with minimal working implementation
- Add complexity incrementally
- Test each component independently
- Document public APIs thoroughly
- Follow existing Xui code style and conventions
- Use existing Xui infrastructure where possible
- Each phase should be completable by a single agent in one session
- Maintain comprehensive test coverage throughout

### Critical Requirements for Backend Implementations

**Source Location Tracking:**
- All IR nodes must have source location information (file, line, column)
- This enables mapping backend errors to original C# source during code generation
- Error messages must reference the original source location for developer clarity
- Example: "Unsupported intrinsic 'Foo' at MyShader.cs(42,15)"

**Strict Backend Translation:**
- Backend code generators must be completely strict - no pass-through or "YOLO" defaults
- Every switch statement must explicitly handle all cases or throw with source location
- Unknown constructs must fail with clear error messages, not silently pass through
- This ensures 1:1 mapping between C# shader functions and native platform functions
- Each intrinsic, operator, and type must have an explicit mapping or error

**Platform Function Mapping:**
- When implementing hardware backends (D3D12, Metal, Vulkan), maintain explicit mappings
- Document all C# → Native function translations (e.g., Shader.Clamp → clamp in HLSL)
- All 40+ shader intrinsics must have verified mappings for each backend
- Update MapIntrinsicToHlsl, MapIntrinsicToMsl, etc. when adding new intrinsics
- Any unmapped function must fail compilation with helpful error message

## References

- RFC document (this plan is derived from it)
- WebGPU specification for API design inspiration
- HLSL reference for backend mapping
- Metal Shading Language specification
- Existing Xui.Runtime.Software for rendering primitives
