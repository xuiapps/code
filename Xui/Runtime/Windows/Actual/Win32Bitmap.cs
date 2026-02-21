using System;

namespace Xui.Runtime.Windows.Actual;

/// <summary>
/// Platform-specific bitmap that owns a <c>ID3D11Texture2D</c> and a <c>ID2D1Bitmap1</c>
/// wrapping the same GPU surface. Created by <see cref="Direct2DContext"/> via the WIC
/// decode → D3D11 upload → D2D1 wrap pipeline.
/// </summary>
public sealed class Win32Bitmap : Xui.Core.Canvas.Bitmap, IDisposable
{
    internal readonly D3D11.Texture2D Texture2D;
    internal readonly D2D1.Bitmap1 D2D1Bitmap;

    public override uint Width { get; }
    public override uint Height { get; }

    public Win32Bitmap(D3D11.Texture2D texture2D, D2D1.Bitmap1 d2d1Bitmap, uint width, uint height)
    {
        this.Texture2D = texture2D;
        this.D2D1Bitmap = d2d1Bitmap;
        this.Width = width;
        this.Height = height;
    }

    public void Dispose()
    {
        this.D2D1Bitmap.Dispose();
        this.Texture2D.Dispose();
    }
}
