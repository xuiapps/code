# GPU Rendering Pipeline — Next Steps

## Data-Driven Vertex Descriptors and Custom Mesh Formats

The vertex descriptor / input layout is currently hardcoded per-backend for the cube demo vertex format. This needs to be data-driven:

- [ ] Derive vertex attribute metadata (format, offset, stride) from IR struct fields at pipeline creation time
- [ ] Flow vertex layout info through `GpuPipelineDesc` so backends build descriptors automatically
- [ ] Support custom mesh vertex formats via source generator:
  - User defines a C# struct (e.g. `struct MyMesh { Float3 Position; Float2 UV; }`)
  - Mesh upload API accepts `ReadOnlySpan<T>` where `T` is the user's vertex struct
  - Source generator reads the fields of `T` at build time
  - Generates the IR struct metadata, vertex descriptor attributes, and field access code
  - Each backend creates the correct vertex descriptor/input layout from the generated metadata
- [ ] Validate vertex struct fields at build time (only allowed shader types)

## Vulkan Backend

- [ ] Implement IR to SPIR-V or GLSL translator in `Xui.Core.GPU.Backends`
- [ ] Add Vulkan runtime in `Xui/Runtime/Linux/Vulkan/` (or Android)
- [ ] Implement VkDevice, VkCommandBuffer, VkRenderPass wrappers
- [ ] Vertex input via VkVertexInputAttributeDescription (explicit offsets, like Metal/D3D11)
- [ ] Shader compilation: GLSL/SPIR-V from IR at compile time via source generator
- [ ] Linux desktop support (X11/Wayland + Vulkan)

## Compute Shaders

- [ ] Add `IComputeShader<TBindings>` interface with `Execute` method
- [ ] Add `IrComputeStage` to the IR with workgroup size
- [ ] Generate HLSL compute shader (`[numthreads(X,Y,Z)]`), MSL kernel (`[[kernel]]`), SPIR-V compute
- [ ] Add `IGpuCommandList.Dispatch(groupCountX, groupCountY, groupCountZ)`
- [ ] Add storage buffer and storage texture resource types
- [ ] Software compute shader execution on CPU (for testing)

## Advanced Features

- [ ] Instancing support (per-instance vertex attributes)
- [ ] Multisampling (MSAA render targets)
- [ ] Texture sampling in shaders (Texture2D + Sampler)
- [ ] Multiple render targets (MRT)
- [ ] Shader hot reload for development
- [ ] IR optimization passes
