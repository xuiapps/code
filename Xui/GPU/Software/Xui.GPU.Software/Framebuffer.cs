using System;
using System.Runtime.InteropServices;

namespace Xui.GPU.Software;

/// <summary>
/// Represents a framebuffer with RGBA32 color data and optional depth buffer.
/// </summary>
/// <remarks>
/// The framebuffer stores pixel data in RGBA format with 8 bits per channel.
/// Coordinates are in screen space with origin at top-left (0,0).
/// </remarks>
public unsafe struct Framebuffer : IDisposable
{
    private readonly int _width;
    private readonly int _height;
    private readonly uint* _colorData;
    private readonly float* _depthData;
    private readonly bool _ownsMemory;

    /// <summary>
    /// Gets the width of the framebuffer in pixels.
    /// </summary>
    public readonly int Width => _width;

    /// <summary>
    /// Gets the height of the framebuffer in pixels.
    /// </summary>
    public readonly int Height => _height;

    /// <summary>
    /// Gets a pointer to the RGBA32 color data.
    /// </summary>
    /// <remarks>
    /// Color data is stored as packed RGBA with 8 bits per channel.
    /// Format: 0xRRGGBBAA
    /// </remarks>
    public readonly uint* ColorData => _colorData;

    /// <summary>
    /// Gets a pointer to the depth buffer data, or null if no depth buffer.
    /// </summary>
    /// <remarks>
    /// Depth values are stored as normalized floats in range [0.0, 1.0].
    /// 0.0 represents the near plane, 1.0 represents the far plane.
    /// </remarks>
    public readonly float* DepthData => _depthData;

    /// <summary>
    /// Gets whether this framebuffer has a depth buffer.
    /// </summary>
    public readonly bool HasDepthBuffer => _depthData != null;

    /// <summary>
    /// Creates a new framebuffer with the specified dimensions.
    /// </summary>
    /// <param name="width">Width in pixels. Must be greater than 0.</param>
    /// <param name="height">Height in pixels. Must be greater than 0.</param>
    /// <param name="withDepthBuffer">Whether to allocate a depth buffer.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when width or height is less than or equal to 0.
    /// </exception>
    public Framebuffer(int width, int height, bool withDepthBuffer = true)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than 0");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than 0");

        _width = width;
        _height = height;
        _ownsMemory = true;

        // Allocate color buffer
        var colorBytes = width * height * sizeof(uint);
        _colorData = (uint*)NativeMemory.AllocZeroed((nuint)colorBytes);

        // Allocate depth buffer if requested
        if (withDepthBuffer)
        {
            var depthBytes = width * height * sizeof(float);
            _depthData = (float*)NativeMemory.AllocZeroed((nuint)depthBytes);
            
            // Initialize depth buffer to far plane (1.0)
            for (int i = 0; i < width * height; i++)
                _depthData[i] = 1.0f;
        }
        else
        {
            _depthData = null;
        }
    }

    /// <summary>
    /// Creates a framebuffer wrapping existing memory buffers.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="colorData">Pointer to RGBA32 color data.</param>
    /// <param name="depthData">Optional pointer to depth buffer data.</param>
    /// <remarks>
    /// This constructor does not take ownership of the memory buffers.
    /// The caller is responsible for managing the lifetime of the buffers.
    /// </remarks>
    public Framebuffer(int width, int height, uint* colorData, float* depthData = null)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));
        if (colorData == null)
            throw new ArgumentNullException(nameof(colorData));

        _width = width;
        _height = height;
        _colorData = colorData;
        _depthData = depthData;
        _ownsMemory = false;
    }

    /// <summary>
    /// Clears the color buffer to the specified color.
    /// </summary>
    /// <param name="color">RGBA32 color value (0xRRGGBBAA).</param>
    public readonly void ClearColor(uint color)
    {
        for (int i = 0; i < _width * _height; i++)
            _colorData[i] = color;
    }

    /// <summary>
    /// Clears the depth buffer to the specified depth value.
    /// </summary>
    /// <param name="depth">Depth value in range [0.0, 1.0]. Default is 1.0 (far plane).</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the framebuffer has no depth buffer.
    /// </exception>
    public readonly void ClearDepth(float depth = 1.0f)
    {
        if (_depthData == null)
            throw new InvalidOperationException("Framebuffer has no depth buffer");

        for (int i = 0; i < _width * _height; i++)
            _depthData[i] = depth;
    }

    /// <summary>
    /// Sets the color at the specified pixel coordinate.
    /// </summary>
    /// <param name="x">X coordinate in pixels.</param>
    /// <param name="y">Y coordinate in pixels.</param>
    /// <param name="color">RGBA32 color value.</param>
    public readonly void SetColor(int x, int y, uint color)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return;
        _colorData[y * _width + x] = color;
    }

    /// <summary>
    /// Gets the color at the specified pixel coordinate.
    /// </summary>
    /// <param name="x">X coordinate in pixels.</param>
    /// <param name="y">Y coordinate in pixels.</param>
    /// <returns>RGBA32 color value, or 0 if coordinates are out of bounds.</returns>
    public readonly uint GetColor(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return 0;
        return _colorData[y * _width + x];
    }

    /// <summary>
    /// Sets the depth at the specified pixel coordinate.
    /// </summary>
    /// <param name="x">X coordinate in pixels.</param>
    /// <param name="y">Y coordinate in pixels.</param>
    /// <param name="depth">Depth value in range [0.0, 1.0].</param>
    public readonly void SetDepth(int x, int y, float depth)
    {
        if (_depthData == null || x < 0 || x >= _width || y < 0 || y >= _height)
            return;
        _depthData[y * _width + x] = depth;
    }

    /// <summary>
    /// Gets the depth at the specified pixel coordinate.
    /// </summary>
    /// <param name="x">X coordinate in pixels.</param>
    /// <param name="y">Y coordinate in pixels.</param>
    /// <returns>Depth value in range [0.0, 1.0], or 1.0 if coordinates are out of bounds or no depth buffer.</returns>
    public readonly float GetDepth(int x, int y)
    {
        if (_depthData == null || x < 0 || x >= _width || y < 0 || y >= _height)
            return 1.0f;
        return _depthData[y * _width + x];
    }

    /// <summary>
    /// Copies the color data to a managed byte array in RGBA format.
    /// </summary>
    /// <returns>Byte array containing RGBA color data.</returns>
    public readonly byte[] ToByteArray()
    {
        var bytes = new byte[_width * _height * 4];
        fixed (byte* dest = bytes)
        {
            Buffer.MemoryCopy(_colorData, dest, bytes.Length, bytes.Length);
        }
        return bytes;
    }

    /// <summary>
    /// Releases the framebuffer memory if owned by this instance.
    /// </summary>
    public void Dispose()
    {
        if (_ownsMemory)
        {
            if (_colorData != null)
                NativeMemory.Free(_colorData);
            if (_depthData != null)
                NativeMemory.Free(_depthData);
        }
    }
}
