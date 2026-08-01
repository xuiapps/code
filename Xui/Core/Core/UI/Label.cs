using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI
{
    /// <summary>
    /// A view that displays a single line of styled text.
    /// </summary>
    public class Label : View
    {
        private string text = "";
        private Color textColor = Colors.Black;
        private Font font = new(15, "Inter", lineHeight: nfloat.NaN);

        /// <summary>
        /// Gets or sets the text content displayed by the label.
        /// </summary>
        public string Text
        {
            get => text;
            set => this.SetViewProperty(ref text, value);
        }

        /// <summary>
        /// Gets or sets the color used to fill the text.
        /// </summary>
        public Color TextColor
        {
            get => textColor;
            set => this.SetViewRenderProperty(ref textColor, value);
        }

        /// <summary>Gets or sets the complete font used for text measurement and rendering.</summary>
        public Font Font
        {
            get => font;
            set => this.SetViewProperty(ref font, value);
        }

        /// <summary>Gets or sets the font family used for rendering the text.</summary>
        public string FontFamily
        {
            get => font.FontFamily;
            set => SetFont(new Font(font.FontSize, value, font.FontWeight, font.FontStyle, font.FontStretch, font.LineHeight));
        }

        /// <summary>
        /// Gets or sets the font size in points.
        /// </summary>
        public nfloat FontSize
        {
            get => font.FontSize;
            set => SetFont(new Font(value, font.FontFamily, font.FontWeight, font.FontStyle, font.FontStretch, font.LineHeight));
        }

        /// <summary>
        /// Gets or sets the font style (e.g., normal, italic, oblique).
        /// </summary>
        public FontStyle FontStyle
        {
            get => font.FontStyle;
            set => SetFont(new Font(font.FontSize, font.FontFamily, font.FontWeight, value, font.FontStretch, font.LineHeight));
        }

        /// <summary>
        /// Gets or sets the font weight (e.g., normal, bold, numeric weight).
        /// </summary>
        public FontWeight FontWeight
        {
            get => font.FontWeight;
            set => SetFont(new Font(font.FontSize, font.FontFamily, value, font.FontStyle, font.FontStretch, font.LineHeight));
        }

        /// <summary>
        /// Gets or sets the font stretch (e.g., condensed, semi-expanded etc.).
        /// </summary>
        public FontStretch FontStretch
        {
            get => font.FontStretch;
            set => SetFont(new Font(font.FontSize, font.FontFamily, font.FontWeight, font.FontStyle, value, font.LineHeight));
        }

        /// <summary>
        /// Gets or sets the line height of the text, in user units.
        /// </summary>
        /// <remarks>
        /// If set to a numeric value, this value overrides the default line height computation from the font metrics.
        /// If set to <see cref="nfloat.NaN"/>, the line height will be automatically computed based on the font's ascender,
        /// descender, and line gap, or fallback to a platform-specific multiplier of the <see cref="Font.FontSize"/> (typically 1.2×).
        /// </remarks>
        public nfloat LineHeight
        {
            get => font.LineHeight;
            set => SetFont(new Font(font.FontSize, font.FontFamily, font.FontWeight, font.FontStyle, font.FontStretch, value));
        }

        /// <inheritdoc/>
        protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
        {
            context.SetFont(font);
            var textSize = context.MeasureText(this.Text);
            return textSize.Size;
        }

        /// <inheritdoc/>
        protected override void RenderCore(IContext context)
        {
            context.SetFont(font);
            context.TextBaseline = TextBaseline.Top;
            context.TextAlign = TextAlign.Left;
            context.SetFill(this.TextColor);
            context.FillText(this.Text, this.Frame.TopLeft);
            base.RenderCore(context);
        }

        private void SetFont(Font value) => this.SetViewProperty(ref font, value);
    }
}
