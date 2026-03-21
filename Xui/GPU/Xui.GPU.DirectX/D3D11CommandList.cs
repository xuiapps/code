using System.Runtime.InteropServices;

namespace Xui.GPU.Hardware.D3D11;

/// <summary>
/// D3D11 GPU command list for recording and executing rendering commands.
/// </summary>
internal sealed unsafe class D3D11CommandList : IGpuCommandList
{
    private readonly void* _device;
    private readonly void* _context;

    // Currently bound state
    private D3D11RenderTarget? _currentRenderTarget;
    private D3D11VertexShader? _currentVertexShader;
    private D3D11FragmentShader? _currentFragmentShader;
    private void* _currentVertexBuffer;
    private uint _currentStride;
    private void* _currentConstantBuffer;
    private bool _disposed;

    internal D3D11CommandList(void* device, void* context)
    {
        _device = device;
        _context = context;
    }

    /// <inheritdoc/>
    public void BeginRenderPass(IGpuRenderTarget renderTarget, GpuClearColor clearColor)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _currentRenderTarget = (D3D11RenderTarget)renderTarget;

        var rtv = _currentRenderTarget.NativeRtv;
        var dsv = _currentRenderTarget.NativeDsv;

        // Set render targets
        D3D11Native.Context_OMSetRenderTargets(_context, 1, &rtv, dsv);

        // Clear color
        var color = stackalloc float[4] { clearColor.R, clearColor.G, clearColor.B, clearColor.A };
        D3D11Native.Context_ClearRenderTargetView(_context, rtv, color);

        // Clear depth (0x1 = DEPTH, 0x2 = STENCIL)
        D3D11Native.Context_ClearDepthStencilView(_context, dsv, 0x1 | 0x2, 1.0f, 0);

        // Set viewport
        var viewport = new D3D11Native.Viewport
        {
            TopLeftX = 0,
            TopLeftY = 0,
            Width = renderTarget.Width,
            Height = renderTarget.Height,
            MinDepth = 0,
            MaxDepth = 1
        };
        D3D11Native.Context_RSSetViewports(_context, 1, &viewport);
    }

    /// <inheritdoc/>
    public void EndRenderPass()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        _currentRenderTarget = null;
    }

    /// <inheritdoc/>
    public void SetPipeline(GpuPipelineDesc pipeline)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _currentVertexShader = pipeline.VertexShader as D3D11VertexShader;
        _currentFragmentShader = pipeline.FragmentShader as D3D11FragmentShader;

        // Set vertex shader
        void* vsNative = _currentVertexShader != null ? _currentVertexShader.NativeShader : null;
        D3D11Native.Context_VSSetShader(_context, vsNative, null, 0);

        // Set pixel shader
        void* psNative = _currentFragmentShader != null ? _currentFragmentShader.NativeShader : null;
        D3D11Native.Context_PSSetShader(_context, psNative, null, 0);

        // Set depth stencil state
        var dsDesc = new D3D11Native.DepthStencilDesc
        {
            DepthEnable = pipeline.DepthTestEnabled ? 1 : 0,
            DepthWriteMask = pipeline.DepthWriteEnabled ? 1u : 0u,
            DepthFunc = 4u,   // D3D11_COMPARISON_LESS
            StencilEnable = 0,
            StencilReadMask = 0xFF,
            StencilWriteMask = 0xFF,
            FrontFace = new D3D11Native.DepthStencilOpDesc
            {
                StencilFailOp = 1u,     // D3D11_STENCIL_OP_KEEP
                StencilDepthFailOp = 1u,
                StencilPassOp = 1u,
                StencilFunc = 8u,       // D3D11_COMPARISON_ALWAYS
            },
            BackFace = new D3D11Native.DepthStencilOpDesc
            {
                StencilFailOp = 1u,
                StencilDepthFailOp = 1u,
                StencilPassOp = 1u,
                StencilFunc = 8u,
            },
        };
        void* depthStencilState = null;
        int hr = D3D11Native.Device_CreateDepthStencilState(_device, &dsDesc, &depthStencilState);
        Marshal.ThrowExceptionForHR(hr);
        D3D11Native.Context_OMSetDepthStencilState(_context, depthStencilState, 0);
        D3D11Native.Release(depthStencilState);

        // Set rasterizer state with cull mode from pipeline descriptor
        uint d3dCullMode = pipeline.CullMode switch
        {
            GpuCullMode.None => 1u,   // D3D11_CULL_NONE
            GpuCullMode.Front => 2u,  // D3D11_CULL_FRONT
            GpuCullMode.Back => 3u,   // D3D11_CULL_BACK
            _ => 1u,
        };
        var rsDesc = new D3D11Native.RasterizerDesc
        {
            FillMode = 3u,   // D3D11_FILL_SOLID
            CullMode = d3dCullMode,
            FrontCounterClockwise = 1, // CCW winding = front-facing (matching Metal/OpenGL convention)
            DepthBias = 0,
            DepthBiasClamp = 0f,
            SlopeScaledDepthBias = 0f,
            DepthClipEnable = 1,
            ScissorEnable = 0,
            MultisampleEnable = 0,
            AntialiasedLineEnable = 0,
        };
        void* rasterizerState = null;
        hr = D3D11Native.Device_CreateRasterizerState(_device, &rsDesc, &rasterizerState);
        Marshal.ThrowExceptionForHR(hr);
        D3D11Native.Context_RSSetState(_context, rasterizerState);
        D3D11Native.Release(rasterizerState);

        // Set primitive topology (triangle list)
        const uint D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST = 4;
        D3D11Native.Context_IASetPrimitiveTopology(_context, D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
    }

    /// <inheritdoc/>
    public void SetVertexBuffer(void* data, GpuVertexBufferDesc desc)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _currentStride = (uint)desc.Stride;

        // Release previous vertex buffer
        if (_currentVertexBuffer != null)
        {
            D3D11Native.Release(_currentVertexBuffer);
            _currentVertexBuffer = null;
        }

        // Create a new vertex buffer
        var bufDesc = new D3D11Native.BufferDesc
        {
            ByteWidth = (uint)(desc.Stride * desc.VertexCount),
            Usage = D3D11Native.Usage.Immutable,
            BindFlags = (uint)D3D11Native.BindFlags.VertexBuffer,
            CPUAccessFlags = 0,
            MiscFlags = 0,
            StructureByteStride = 0
        };

        var initData = new D3D11Native.SubresourceData
        {
            pSysMem = data,
            SysMemPitch = 0,
            SysMemSlicePitch = 0
        };

        void* buffer = null;
        int hr = D3D11Native.Device_CreateBuffer(_device, &bufDesc, &initData, &buffer);
        Marshal.ThrowExceptionForHR(hr);
        _currentVertexBuffer = buffer;

        // Bind vertex buffer
        var stride = _currentStride;
        uint offset = 0;
        D3D11Native.Context_IASetVertexBuffers(_context, 0, 1, &buffer, &stride, &offset);

        // Create input layout from vertex shader bytecode
        // For simplicity: position (float3=12 bytes) + color (float4=16 bytes) = 28 bytes per vertex
        // This matches the cube shader layout
        if (_currentVertexShader != null)
        {
            CreateAndSetInputLayout(_currentVertexShader.Bytecode);
        }
    }

    private void CreateAndSetInputLayout(byte[] vertexShaderBytecode)
    {
        // Input layout for CubeVertex: Float3 Position, Color4 (Float4) Color
        // The HLSL semantic names must match what the HLSL generator produces (TEXCOORD0, TEXCOORD1)
        var posSemanticBytes = System.Text.Encoding.ASCII.GetBytes("TEXCOORD\0");
        var colorSemanticBytes = System.Text.Encoding.ASCII.GetBytes("TEXCOORD\0");

        fixed (byte* posName = posSemanticBytes)
        fixed (byte* colorName = colorSemanticBytes)
        {
            var elements = stackalloc D3D11Native.InputElementDesc[2];

            // Position: TEXCOORD0, float3 (R32G32B32_FLOAT = 6 in DXGI)
            elements[0] = new D3D11Native.InputElementDesc
            {
                SemanticName = posName,
                SemanticIndex = 0,
                Format = (D3D11Native.Format)6,   // DXGI_FORMAT_R32G32B32_FLOAT
                InputSlot = 0,
                AlignedByteOffset = 0,
                InputSlotClass = 0,  // PER_VERTEX_DATA
                InstanceDataStepRate = 0
            };

            // Color: TEXCOORD1, float4 (R32G32B32A32_FLOAT = 2 in DXGI)
            elements[1] = new D3D11Native.InputElementDesc
            {
                SemanticName = colorName,
                SemanticIndex = 1,
                Format = (D3D11Native.Format)2,   // DXGI_FORMAT_R32G32B32A32_FLOAT
                InputSlot = 0,
                AlignedByteOffset = 12,  // After float3 position
                InputSlotClass = 0,
                InstanceDataStepRate = 0
            };

            void* inputLayout = null;
            fixed (byte* pBytecode = vertexShaderBytecode)
            {
                int hr = D3D11Native.Device_CreateInputLayout(
                    _device, elements, 2, pBytecode, (nuint)vertexShaderBytecode.Length, &inputLayout);
                Marshal.ThrowExceptionForHR(hr);
            }

            D3D11Native.Context_IASetInputLayout(_context, inputLayout);
            D3D11Native.Release(inputLayout);
        }
    }

    /// <inheritdoc/>
    public void SetConstantBuffer(void* data, int sizeInBytes)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        // Release previous constant buffer
        if (_currentConstantBuffer != null)
        {
            D3D11Native.Release(_currentConstantBuffer);
            _currentConstantBuffer = null;
        }

        // Constant buffer sizes must be multiples of 16 bytes (Direct3D 11 requirement:
        // D3D11_REQ_CONSTANT_BUFFER_ELEMENT_COUNT per D3D11 spec, CBs are indexed in 16-byte chunks).
        int alignedSize = (sizeInBytes + 15) & ~15;

        var bufDesc = new D3D11Native.BufferDesc
        {
            ByteWidth = (uint)alignedSize,
            Usage = D3D11Native.Usage.Default,
            BindFlags = (uint)D3D11Native.BindFlags.ConstantBuffer,
            CPUAccessFlags = 0,
            MiscFlags = 0,
            StructureByteStride = 0
        };

        void* buffer = null;
        int hr;

        if (data != null)
        {
            // If we need to pad (align to 16 bytes), allocate a padded copy
            if (alignedSize > sizeInBytes)
            {
                var padded = stackalloc byte[alignedSize];
                Buffer.MemoryCopy(data, padded, alignedSize, sizeInBytes);

                var initData = new D3D11Native.SubresourceData
                {
                    pSysMem = padded,
                    SysMemPitch = 0,
                    SysMemSlicePitch = 0
                };
                hr = D3D11Native.Device_CreateBuffer(_device, &bufDesc, &initData, &buffer);
            }
            else
            {
                var initData = new D3D11Native.SubresourceData
                {
                    pSysMem = data,
                    SysMemPitch = 0,
                    SysMemSlicePitch = 0
                };
                hr = D3D11Native.Device_CreateBuffer(_device, &bufDesc, &initData, &buffer);
            }
        }
        else
        {
            hr = D3D11Native.Device_CreateBuffer(_device, &bufDesc, null, &buffer);
        }

        Marshal.ThrowExceptionForHR(hr);
        _currentConstantBuffer = buffer;

        // Bind to vertex and pixel shader constant buffer slot 0
        D3D11Native.Context_VSSetConstantBuffers(_context, 0, 1, &buffer);
        D3D11Native.Context_PSSetConstantBuffers(_context, 0, 1, &buffer);
    }

    /// <inheritdoc/>
    public void Draw(int vertexCount)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        D3D11Native.Context_Draw(_context, (uint)vertexCount, 0);
    }

    /// <inheritdoc/>
    public void Commit()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        D3D11Native.Context_Flush(_context);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_currentVertexBuffer != null)
        {
            D3D11Native.Release(_currentVertexBuffer);
            _currentVertexBuffer = null;
        }

        if (_currentConstantBuffer != null)
        {
            D3D11Native.Release(_currentConstantBuffer);
            _currentConstantBuffer = null;
        }
    }
}
