using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI
{
    /// <summary>
    /// A view that draws a background, border, and padding around a single child content view.
    /// </summary>
    public class Border : View
    {
        /// <summary>
        /// Gets or sets the content view displayed inside the border.
        /// </summary>
        public View? Content { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border on each side.
        /// </summary>
        public Frame BorderThickness { get; set; } = Math2D.Frame.Zero;

        /// <summary>
        /// Gets or sets the corner radius used to round the corners of the border and background.
        /// </summary>
        public CornerRadius CornerRadius { get; set; } = 0;

        /// <summary>
        /// Gets or sets the background color inside the border.
        /// </summary>
        public Color BackgroundColor { get; set; } = Colors.Transparent;

        /// <summary>
        /// Gets or sets the color of the border stroke.
        /// </summary>
        public Color BorderColor { get; set; } = Colors.Transparent;

        /// <summary>
        /// Gets or sets the padding between the border and the content.
        /// </summary>
        public Frame Padding { get; set; } = Math2D.Frame.Zero;

        /// <inheritdoc/>
        protected override Size MeasureCore(Size constraints, IMeasureContext context)
        {
            Size contentSize = Content?.Measure(Size.Max(Size.Empty, constraints - this.BorderThickness - this.Padding), context) ?? Size.Empty;
            return contentSize + this.BorderThickness + this.Padding;
        }

        /// <inheritdoc/>
        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            this.Content?.Arrange(rect - this.Padding - this.BorderThickness, context);
            base.ArrangeCore(rect, context);
        }

        /// <inheritdoc/>
        protected override void RenderCore(IContext context)
        {
            // Draw background
            if (!this.BackgroundColor.IsTransparent)
            {
                if (this.CornerRadius.IsZero)
                {
                    context.SetFill(this.BackgroundColor);
                    context.FillRect(this.Frame - this.BorderThickness);
                }
                else if (this.BorderThickness.IsUniform)
                {
                    context.BeginPath();
                    var cornerRadius = CornerRadius.Max(CornerRadius.Zero, this.CornerRadius - this.BorderThickness.Left);
                    context.RoundRect(this.Frame - this.BorderThickness, cornerRadius);
                    context.SetFill(this.BackgroundColor);
                    context.Fill();
                }
                else
                {
                    // TODO: Corners here are elliptical, calculate the shape...
                }
            }

            // Draw border
            if (!this.BorderColor.IsTransparent)
            {
                if (this.BorderThickness.IsZero)
                {
                }
                else if (this.BorderThickness.IsUniform)
                {
                    nfloat halfBorderThickness = this.BorderThickness.Left * (nfloat).5;
                    if (this.CornerRadius.IsZero)
                    {
                        context.SetStroke(this.BorderColor);
                        context.LineWidth = this.BorderThickness.Left;
                        context.SetStroke(this.BorderColor);
                        context.StrokeRect(this.Frame - halfBorderThickness);
                    }
                    else
                    {
                        context.BeginPath();
                        context.RoundRect(this.Frame - halfBorderThickness, this.CornerRadius - halfBorderThickness);
                        context.LineWidth = this.BorderThickness.Left;
                        context.SetStroke(this.BorderColor);
                        context.Stroke();
                    }
                }
                else
                {
                    if (this.CornerRadius.IsZero)
                    {
                        // TODO: Somewhat rectangular
                    }
                    else
                    {
                        // TODO: Outer RoundRect with the this.CornerRadius, inner edges are elliptical, or square...
                    }
                }
            }

            this.Content?.Render(context);
        }
    }
}
