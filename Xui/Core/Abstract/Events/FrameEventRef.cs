namespace Xui.Core.Abstract.Events;

public ref struct FrameEventRef
{
    public TimeSpan Previous;
    public TimeSpan Next;
    public TimeSpan Delta;

    public FrameEventRef(TimeSpan previous, TimeSpan next)
    {
        Previous = previous;
        Next = next;
        this.Delta = next - previous;
    }
}