namespace Xui.Core.UI;

/// <summary>Defines how an axis participates in a layout pass.</summary>
public enum LayoutSizeMode : byte
{
    /// <summary>The view must exactly match the available size.</summary>
    Exact,
    /// <summary>The view may size to content without exceeding the available size.</summary>
    AtMost,
}
