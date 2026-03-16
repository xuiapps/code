namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// Represents a single CSS grid track sizing value such as a length, <c>fr</c> unit,
    /// <c>auto</c>, <c>min-content</c>, <c>max-content</c>, <c>minmax()</c>, or <c>fit-content()</c>.
    /// </summary>
    public readonly struct TrackSize
    {
        /// <summary>The kind of track sizing function.</summary>
        public TrackSizeKind Kind { get; }

        private readonly nfloat value;
        private readonly nfloat minValue;
        private readonly nfloat maxValue;

        private TrackSize(TrackSizeKind kind, nfloat value = default, nfloat minValue = default, nfloat maxValue = default)
        {
            Kind = kind;
            this.value = value;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        /// <summary>Pixel length for the track.</summary>
        public nfloat Value => value;

        /// <summary>Minimum value for <see cref="TrackSizeKind.MinMax"/> tracks.</summary>
        public nfloat MinValue => minValue;

        /// <summary>Maximum value for <see cref="TrackSizeKind.MinMax"/> and <see cref="TrackSizeKind.FitContent"/> tracks.</summary>
        public nfloat MaxValue => maxValue;

        /// <summary>The track sizes to content automatically. Equivalent to CSS <c>auto</c>.</summary>
        public static readonly TrackSize Auto = new(TrackSizeKind.Auto);

        /// <summary>The track is sized to the smallest size that fits its content without overflow. Equivalent to CSS <c>min-content</c>.</summary>
        public static readonly TrackSize MinContent = new(TrackSizeKind.MinContent);

        /// <summary>The track is sized to the largest max-content contribution of its items. Equivalent to CSS <c>max-content</c>.</summary>
        public static readonly TrackSize MaxContent = new(TrackSizeKind.MaxContent);

        /// <summary>Creates a track of an exact pixel length.</summary>
        public static TrackSize Px(nfloat px) => new(TrackSizeKind.Length, px);

        /// <summary>Creates a fractional track. <c>Fr(1)</c> = CSS <c>1fr</c>.</summary>
        public static TrackSize Fr(nfloat fr) => new(TrackSizeKind.Fr, fr);

        /// <summary>Creates a <c>minmax(min, max)</c> track with pixel min and max values.</summary>
        public static TrackSize MinMax(nfloat min, nfloat max) => new(TrackSizeKind.MinMax, minValue: min, maxValue: max);

        /// <summary>Creates a <c>fit-content(max)</c> track clamped to the given pixel maximum.</summary>
        public static TrackSize FitContent(nfloat max) => new(TrackSizeKind.FitContent, maxValue: max);

        /// <summary>Implicitly converts a pixel value to a <see cref="TrackSizeKind.Length"/> track.</summary>
        public static implicit operator TrackSize(nfloat px) => Px(px);

        /// <summary>Implicitly converts a pixel value to a <see cref="TrackSizeKind.Length"/> track.</summary>
        public static implicit operator TrackSize(float px) => Px(px);

        /// <summary>Implicitly converts a pixel value to a <see cref="TrackSizeKind.Length"/> track.</summary>
        public static implicit operator TrackSize(double px) => Px((nfloat)px);

        /// <summary>Implicitly converts a pixel value to a <see cref="TrackSizeKind.Length"/> track.</summary>
        public static implicit operator TrackSize(int px) => Px(px);

        /// <inheritdoc/>
        public override string ToString() => Kind switch
        {
            TrackSizeKind.Auto       => "auto",
            TrackSizeKind.MinContent => "min-content",
            TrackSizeKind.MaxContent => "max-content",
            TrackSizeKind.Length     => $"{value}px",
            TrackSizeKind.Fr         => $"{value}fr",
            TrackSizeKind.MinMax     => $"minmax({minValue}px, {maxValue}px)",
            TrackSizeKind.FitContent => $"fit-content({maxValue}px)",
            _                        => Kind.ToString(),
        };
    }

    /// <summary>Identifies the kind of sizing function a <see cref="TrackSize"/> represents.</summary>
    public enum TrackSizeKind : byte
    {
        /// <summary>Sized to available space (<c>auto</c>).</summary>
        Auto = 0,

        /// <summary>Sized to smallest content contribution (<c>min-content</c>).</summary>
        MinContent = 1,

        /// <summary>Sized to largest content contribution (<c>max-content</c>).</summary>
        MaxContent = 2,

        /// <summary>Fixed pixel length.</summary>
        Length = 3,

        /// <summary>Flexible fractional unit (<c>fr</c>).</summary>
        Fr = 4,

        /// <summary>Clamped between a min and a max (<c>minmax()</c>).</summary>
        MinMax = 5,

        /// <summary>Like <see cref="MaxContent"/> but clamped to a maximum (<c>fit-content()</c>).</summary>
        FitContent = 6,
    }
}
