namespace Xui.Core.Abstract.Events;

public ref struct TouchEventRef
{
    public readonly ReadOnlySpan<Touch> Touches;

    public TouchEventRef(ReadOnlySpan<Touch> touches)
    {
        this.Touches = touches;
    }
}