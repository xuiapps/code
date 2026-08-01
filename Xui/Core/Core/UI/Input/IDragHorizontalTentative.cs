namespace Xui.Core.UI.Input
{
    /// <summary>
    /// A horizontal-axis drag that is willing to yield capture to a vertical-axis
    /// ancestor whose drag intent appears stronger. Used by horizontal scroll views
    /// nested inside vertical ones: the inner scrolls horizontally from the first
    /// pointer move, but if the user's motion is dominantly vertical when crossing
    /// the recognition threshold, the outer scroll view steals capture.
    ///
    /// <para>
    /// The capturing view is expected to "promote" itself by re-capturing with a
    /// plain <see cref="IDrag"/> (e.g. <see cref="PointerGestures.Drag"/>) once
    /// horizontal motion has clearly committed to its axis (typical threshold:
    /// 20pt of horizontal travel). After promotion, vertical ancestors can no
    /// longer steal capture for this pointer.
    /// </para>
    /// </summary>
    public interface IDragHorizontalTentative : IDrag
    {
    }
}
