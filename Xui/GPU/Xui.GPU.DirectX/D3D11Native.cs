using System.Runtime.InteropServices;

namespace Xui.GPU.Hardware.D3D11;

/// <summary>
/// P/Invoke declarations for Direct3D 11 native API.
/// </summary>
internal static unsafe partial class D3D11Native
{
    private const string D3D11Lib = "d3d11.dll";

    internal const uint D3D11SdkVersion = 7;

    [Flags]
    internal enum DeviceCreationFlags : uint
    {
        None = 0,
        SingleThreaded = 0x1,
        Debug = 0x2,
        BgraSupport = 0x20,
    }

    internal enum DriverType
    {
        Unknown = 0,
        Hardware = 1,
        Reference = 2,
        Null = 3,
        Software = 4,
        Warp = 5,
    }

    internal enum Format : uint
    {
        Unknown = 0,
        R8G8B8A8Unorm = 28,
        B8G8R8A8Unorm = 87,
        D24UnormS8Uint = 45,
        D32Float = 40,
    }

    internal enum BindFlags : uint
    {
        None = 0,
        VertexBuffer = 0x1,
        IndexBuffer = 0x2,
        ConstantBuffer = 0x4,
        ShaderResource = 0x8,
        RenderTarget = 0x20,
        DepthStencil = 0x40,
    }

    internal enum Usage : uint
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        Staging = 3,
    }

    internal enum CpuAccessFlags : uint
    {
        None = 0,
        Write = 0x10000,
        Read = 0x20000,
    }

    internal enum Map : uint
    {
        Read = 1,
        Write = 2,
        ReadWrite = 3,
        WriteDiscard = 4,
        WriteNoOverwrite = 5,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Texture2DDesc
    {
        public uint Width;
        public uint Height;
        public uint MipLevels;
        public uint ArraySize;
        public Format Format;
        public SampleDesc SampleDesc;
        public Usage Usage;
        public uint BindFlags;
        public uint CPUAccessFlags;
        public uint MiscFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SampleDesc
    {
        public uint Count;
        public uint Quality;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BufferDesc
    {
        public uint ByteWidth;
        public Usage Usage;
        public uint BindFlags;
        public uint CPUAccessFlags;
        public uint MiscFlags;
        public uint StructureByteStride;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SubresourceData
    {
        public void* pSysMem;
        public uint SysMemPitch;
        public uint SysMemSlicePitch;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MappedSubresource
    {
        public void* pData;
        public uint RowPitch;
        public uint DepthPitch;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Viewport
    {
        public float TopLeftX;
        public float TopLeftY;
        public float Width;
        public float Height;
        public float MinDepth;
        public float MaxDepth;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct InputElementDesc
    {
        public byte* SemanticName;
        public uint SemanticIndex;
        public Format Format;
        public uint InputSlot;
        public uint AlignedByteOffset;
        public uint InputSlotClass;  // 0 = PER_VERTEX_DATA
        public uint InstanceDataStepRate;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RenderTargetBlendDesc
    {
        public int BlendEnable;
        public uint SrcBlend;
        public uint DestBlend;
        public uint BlendOp;
        public uint SrcBlendAlpha;
        public uint DestBlendAlpha;
        public uint BlendOpAlpha;
        public byte RenderTargetWriteMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct BlendDesc
    {
        public int AlphaToCoverageEnable;
        public int IndependentBlendEnable;
        public fixed byte RenderTarget[8 * 32]; // 8 RenderTargetBlendDesc
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DepthStencilDesc
    {
        public int DepthEnable;
        public uint DepthWriteMask;   // 0 = ZERO, 1 = ALL
        public uint DepthFunc;        // 4 = LESS
        public int StencilEnable;
        public byte StencilReadMask;
        public byte StencilWriteMask;
        public DepthStencilOpDesc FrontFace;
        public DepthStencilOpDesc BackFace;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DepthStencilOpDesc
    {
        public uint StencilFailOp;      // 1 = KEEP
        public uint StencilDepthFailOp; // 1 = KEEP
        public uint StencilPassOp;      // 1 = KEEP
        public uint StencilFunc;        // 8 = ALWAYS
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RasterizerDesc
    {
        public uint FillMode;   // 3 = SOLID
        public uint CullMode;   // 1 = NONE, 2 = FRONT, 3 = BACK
        public int FrontCounterClockwise;
        public int DepthBias;
        public float DepthBiasClamp;
        public float SlopeScaledDepthBias;
        public int DepthClipEnable;
        public int ScissorEnable;
        public int MultisampleEnable;
        public int AntialiasedLineEnable;
    }

    // D3D11CreateDevice
    [LibraryImport(D3D11Lib, EntryPoint = "D3D11CreateDevice")]
    internal static partial int D3D11CreateDevice(
        void* adapter,
        DriverType driverType,
        void* software,
        DeviceCreationFlags flags,
        void* featureLevels,
        uint numFeatureLevels,
        uint sdkVersion,
        void** device,
        void* featureLevel,
        void** immediateContext);

    /// <summary>
    /// Releases a COM object (calls IUnknown::Release via vtable).
    /// </summary>
    internal static uint Release(void* comObject)
    {
        var vtable = *(void***)comObject;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint>)vtable[2];
        return fn(comObject);
    }

    // ---- ID3D11Device methods ----

    /// <summary>Calls ID3D11Device::CreateTexture2D (vtable index 5).</summary>
    internal static int Device_CreateTexture2D(void* device, Texture2DDesc* pDesc, SubresourceData* pInitialData, void** ppTexture2D)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, Texture2DDesc*, SubresourceData*, void**, int>)vtable[5];
        return fn(device, pDesc, pInitialData, ppTexture2D);
    }

    /// <summary>Calls ID3D11Device::CreateRenderTargetView (vtable index 9).</summary>
    internal static int Device_CreateRenderTargetView(void* device, void* pResource, void* pDesc, void** ppRTView)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void*, void**, int>)vtable[9];
        return fn(device, pResource, pDesc, ppRTView);
    }

    /// <summary>Calls ID3D11Device::CreateDepthStencilView (vtable index 10).</summary>
    internal static int Device_CreateDepthStencilView(void* device, void* pResource, void* pDesc, void** ppDepthStencilView)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void*, void**, int>)vtable[10];
        return fn(device, pResource, pDesc, ppDepthStencilView);
    }

    /// <summary>Calls ID3D11Device::CreateVertexShader (vtable index 12).</summary>
    internal static int Device_CreateVertexShader(void* device, void* pShaderBytecode, nuint bytecodeLength, void* pClassLinkage, void** ppVertexShader)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, nuint, void*, void**, int>)vtable[12];
        return fn(device, pShaderBytecode, bytecodeLength, pClassLinkage, ppVertexShader);
    }

    /// <summary>Calls ID3D11Device::CreatePixelShader (vtable index 15).</summary>
    internal static int Device_CreatePixelShader(void* device, void* pShaderBytecode, nuint bytecodeLength, void* pClassLinkage, void** ppPixelShader)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, nuint, void*, void**, int>)vtable[15];
        return fn(device, pShaderBytecode, bytecodeLength, pClassLinkage, ppPixelShader);
    }

    /// <summary>Calls ID3D11Device::CreateBuffer (vtable index 3).</summary>
    internal static int Device_CreateBuffer(void* device, BufferDesc* pDesc, SubresourceData* pInitialData, void** ppBuffer)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, BufferDesc*, SubresourceData*, void**, int>)vtable[3];
        return fn(device, pDesc, pInitialData, ppBuffer);
    }

    /// <summary>Calls ID3D11Device::CreateInputLayout (vtable index 11).</summary>
    internal static int Device_CreateInputLayout(void* device, InputElementDesc* pInputElementDescs, uint numElements, void* pShaderBytecodeWithInputSignature, nuint bytecodeLength, void** ppInputLayout)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, InputElementDesc*, uint, void*, nuint, void**, int>)vtable[11];
        return fn(device, pInputElementDescs, numElements, pShaderBytecodeWithInputSignature, bytecodeLength, ppInputLayout);
    }

    /// <summary>Calls ID3D11Device::CreateDepthStencilState (vtable index 26).</summary>
    internal static int Device_CreateDepthStencilState(void* device, DepthStencilDesc* pDepthStencilDesc, void** ppDepthStencilState)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, DepthStencilDesc*, void**, int>)vtable[26];
        return fn(device, pDepthStencilDesc, ppDepthStencilState);
    }

    /// <summary>Calls ID3D11Device::CreateRasterizerState (vtable index 27).</summary>
    internal static int Device_CreateRasterizerState(void* device, RasterizerDesc* pRasterizerDesc, void** ppRasterizerState)
    {
        var vtable = *(void***)device;
        var fn = (delegate* unmanaged[Stdcall]<void*, RasterizerDesc*, void**, int>)vtable[27];
        return fn(device, pRasterizerDesc, ppRasterizerState);
    }

    // ---- ID3D11DeviceContext methods ----

    /// <summary>Calls ID3D11DeviceContext::ClearRenderTargetView (vtable index 50).</summary>
    internal static void Context_ClearRenderTargetView(void* context, void* pRenderTargetView, float* colorRGBA)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, float*, void>)vtable[50];
        fn(context, pRenderTargetView, colorRGBA);
    }

    /// <summary>Calls ID3D11DeviceContext::ClearDepthStencilView (vtable index 53).</summary>
    internal static void Context_ClearDepthStencilView(void* context, void* pDepthStencilView, uint clearFlags, float depth, byte stencil)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, uint, float, byte, void>)vtable[53];
        fn(context, pDepthStencilView, clearFlags, depth, stencil);
    }

    /// <summary>Calls ID3D11DeviceContext::OMSetRenderTargets (vtable index 33).</summary>
    internal static void Context_OMSetRenderTargets(void* context, uint numViews, void** ppRenderTargetViews, void* pDepthStencilView)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, void**, void*, void>)vtable[33];
        fn(context, numViews, ppRenderTargetViews, pDepthStencilView);
    }

    /// <summary>Calls ID3D11DeviceContext::RSSetViewports (vtable index 44).</summary>
    internal static void Context_RSSetViewports(void* context, uint numViewports, Viewport* pViewports)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, Viewport*, void>)vtable[44];
        fn(context, numViewports, pViewports);
    }

    /// <summary>Calls ID3D11DeviceContext::IASetInputLayout (vtable index 17).</summary>
    internal static void Context_IASetInputLayout(void* context, void* pInputLayout)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void>)vtable[17];
        fn(context, pInputLayout);
    }

    /// <summary>Calls ID3D11DeviceContext::IASetVertexBuffers (vtable index 18).</summary>
    internal static void Context_IASetVertexBuffers(void* context, uint startSlot, uint numBuffers, void** ppVertexBuffers, uint* pStrides, uint* pOffsets)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, uint, void**, uint*, uint*, void>)vtable[18];
        fn(context, startSlot, numBuffers, ppVertexBuffers, pStrides, pOffsets);
    }

    /// <summary>Calls ID3D11DeviceContext::IASetPrimitiveTopology (vtable index 24).</summary>
    internal static void Context_IASetPrimitiveTopology(void* context, uint topology)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, void>)vtable[24];
        fn(context, topology);
    }

    /// <summary>Calls ID3D11DeviceContext::VSSetShader (vtable index 11).</summary>
    internal static void Context_VSSetShader(void* context, void* pVertexShader, void** ppClassInstances, uint numClassInstances)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void**, uint, void>)vtable[11];
        fn(context, pVertexShader, ppClassInstances, numClassInstances);
    }

    /// <summary>Calls ID3D11DeviceContext::VSSetConstantBuffers (vtable index 7).</summary>
    internal static void Context_VSSetConstantBuffers(void* context, uint startSlot, uint numBuffers, void** ppConstantBuffers)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, uint, void**, void>)vtable[7];
        fn(context, startSlot, numBuffers, ppConstantBuffers);
    }

    /// <summary>Calls ID3D11DeviceContext::PSSetShader (vtable index 9).</summary>
    internal static void Context_PSSetShader(void* context, void* pPixelShader, void** ppClassInstances, uint numClassInstances)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void**, uint, void>)vtable[9];
        fn(context, pPixelShader, ppClassInstances, numClassInstances);
    }

    /// <summary>Calls ID3D11DeviceContext::PSSetConstantBuffers (vtable index 16).</summary>
    internal static void Context_PSSetConstantBuffers(void* context, uint startSlot, uint numBuffers, void** ppConstantBuffers)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, uint, void**, void>)vtable[16];
        fn(context, startSlot, numBuffers, ppConstantBuffers);
    }

    /// <summary>Calls ID3D11DeviceContext::OMSetDepthStencilState (vtable index 35).</summary>
    internal static void Context_OMSetDepthStencilState(void* context, void* pDepthStencilState, uint stencilRef)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, uint, void>)vtable[35];
        fn(context, pDepthStencilState, stencilRef);
    }

    /// <summary>Calls ID3D11DeviceContext::RSSetState (vtable index 43).</summary>
    internal static void Context_RSSetState(void* context, void* pRasterizerState)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void>)vtable[43];
        fn(context, pRasterizerState);
    }

    /// <summary>Calls ID3D11DeviceContext::Draw (vtable index 73).</summary>
    internal static void Context_Draw(void* context, uint vertexCount, uint startVertexLocation)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, uint, uint, void>)vtable[73];
        fn(context, vertexCount, startVertexLocation);
    }

    /// <summary>Calls ID3D11DeviceContext::Flush (vtable index 114).</summary>
    internal static void Context_Flush(void* context)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void>)vtable[114];
        fn(context);
    }

    /// <summary>Calls ID3D11DeviceContext::Map (vtable index 14).</summary>
    internal static int Context_Map(void* context, void* pResource, uint subresource, Map mapType, uint mapFlags, MappedSubresource* pMappedResource)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, uint, Map, uint, MappedSubresource*, int>)vtable[14];
        return fn(context, pResource, subresource, mapType, mapFlags, pMappedResource);
    }

    /// <summary>Calls ID3D11DeviceContext::Unmap (vtable index 15).</summary>
    internal static void Context_Unmap(void* context, void* pResource, uint subresource)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, uint, void>)vtable[15];
        fn(context, pResource, subresource);
    }

    /// <summary>Calls ID3D11DeviceContext::UpdateSubresource (vtable index 68).</summary>
    internal static void Context_UpdateSubresource(void* context, void* pDstResource, uint dstSubresource, void* pDstBox, void* pSrcData, uint srcRowPitch, uint srcDepthPitch)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, uint, void*, void*, uint, uint, void>)vtable[68];
        fn(context, pDstResource, dstSubresource, pDstBox, pSrcData, srcRowPitch, srcDepthPitch);
    }

    /// <summary>Calls ID3D11DeviceContext::CopyResource (vtable index 47).</summary>
    internal static void Context_CopyResource(void* context, void* pDstResource, void* pSrcResource)
    {
        var vtable = *(void***)context;
        var fn = (delegate* unmanaged[Stdcall]<void*, void*, void*, void>)vtable[47];
        fn(context, pDstResource, pSrcResource);
    }
}
