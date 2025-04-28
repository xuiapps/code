using Xui.Core.Canvas;

namespace Xui.Runtime.Software;

public partial class SoftwareContext
{
    public RGBABitmap bitmap { get; }
    public G16Stencil stencil { get; }

    public SoftwareContext(uint width, uint height)
    {
        this.bitmap = new RGBABitmap(width, height);
        this.stencil = new G16Stencil(width, height);
    }
}
