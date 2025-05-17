using System;
using System.Collections.Generic;
using Xui.Core.Canvas;

namespace Xui.Runtime.Software.Font;

public readonly struct FontFace
{
    /// <summary>
    /// A struct comparer for <see cref="FontFace"/> that avoids boxing and uses case-insensitive family comparison.
    /// </summary>
    public static readonly IEqualityComparer<FontFace> Comparer = new FontFaceComparer();

    public string Family { get; }
    public FontWeight Weight { get; }
    public FontStyle Style { get; }
    public FontStretch Stretch { get; }

    public FontFace(
        string family,
        FontWeight weight,
        FontStyle style,
        FontStretch stretch)
    {
        Family = family;
        Weight = weight;
        Style = style;
        Stretch = stretch;
    }

    private readonly struct FontFaceComparer : IEqualityComparer<FontFace>
    {
        public bool Equals(FontFace x, FontFace y)
        {
            return string.Equals(x.Family, y.Family, StringComparison.OrdinalIgnoreCase)
                && x.Weight.Equals(y.Weight)
                && x.Style.Equals(y.Style)
                && x.Stretch.Equals(y.Stretch);
        }

        public int GetHashCode(FontFace obj)
        {
            var hash = new HashCode();
            hash.Add(obj.Family, StringComparer.OrdinalIgnoreCase);
            hash.Add(obj.Weight);
            hash.Add(obj.Style);
            hash.Add(obj.Stretch);
            return hash.ToHashCode();
        }
    }
}
