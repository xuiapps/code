using System.Runtime.InteropServices;

namespace Xui.GPU.Hardware.D3D11;

/// <summary>
/// D3D11 compiled vertex shader.
/// </summary>
internal sealed unsafe class D3D11VertexShader : IGpuVertexShader
{
    private readonly void* _nativeShader;
    private readonly byte[] _bytecode;
    private bool _disposed;

    public string Name { get; }
    internal byte[] Bytecode => _bytecode;

    internal D3D11VertexShader(string name, byte[] bytecode, void* device)
    {
        Name = name;
        _bytecode = bytecode;

        void* shader = null;
        fixed (byte* pBytecode = bytecode)
        {
            int hr = D3D11Native.Device_CreateVertexShader(
                device, pBytecode, (nuint)bytecode.Length, null, &shader);
            Marshal.ThrowExceptionForHR(hr);
        }
        _nativeShader = shader;
    }

    internal void* NativeShader => _nativeShader;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        D3D11Native.Release(_nativeShader);
    }
}

/// <summary>
/// D3D11 compiled fragment (pixel) shader.
/// </summary>
internal sealed unsafe class D3D11FragmentShader : IGpuFragmentShader
{
    private readonly void* _nativeShader;
    private bool _disposed;

    public string Name { get; }

    internal D3D11FragmentShader(string name, byte[] bytecode, void* device)
    {
        Name = name;

        void* shader = null;
        fixed (byte* pBytecode = bytecode)
        {
            int hr = D3D11Native.Device_CreatePixelShader(
                device, pBytecode, (nuint)bytecode.Length, null, &shader);
            Marshal.ThrowExceptionForHR(hr);
        }
        _nativeShader = shader;
    }

    internal void* NativeShader => _nativeShader;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        D3D11Native.Release(_nativeShader);
    }
}

/// <summary>
/// D3D11 render target backed by a texture.
/// </summary>
internal sealed unsafe class D3D11RenderTarget : IGpuRenderTarget
{
    private readonly void* _texture;
    private readonly void* _rtv;        // ID3D11RenderTargetView
    private readonly void* _dsv;        // ID3D11DepthStencilView
    private readonly void* _stagingTex; // Staging texture for CPU readback
    private readonly void* _depthTex;  // Depth/stencil texture
    private readonly void* _device;
    private readonly void* _context;
    private bool _disposed;

    public int Width { get; }
    public int Height { get; }

    public IGpuTexture Texture => new D3D11Texture(_texture, Width, Height);

    internal void* NativeRtv => _rtv;
    internal void* NativeDsv => _dsv;

    internal D3D11RenderTarget(int width, int height, void* device, void* context)
    {
        Width = width;
        Height = height;
        _device = device;
        _context = context;

        // Create the main render target texture (RGBA)
        var rtDesc = new D3D11Native.Texture2DDesc
        {
            Width = (uint)width,
            Height = (uint)height,
            MipLevels = 1,
            ArraySize = 1,
            Format = D3D11Native.Format.B8G8R8A8Unorm,
            SampleDesc = new D3D11Native.SampleDesc { Count = 1, Quality = 0 },
            Usage = D3D11Native.Usage.Default,
            BindFlags = (uint)(D3D11Native.BindFlags.RenderTarget | D3D11Native.BindFlags.ShaderResource),
            CPUAccessFlags = 0,
            MiscFlags = 0
        };

        void* texture = null;
        int hr = D3D11Native.Device_CreateTexture2D(device, &rtDesc, null, &texture);
        Marshal.ThrowExceptionForHR(hr);
        _texture = texture;

        // Create render target view
        void* rtv = null;
        hr = D3D11Native.Device_CreateRenderTargetView(device, texture, null, &rtv);
        Marshal.ThrowExceptionForHR(hr);
        _rtv = rtv;

        // Create the depth texture
        var depthDesc = new D3D11Native.Texture2DDesc
        {
            Width = (uint)width,
            Height = (uint)height,
            MipLevels = 1,
            ArraySize = 1,
            Format = D3D11Native.Format.D24UnormS8Uint,
            SampleDesc = new D3D11Native.SampleDesc { Count = 1, Quality = 0 },
            Usage = D3D11Native.Usage.Default,
            BindFlags = (uint)D3D11Native.BindFlags.DepthStencil,
            CPUAccessFlags = 0,
            MiscFlags = 0
        };

        void* depthTex = null;
        hr = D3D11Native.Device_CreateTexture2D(device, &depthDesc, null, &depthTex);
        Marshal.ThrowExceptionForHR(hr);
        _depthTex = depthTex;

        // Create depth stencil view
        void* dsv = null;
        hr = D3D11Native.Device_CreateDepthStencilView(device, depthTex, null, &dsv);
        Marshal.ThrowExceptionForHR(hr);
        _dsv = dsv;

        // Create staging texture for CPU readback
        var stagingDesc = new D3D11Native.Texture2DDesc
        {
            Width = (uint)width,
            Height = (uint)height,
            MipLevels = 1,
            ArraySize = 1,
            Format = D3D11Native.Format.B8G8R8A8Unorm,
            SampleDesc = new D3D11Native.SampleDesc { Count = 1, Quality = 0 },
            Usage = D3D11Native.Usage.Staging,
            BindFlags = 0,
            CPUAccessFlags = (uint)D3D11Native.CpuAccessFlags.Read,
            MiscFlags = 0
        };

        void* stagingTex = null;
        hr = D3D11Native.Device_CreateTexture2D(device, &stagingDesc, null, &stagingTex);
        Marshal.ThrowExceptionForHR(hr);
        _stagingTex = stagingTex;
    }

    /// <inheritdoc/>
    public void ReadbackPixelsBgra(byte[] destination)
    {
        if (destination == null || destination.Length < Width * Height * 4)
            throw new ArgumentException($"Destination must be at least {Width * Height * 4} bytes.", nameof(destination));

        // Copy render target to staging texture
        D3D11Native.Context_CopyResource(_context, _stagingTex, _texture);
        D3D11Native.Context_Flush(_context);

        // Map staging texture for CPU read
        var mapped = new D3D11Native.MappedSubresource();
        int hr = D3D11Native.Context_Map(_context, _stagingTex, 0, D3D11Native.Map.Read, 0, &mapped);
        Marshal.ThrowExceptionForHR(hr);

        try
        {
            // Copy pixels to destination
            var src = (byte*)mapped.pData;
            int rowPitch = (int)mapped.RowPitch;
            int rowBytes = Width * 4;

            for (int y = 0; y < Height; y++)
            {
                var srcRow = src + y * rowPitch;
                var dstRow = y * rowBytes;
                Marshal.Copy((nint)srcRow, destination, dstRow, rowBytes);
            }
        }
        finally
        {
            D3D11Native.Context_Unmap(_context, _stagingTex, 0);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        D3D11Native.Release(_rtv);
        D3D11Native.Release(_dsv);
        D3D11Native.Release(_texture);
        D3D11Native.Release(_depthTex);
        D3D11Native.Release(_stagingTex);
    }
}

/// <summary>
/// Thin wrapper implementing IGpuTexture for D3D11 textures.
/// </summary>
internal sealed unsafe class D3D11Texture : IGpuTexture
{
    private readonly void* _texture;
    public int Width { get; }
    public int Height { get; }

    internal D3D11Texture(void* texture, int width, int height)
    {
        _texture = texture;
        Width = width;
        Height = height;
    }

    public void Dispose()
    {
        // Note: lifetime is managed by D3D11RenderTarget
    }
}
