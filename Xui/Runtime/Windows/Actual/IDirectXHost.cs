using System.Runtime.InteropServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Debug;
using static Xui.Runtime.Windows.Win32.User32.Types;

namespace Xui.Runtime.Windows.Actual;

/// <summary>
/// Abstracts the context that <see cref="DirectXContext"/> renders into —
/// either a regular <see cref="Win32Window"/> or a <see cref="Win32Popup"/>.
/// </summary>
internal interface IDirectXHost
{
    HWND Hwnd { get; }
    NFloat ExtendedFrameTopOffset { get; }
    InstrumentsAccessor Instruments { get; }
    void Render(RenderEventRef render);
}
