using System.Threading.Tasks;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Runtime.MacOS.Actual;

/// <summary>
/// Platform image handle for macOS. Backed by a <see cref="MacOSImageResource"/> that
/// holds a CGImageRef (and, in the future, an MTLTexture for Metal rendering).
/// Instances are vended by <see cref="MacOSImageFactory"/> via the service chain.
/// </summary>
internal sealed class MacOSImage : IImage
{
    private readonly MacOSImageFactory factory;
    private MacOSImageResource? resource;

    internal MacOSImageResource? Resource => resource;

    internal MacOSImage(MacOSImageFactory factory)
    {
        this.factory = factory;
    }

    public Size Size => resource != null
        ? new Size(resource.Width, resource.Height)
        : Size.Empty;

    public void Load(string uri) =>
        resource = factory.GetOrLoad(uri);

    public Task LoadAsync(string uri) =>
        factory.GetOrLoadAsync(uri).ContinueWith(t => resource = t.Result);
}
