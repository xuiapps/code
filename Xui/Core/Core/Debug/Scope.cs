namespace Xui.Core.Debug;

[Flags]
public enum Scope : uint
{
    Application      = 1 << 0,
    Input            = 1 << 1,
    Rendering        = 1 << 2,
    ViewLifecycle    = 1 << 3,
    ViewAnimation    = 1 << 4,
    ViewMeasure      = 1 << 5,
    ViewArrange      = 1 << 6,
    ViewRendering    = 1 << 7,
    ViewState        = 1 << 8,
}
