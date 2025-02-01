namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public enum NSLayoutAttribute : int
    {
        Left = 1,
        Right = 2,
        Top = 3,
        Bottom = 4,
        Leading = 5,
        Trailing = 6,
        Width = 7,
        Height = 8,
        CenterX = 9,
        CenterY = 10,
        LastBaseline = 11,
        FirstBaseline = 12,
        LeftMargin = 13,
        RightMarting = 14,
        TopMargin = 15,
        BottomMargin = 16,
        LeadingMargin = 17,
        TrailingMargin = 18,
        CenterXWithinMargins = 19,
        CenterYWithinMargins = 20,
        NotAnAttribute = 0
    }
}