namespace Xui.Core.UI;

/// <summary>Pass selection and per-pass immutable constraints for one view update.</summary>
public readonly struct LayoutUpdate
{
    /// <summary>Initializes a layout update.</summary>
    public LayoutUpdate(LayoutPass pass, MeasureConstraints measure, ArrangeConstraints arrange)
    {
        Pass = pass;
        Measure = measure;
        Arrange = arrange;
    }

    /// <summary>Requested passes.</summary>
    public LayoutPass Pass { get; }
    /// <summary>Measurement constraints.</summary>
    public MeasureConstraints Measure { get; }
    /// <summary>Arrangement constraints.</summary>
    public ArrangeConstraints Arrange { get; }
    /// <summary>Whether animation is requested.</summary>
    public bool IsAnimate => (Pass & LayoutPass.Animate) != 0;
    /// <summary>Whether measurement is requested.</summary>
    public bool IsMeasure => (Pass & LayoutPass.Measure) != 0;
    /// <summary>Whether arrangement is requested.</summary>
    public bool IsArrange => (Pass & LayoutPass.Arrange) != 0;
    /// <summary>Whether rendering is requested.</summary>
    public bool IsRender => (Pass & LayoutPass.Render) != 0;
}
