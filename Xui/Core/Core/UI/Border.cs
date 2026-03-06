using Xui.Core.Math2D;
using Xui.Core.Canvas;
using Xui.Core.UI.Layer;

namespace Xui.Core.UI;

/// <summary>
/// A view that draws a background, border, and padding around a single child content view.
/// Rendering is implemented by <see cref="BorderLayer{TChild}"/> composed with
/// <see cref="ContentLayer"/>; this class exposes the layer fields as named properties
/// and manages the child view's place in the hierarchy.
/// </summary>
public class Border : LayerView<BorderLayer<ContentLayer>>
{
    private View? content;

    /// <summary>
    /// Gets or sets the content view displayed inside the border.
    /// </summary>
    public View? Content
    {
        get => content;
        set
        {
            SetProtectedChild(ref content, value);
            Layer.Child.Child = value;
        }
    }

    /// <summary>
    /// Gets or sets the thickness of the border on each side.
    /// </summary>
    public Frame BorderThickness
    {
        get => Layer.BorderThickness;
        set => Layer.BorderThickness = value;
    }

    /// <summary>
    /// Gets or sets the corner radius used to round the corners of the border and background.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => Layer.CornerRadius;
        set => Layer.CornerRadius = value;
    }

    /// <summary>
    /// Gets or sets the background color inside the border.
    /// </summary>
    public Color BackgroundColor
    {
        get => Layer.BackgroundColor;
        set => Layer.BackgroundColor = value;
    }

    /// <summary>
    /// Gets or sets the color of the border stroke.
    /// </summary>
    public Color BorderColor
    {
        get => Layer.BorderColor;
        set => Layer.BorderColor = value;
    }

    /// <summary>
    /// Gets or sets the padding between the border and the content.
    /// </summary>
    public Frame Padding
    {
        get => Layer.Padding;
        set => Layer.Padding = value;
    }

    /// <inheritdoc/>
    public override int Count => content is null ? 0 : 1;

    /// <inheritdoc/>
    public override View this[int index]
    {
        get
        {
            if (content is not null && index == 0)
                return content;
            throw new IndexOutOfRangeException();
        }
    }
}
