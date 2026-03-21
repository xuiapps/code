# Integrate GPU into Xui Core and Platform Runtimes

## Goal

Dissolve the standalone `Xui/GPU/` projects into the main Xui framework, placing GPU abstractions alongside the UI framework in Core and GPU implementations inside their respective platform runtimes.

## Current State

```
Xui/GPU/
├── Xui.GPU/              Core types, IR, backends, hardware interfaces
├── Xui.GPU.Generator/    Roslyn source generator (netstandard2.0)
├── Xui.GPU.Software/     CPU renderer
├── Xui.GPU.DirectX/      D3D11 backend (~1k LOC)
├── Xui.GPU.Metal/        Metal backend (~1k LOC)
├── Xui.GPU.Tests/        Tests
└── Samples/              Demos
```

## Target State

```
Xui/Core/Core/GPU/           → Xui.Core.GPU namespace (abstractions + IR + backends)
Xui/Runtime/Software/GPU/    → Xui.Runtime.Software (CPU renderer)
Xui/Runtime/Windows/GPU/     → Xui.Runtime.Windows (D3D11, reuse COM.Unknown)
Xui/Runtime/MacOS/GPU/       → Xui.Runtime.MacOS (Metal, reuse ObjC interop)
Xui/Runtime/IOS/GPU/         → Xui.Runtime.IOS (Metal, reuse ObjC interop)
Xui/GPU/Xui.GPU.Generator/  → stays separate (netstandard2.0 Roslyn constraint)
Xui/Core/Core.Tests/GPU/     → unit tests
Xui/Tests/Component/GPU/     → integration/visual tests
```

## Why

- Metal P/Invoke layer in `MetalNative.cs` duplicates ObjC interop already in `MacOS/ObjC/` (retain, release, msgSend, selector registration, class lookup, NSString conversion)
- D3D11 COM wrappers duplicate patterns already in `Windows/COM/COM.Unknown.cs`
- GPU device registration via `GetService<IGpuDevice>()` is already wired into platform windows — the implementation should live there
- ~1k LOC per platform backend doesn't justify separate projects
- The GPU types (Float3, Float4, Color4, Float4x4) are general-purpose math types useful across the framework

## Migration Plan

### Phase A: Xui.GPU → Xui.Core.GPU

Move the core GPU types into `Xui/Core/Core/GPU/`. Rename namespaces from `Xui.GPU.*` to `Xui.Core.GPU.*`.

#### A.1: Shader types and math
- `Shaders/Types/*.cs` (F32, Float2, Float3, Float4, Color4, Float4x4, etc.)
  - → `Xui/Core/Core/GPU/Types/`
  - Namespace: `Xui.Core.GPU.Types`
  - These are pure math types with no dependencies

#### A.2: Shader attributes and interfaces
- `Shaders/Attributes/*.cs` ([ShaderProgram], [Location], [BuiltIn], etc.)
  - → `Xui/Core/Core/GPU/Shaders/Attributes/`
  - Namespace: `Xui.Core.GPU.Shaders.Attributes`
- `Shaders/IVertexShader.cs`, `IFragmentShader.cs`, `FragmentOutput.cs`, `Shader.cs` (intrinsics)
  - → `Xui/Core/Core/GPU/Shaders/`
  - Namespace: `Xui.Core.GPU.Shaders`

#### A.3: IR (intermediate representation)
- `IR/*.cs` (IrShaderModule, all IR nodes)
  - → `Xui/Core/Core/GPU/IR/`
  - Namespace: `Xui.Core.GPU.IR`

#### A.4: Hardware abstractions
- `Hardware/IGpuDevice.cs` (IGpuDevice, IGpuCommandList, GpuPipelineDesc, etc.)
  - → `Xui/Core/Core/GPU/`
  - Namespace: `Xui.Core.GPU`
  - These are the interfaces that platform runtimes implement

#### A.5: Backends (code generators)
- `Backends/*.cs` (IShaderBackend, HlslCodeGenerator, MslCodeGenerator)
  - → `Xui/Core/Core/GPU/Backends/`
  - Namespace: `Xui.Core.GPU.Backends`

#### A.6: Resources
- `Resources/Uniform.cs`
  - → `Xui/Core/Core/GPU/Resources/`
  - Namespace: `Xui.Core.GPU.Resources`

#### A.7: Update Xui.Core.csproj
- Add `AllowUnsafeBlocks` (needed by some GPU types)
- The Core project has no new external dependencies — GPU types are self-contained

### Phase B: Xui.GPU.Software → Xui.Runtime.Software

Move the software renderer into the existing Software runtime.

#### B.1: Move source files
- `Xui.GPU.Software/*.cs` (RenderContext, Framebuffer, VertexSource, Viewport, ColorTarget, CullMode)
  - → `Xui/Runtime/Software/GPU/`
  - Namespace: `Xui.Runtime.Software.GPU` (or keep as `Xui.GPU.Software` for now, rename later)

#### B.2: Align with existing Software runtime patterns
- The Software runtime already has `SoftwareContext.cs`, `G16Stencil.cs`, `RGBABitmap.cs`
- The GPU `Framebuffer` is similar to `RGBABitmap` — consider whether to unify or keep separate
- GPU framebuffer uses `uint*` for RGBA32, Software uses `RGBA` struct — document the difference

### Phase C: Xui.GPU.DirectX → Xui.Runtime.Windows

Integrate D3D11 backend into the Windows runtime.

#### C.1: Reuse existing COM infrastructure
- `D3D11Native.cs` currently has its own COM vtable helpers
- Windows runtime has `COM/COM.Unknown.cs` with `AddRef`, `Release`, `QueryInterface`
- **Merge**: Refactor D3D11 types to use `COM.Unknown` base class instead of raw void pointers
- This may require adapting the D3D11 command list and device to use the existing COM wrapper pattern

#### C.2: Move source files
- `D3D11GpuDevice.cs` → `Xui/Runtime/Windows/GPU/D3D11GpuDevice.cs`
- `D3D11CommandList.cs` → `Xui/Runtime/Windows/GPU/D3D11CommandList.cs`
- `D3D11Native.cs` → merge with existing Windows native helpers or `Xui/Runtime/Windows/GPU/D3D11Native.cs`
- `D3DCompiler.cs` → `Xui/Runtime/Windows/GPU/D3DCompiler.cs`
- `D3D11Shaders.cs` → `Xui/Runtime/Windows/GPU/`

#### C.3: Update service registration
- `Win32Window.GetService()` already returns `IGpuDevice` — the implementation just moves from a referenced project to the same project
- Remove the `Xui.GPU.DirectX` project reference from `Windows.csproj`

### Phase D: Xui.GPU.Metal → Xui.Runtime.MacOS + Xui.Runtime.IOS

Integrate Metal backend into both Apple runtimes.

#### D.1: Reuse existing ObjC infrastructure (critical)
- `MetalNative.cs` has its own `ObjC_MsgSend` P/Invoke declarations, `Retain()`, `Release()`, `NSStringFromString()`, selector registration
- The MacOS runtime already has all of this in `ObjC/ObjC.cs`, `ObjC/Sel.cs`, `ObjC/Ref.cs`
- **Merge**: Refactor Metal types to use the existing ObjC interop layer:
  - Replace `MetalNative.ObjC_MsgSend(ptr, sel)` with the runtime's `ObjC.objc_msgSend_retIntPtr(ptr, sel)`
  - Replace `MetalNative.Sel_RegisterName("foo")` with `Sel.RegisterName("foo")` or cached `Sel` instances
  - Replace `MetalNative.Retain()/Release()` with the runtime's retain/release pattern
  - Replace `MetalNative.NSStringFromString()` with the runtime's NSString helpers
- This is the most labor-intensive step — `MetalNative.cs` has ~500 LOC of ObjC interop that overlaps

#### D.2: Move Metal implementation files
- `MetalGpuDevice.cs` → `Xui/Runtime/MacOS/GPU/MetalGpuDevice.cs`
- `MetalCommandList.cs` → `Xui/Runtime/MacOS/GPU/MetalCommandList.cs`
- `MetalShaders.cs` (MetalRenderTarget, MetalVertexShader, etc.) → `Xui/Runtime/MacOS/GPU/`
- Metal-specific native helpers (texture creation, depth stencil, vertex descriptors) → `Xui/Runtime/MacOS/GPU/MetalNative.cs` (only the Metal-specific parts, not the ObjC base)

#### D.3: iOS copy
- iOS and macOS Metal code is nearly identical
- Copy the GPU/ directory from MacOS to IOS
- Both share the same Metal API — differences are only in UIKit vs AppKit integration
- Consider a shared source file approach if the code is truly identical

#### D.4: Update service registration
- `MacOSWindow.GetService()` already returns `IGpuDevice` — implementation moves in-project
- Same for iOS window
- Remove `Xui.GPU.Metal` project reference from both runtime csprojsw

### Phase E: Tests

#### E.1: Unit tests → Core.Tests
- IR type tests, code generator tests, shader type tests
  - → `Xui/Core/Core.Tests/GPU/`
  - These test pure logic with no platform dependencies

#### E.2: Component/visual tests → Tests/Component
- Tests that render and compare against reference images
  - → `Xui/Tests/Component/GPU/`
  - These may use the Software runtime for headless rendering

### Phase F: Generator → Xui.CompileTime

Move `Xui/GPU/Xui.GPU.Generator/` → `Xui/CompileTime/Xui.CompileTime.csproj`

- Rename to `Xui.CompileTime` — a single compile-time project for all Roslyn generators
- Future home for markup language generators, custom shading language support, etc.
- Stays `netstandard2.0` (Roslyn requirement)
- Keeps `Microsoft.CodeAnalysis.CSharp` dependency
- Includes Core GPU source files directly via `<Compile Include>` — update paths:

```xml
<!-- Points to new Core locations -->
<Compile Include="../Core/Core/GPU/IR/*.cs" LinkBase="GPU/IR" />
<Compile Include="../Core/Core/GPU/Backends/*.cs" LinkBase="GPU/Backends" />
```

- Wired into consuming projects as an Analyzer:
```xml
<ProjectReference Include="../../CompileTime/Xui.CompileTime.csproj"
                  OutputItemType="Analyzer"
                  ReferenceOutputAssembly="false" />
```

### Phase G: Namespace rename

After all files are in place and building, do a global namespace rename:
- `Xui.GPU.Shaders` → `Xui.Core.GPU.Shaders`
- `Xui.GPU.Shaders.Types` → `Xui.Core.GPU.Types`
- `Xui.GPU.Shaders.Attributes` → `Xui.Core.GPU.Shaders.Attributes`
- `Xui.GPU.IR` → `Xui.Core.GPU.IR`
- `Xui.GPU.Hardware` → `Xui.Core.GPU`
- `Xui.GPU.Backends` → `Xui.Core.GPU.Backends`
- `Xui.GPU.Software` → `Xui.Runtime.Software.GPU`
- `Xui.GPU.Hardware.D3D11` → `Xui.Runtime.Windows.GPU`
- `Xui.GPU.Hardware.Metal` → `Xui.Runtime.MacOS.GPU` / `Xui.Runtime.IOS.GPU`

This is the riskiest step — touches every file. Do it last, one namespace at a time, verifying builds between each.

### Phase H: Cleanup

- Delete `Xui/GPU/Xui.GPU/` (merged into Core)
- Delete `Xui/GPU/Xui.GPU.Software/` (merged into Runtime/Software)
- Delete `Xui/GPU/Xui.GPU.DirectX/` (merged into Runtime/Windows)
- Delete `Xui/GPU/Xui.GPU.Metal/` (merged into Runtime/MacOS + IOS)
- Delete `Xui/GPU/Xui.GPU.Tests/` (merged into Core.Tests + Tests/Component)
- Update `Xui.sln` to remove old project entries
- Update `Xui.targets` in TestApp to remove standalone GPU project references
- Samples may need updating or can be deprecated if TestApp covers the same ground

## Execution Order

1. **Phase A** first — move Core types, everything else still references them
2. **Phase F** next — update Generator paths since it includes source files
3. **Phase B** — Software renderer (simplest platform, no native interop merge)
4. **Phase C** — Windows/D3D11 (COM merge is moderate complexity)
5. **Phase D** — Metal/macOS+iOS (ObjC merge is highest complexity)
6. **Phase E** — Tests (after all runtime code is in place)
7. **Phase G** — Namespace rename (after everything builds)
8. **Phase H** — Cleanup

## Risks

- **ObjC interop merge (Phase D)**: The existing MacOS `ObjC.cs` has different msgSend overload signatures than `MetalNative.cs`. Need to verify all required signatures exist or add them.
- **Namespace rename (Phase G)**: Touching every file. Do incrementally with builds between each rename.
- **Generator source includes**: The Generator compiles IR and backend files directly — paths must track the Core location changes.
- **Solution file**: Multiple project moves will require careful `.sln` updates.
- **Samples**: May break if they reference old project locations. Consider deprecating in favor of TestApp examples.

## Not in scope

- Changing the GPU rendering architecture itself
- Adding new features (Vulkan, compute shaders, etc.)
- Changing the shader DSL or IR
- Modifying the source generator logic
