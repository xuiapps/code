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
- [ ] Create directory structure for all projects
- [ ] Add projects to Xui.sln
- [ ] Set up basic project files with dependencies
- [ ] Create placeholder namespaces and folder structure
- [ ] Initial commit of skeleton structure

### Phase 1: Core Type System (Milestone 1a)
- [ ] Implement shader scalar types (F32, I32, U32, Bool)
- [ ] Implement vector types (Vec2<T>, Vec3<T>, Vec4<T>)
- [ ] Create convenience aliases (Float2, Float3, Float4)
- [ ] Implement Color4 type
- [ ] Add matrix types (Mat2<T>, Mat3<T>, Mat4<T>, Float4x4)
- [ ] Add operator overloads for shader math
- [ ] Add unit tests for type system
- [ ] Document public API with XML comments

### Phase 2: Shader Interfaces and Attributes (Milestone 1b)
- [ ] Define IShaderStage base interface
- [ ] Define IVertexShader<TInput, TOutput, TBindings> interface
- [ ] Define IFragmentShader<TInput, TOutput, TBindings> interface
- [ ] Implement shader attributes ([ShaderProgram], [VertexShader], [FragmentShader])
- [ ] Implement location attributes ([Location(n)], [BuiltIn(semantic)])
- [ ] Implement binding attributes ([Group(n)], [Binding(n)])
- [ ] Add interpolation attributes (Flat, Linear, Perspective)
- [ ] Document attribute usage patterns

### Phase 3: Resource Types (Milestone 1c)
- [ ] Implement Uniform<T> wrapper type
- [ ] Implement Texture2D<TPixel> type
- [ ] Implement Sampler type
- [ ] Define resource access methods (Sample, Load)
- [ ] Add resource capability flags
- [ ] Add unit tests for resource types

### Phase 4: Shader Intrinsics (Milestone 1d)
- [ ] Create Shader static class for intrinsics
- [ ] Implement basic math intrinsics (Clamp, Saturate, Lerp, Min, Max, Abs)
- [ ] Implement trigonometric intrinsics (Sin, Cos, Tan)
- [ ] Implement vector intrinsics (Dot, Length, Normalize, Cross)
- [ ] Implement derivative intrinsics (Ddx, Ddy) - fragment only
- [ ] Implement texture sampling intrinsics
- [ ] Document stage restrictions for each intrinsic

### Phase 5: Intermediate Representation (Milestone 2a)
- [ ] Design IR node base types
- [ ] Implement type nodes (ScalarType, VectorType, MatrixType, StructType)
- [ ] Implement expression nodes (Constant, Parameter, Field, BinaryOp, UnaryOp)
- [ ] Implement statement nodes (VarDecl, Assignment, Return, If, Block)
- [ ] Implement decoration nodes (Location, BuiltIn, Binding, Interpolation)
- [ ] Implement module and stage declaration nodes
- [ ] Add IR serialization for debugging
- [ ] Add unit tests for IR construction

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
- [ ] Design internal execution types (CpuScalarF32, CpuQuadF32)
- [ ] Implement Framebuffer and ColorTarget
- [ ] Implement VertexSource<T> for vertex data
- [ ] Implement basic primitive assembly
- [ ] Implement viewport transform
- [ ] Add unit tests for infrastructure

### Phase 9: Software Renderer - Rasterization (Milestone 3b)
- [ ] Implement triangle rasterization (scan conversion)
- [ ] Implement barycentric coordinate calculation
- [ ] Implement edge function tests
- [ ] Implement per-pixel coverage determination
- [ ] Add depth buffer support
- [ ] Add unit tests with known triangle outputs

### Phase 10: Software Renderer - Fragment Execution (Milestone 3c)
- [ ] Implement fragment quad grouping (2x2 pixels)
- [ ] Implement varying interpolation for quads
- [ ] Implement derivative calculation (ddx, ddy)
- [ ] Implement fragment shader execution
- [ ] Implement blend operations
- [ ] Add unit tests for fragment execution

### Phase 11: MVP Triangle Demo (Milestone 3d)
- [ ] Create Xui.GPU.Samples.Triangle project
- [ ] Define simple vertex structure with position and color
- [ ] Author vertex shader in C#
- [ ] Author fragment shader in C#
- [ ] Set up pipeline descriptor
- [ ] Render triangle to RGBA bitmap
- [ ] Save output as PNG
- [ ] Add depth/stencil buffer
- [ ] Verify output visually
- [ ] Document the demo

### Phase 12: HLSL Backend (Milestone 4a)
- [ ] Implement IR to HLSL translator
- [ ] Map shader types to HLSL types
- [ ] Generate HLSL vertex shader entry point
- [ ] Generate HLSL fragment (pixel) shader entry point
- [ ] Handle resource bindings in HLSL
- [ ] Handle built-in semantics (SV_Position, etc.)
- [ ] Generate metadata for compiled shaders
- [ ] Add unit tests comparing HLSL output

### Phase 13: Pipeline Composition (Milestone 4b)
- [ ] Implement RenderPipeline<TVertex, TFragment, ...> type
- [ ] Implement PipelineDescriptor with state (topology, blend, etc.)
- [ ] Implement pipeline validation
- [ ] Verify vertex output matches fragment input
- [ ] Add pipeline creation APIs
- [ ] Add unit tests for pipeline composition

### Phase 14: Extended Math and Intrinsics (Milestone 5a)
- [ ] Add swizzle support for vectors
- [ ] Add more texture sampling modes
- [ ] Add more intrinsic functions (Pow, Exp, Log, Sqrt)
- [ ] Add interpolation control
- [ ] Add more blend modes
- [ ] Add unit tests

### Phase 15: Metal Backend (Milestone 5b)
- [ ] Implement IR to Metal Shading Language translator
- [ ] Map shader types to MSL types
- [ ] Generate MSL vertex shader entry point
- [ ] Generate MSL fragment shader entry point
- [ ] Handle resource bindings in MSL
- [ ] Add unit tests comparing MSL output

### Phase 16: Advanced Features (Future)
- [ ] Add compute shader support
- [ ] Add storage buffer support
- [ ] Add storage texture support
- [ ] Add instancing support
- [ ] Implement Vulkan backend
- [ ] Implement hardware backends (D3D12, Metal, Vulkan)
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
