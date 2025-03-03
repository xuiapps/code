using Xui.Core.Math2D;

namespace Xui.Core.UI.Layout;

public struct Constraint
{
    public Size Size;

    public SizeTo HorizontalSize;
    public nfloat HorizontalGuide;
    public HorizontalAlign HorizontalAlign;

    public SizeTo VerticalSize;
    public nfloat VerticalGuide;
    public VerticalAlign VerticalAlign;

    public bool IsSizeToParent(out Rect rect)
    {
        if (this.HorizontalSize == SizeTo.Parent &&
            this.VerticalSize == SizeTo.Parent &&
            nfloat.IsFinite(this.Size.Width) &&
            nfloat.IsFinite(this.Size.Height))
        {
            rect = new Rect(
                x: this.HorizontalGuide + this.Size.Width * -0.5f * (int)this.HorizontalAlign,
                y: this.VerticalGuide + this.Size.Height * -0.5f * (int)this.VerticalAlign,
                width: this.Size.Width,
                height: this.Size.Height
            );
            return true;
        }
        else
        {
            rect = default;
            return false;
        }
    }

    public Rect AlignContent(Size content)
    {
        Rect rect;
        if (this.HorizontalSize == SizeTo.Parent && nfloat.IsFinite(this.Size.Width))
        {
            rect.Width = this.Size.Width;
        }
        else
        {
            rect.Width = content.Width;
        }

        if (this.VerticalSize == SizeTo.Parent && nfloat.IsFinite(this.Size.Height))
        {
            rect.Height = this.Size.Height;
        }
        else
        {
            rect.Height = content.Height;
        }

        rect.X = this.HorizontalGuide + rect.Width * -0.5f * (int)this.HorizontalAlign;
        rect.Y = this.VerticalGuide + rect.Height * -0.5f * (int)this.VerticalAlign;
        return rect;
    }

    public static Constraint AlignToParent(Rect rect, HorizontalAlign horizontalAlign = HorizontalAlign.Left, VerticalAlign verticalAlign = VerticalAlign.Top) =>
        new Constraint()
        {
            HorizontalGuide = rect.X + 0.5f * (int)horizontalAlign * rect.Width,
            HorizontalAlign = horizontalAlign,
            HorizontalSize = SizeTo.Parent,
            VerticalGuide = rect.Y + 0.5f * (int)verticalAlign * rect.Height,
            VerticalAlign = verticalAlign,
            VerticalSize = SizeTo.Parent,
            Size = rect.Size
        };
}