using System;
using System.Runtime.InteropServices;
using Xui.Runtime.Windows;
using static Xui.Runtime.Windows.D3D11;
using static Xui.Runtime.Windows.COM;

namespace Xui.GPU.Hardware.D3D11;

/// <summary>
/// D3D11 compiled vertex shader.
/// </summary>
internal sealed unsafe class DirectXVertexShader : IGpuVertexShader
{
    private readonly void* _nativeShader;
    private readonly byte[] _bytecode;
    private bool _disposed;

    public string Name { get; }
    internal byte[] Bytecode => _bytecode;

    internal DirectXVertexShader(string name, byte[] bytecode, Device device)
    {
        Name = name;
        _bytecode = bytecode;

        fixed (byte* pBytecode = bytecode)
        {
            _nativeShader = device.CreateVertexShader(pBytecode, (nuint)bytecode.Length, null);
        }
    }

    internal void* NativeShader => _nativeShader;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Unknown.Release(_nativeShader);
    }
}

/// <summary>
/// D3D11 compiled fragment (pixel) shader.
/// </summary>
internal sealed unsafe class DirectXFragmentShader : IGpuFragmentShader
{
    private readonly void* _nativeShader;
    private bool _disposed;

    public string Name { get; }

    internal DirectXFragmentShader(string name, byte[] bytecode, Device device)
    {
        Name = name;

        fixed (byte* pBytecode = bytecode)
        {
            _nativeShader = device.CreatePixelShader(pBytecode, (nuint)bytecode.Length, null);
        }
    }

    internal void* NativeShader => _nativeShader;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Unknown.Release(_nativeShader);
    }
}

/// <summary>
/// D3D11 render target backed by a texture.
/// </summary>
internal sealed unsafe class DirectXRenderTarget : IGpuRenderTarget
{
    private readonly void* _texture;
    private readonly void* _rtv;        // ID3D11RenderTargetView
    private readonly void* _dsv;        // ID3D11DepthStencilView
    private readonly void* _stagingTex; // Staging texture for CPU readback
    private readonly void* _depthTex;   // Depth/stencil texture
    private readonly Device _device;
    private readonly DeviceContext _context;
    private bool _disposed;

    public int Width { get; }
    public int Height { get; }

    public IGpuTexture Texture => new DirectXTexture(_texture, Width, Height);

    internal void* NativeRtv => _rtv;
    internal void* NativeDsv => _dsv;

    internal DirectXRenderTarget(int width, int height, Device device, DeviceContext context)
    {
        Width = width;
        Height = height;
        _device = device;
        _context = context;

        // Create the main render target texture (RGBA)
        var rtDesc = new Texture2DDesc
        {
            Width = (uint)width,
            Height = (uint)height,
            MipLevels = 1,
            ArraySize = 1,
            Format = DXGI.Format.B8G8R8A8_UNORM,
            SampleDesc = new DXGI.SampleDesc { Count = 1, Quality = 0 },
            Usage = (uint)Xui.Runtime.Windows.D3D11.Usage.Default,
            BindFlags = (uint)(BindFlags.RenderTarget | BindFlags.ShaderResource),
            CPUAccessFlags = 0,
            MiscFlags = 0
        };

        _texture = device.CreateTexture2D(rtDesc);

        // Create render target view
        _rtv = device.CreateRenderTargetView(_texture, null);

        // Create the depth texture
        var depthDesc = new Texture2DDesc
        {
            Width = (uint)width,
            Height = (uint)height,
            MipLevels = 1,
            ArraySize = 1,
            Format = DXGI.Format.D24_UNORM_S8_UINT,
            SampleDesc = new DXGI.SampleDesc { Count = 1, Quality = 0 },
            Usage = (uint)Xui.Runtime.Windows.D3D11.Usage.Default,
            BindFlags = (uint)BindFlags.DepthStencil,
            CPUAccessFlags = 0,
            MiscFlags = 0
        };

        _depthTex = device.CreateTexture2D(depthDesc);

        // Create depth stencil view
        _dsv = device.CreateDepthStencilView(_depthTex, null);

        // Create staging texture for CPU readback
        var stagingDesc = new Texture2DDesc
        {
            Width = (uint)width,
            Height = (uint)height,
            MipLevels = 1,
            ArraySize = 1,
            Format = DXGI.Format.B8G8R8A8_UNORM,
            SampleDesc = new DXGI.SampleDesc { Count = 1, Quality = 0 },
            Usage = (uint)Xui.Runtime.Windows.D3D11.Usage.Staging,
            BindFlags = 0,
            CPUAccessFlags = (uint)CpuAccessFlags.Read,
            MiscFlags = 0
        };

        _stagingTex = device.CreateTexture2D(stagingDesc);
    }

    /// <inheritdoc/>
    public void ReadbackPixelsBgra(byte[] destination)
    {
        if (destination == null || destination.Length < Width * Height * 4)
            throw new ArgumentException($"Destination must be at least {Width * Height * 4} bytes.", nameof(destination));

        // Copy render target to staging texture
        _context.CopyResource(_stagingTex, _texture);
        _context.Flush();

        // Map staging texture for CPU read
        var mapped = new MappedSubresource();
        _context.Map(_stagingTex, 0, Xui.Runtime.Windows.D3D11.Map.Read, 0, &mapped);

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
            _context.Unmap(_stagingTex, 0);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        Unknown.Release(_rtv);
        Unknown.Release(_dsv);
        Unknown.Release(_texture);
        Unknown.Release(_depthTex);
        Unknown.Release(_stagingTex);
    }
}

/// <summary>
/// Thin wrapper implementing IGpuTexture for D3D11 textures.
/// </summary>
internal sealed unsafe class DirectXTexture : IGpuTexture
{
    private readonly void* _texture;
    public int Width { get; }
    public int Height { get; }

    internal DirectXTexture(void* texture, int width, int height)
    {
        _texture = texture;
        Width = width;
        Height = height;
    }

    public void Dispose()
    {
        // Note: lifetime is managed by DirectXRenderTarget
    }
}
