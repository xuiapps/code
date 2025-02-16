using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Runtime.IOS;
using static Xui.Runtime.IOS.UIKit;
using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.Foundation;
using static Xui.Runtime.IOS.CoreText;
using System.Diagnostics;
using Xui.Core.Math2D;
using System;

namespace Xui.Runtime.IOS.Actual;

public class IOSDrawingContext : IContext
{
    private nint cgContextRef;

    private Paint fill;

    private Paint stroke;

    private nint nsFont;

    private AffineTransform? baseTransform;

    public IOSDrawingContext()
    {
    }

    NFloat IPenContext.GlobalAlpha { set => CGContextRef.CGContextSetAlpha(this.cgContextRef, value); }

    LineCap IPenContext.LineCap { set => CGContextRef.CGContextSetLineCap(this.cgContextRef, (CGLineCap)value); }
    LineJoin IPenContext.LineJoin { set => CGContextRef.CGContextSetLineJoin(this.cgContextRef, (CGLineJoin)value); }
    NFloat IPenContext.LineWidth { set => CGContextRef.CGContextSetLineWidth(this.cgContextRef, value); }
    NFloat IPenContext.MiterLimit { set => CGContextRef.CGContextSetMiterLimit(this.cgContextRef, value); }

    // TODO: Handle change and verify
    public NFloat LineDashOffset { get; set; }

    public TextAlign TextAlign { get; set; }

    public TextBaseline TextBaseline { get; set; }

    public IOSDrawingContext Bind()
    {
        // this.cgContextRef = NSGraphicsContext.CurrentCGContext.Self;
        this.cgContextRef = Xui.Runtime.IOS.UIKit.UIGraphicsGetCurrentContext();
        this.fill.Reset();
        this.stroke.Reset();
        this.baseTransform = null;

        return this;
    }

    void IStateContext.Restore() =>
        CGContextRef.CGContextRestoreGState(this.cgContextRef);

    void IStateContext.Save() =>
        CGContextRef.CGContextSaveGState(this.cgContextRef);

    void IPenContext.SetLineDash(ReadOnlySpan<NFloat> segments)
    {
        if (segments.Length == 0)
        {
            CGContextRef.CGContextSetLineDash(this.cgContextRef, this.LineDashOffset, nint.Zero, nint.Zero);
        }
        else if (segments.Length % 2 == 0)
        {
            CGContextRef.CGContextSetLineDash(this.cgContextRef, this.LineDashOffset, ref MemoryMarshal.GetReference(segments), segments.Length);
        }
        else
        {
            Span<NFloat> mirrored = stackalloc NFloat[segments.Length * 2];
            for(var i = 0; i < segments.Length; i++)
            {
                mirrored[i] = mirrored[i * 2] = segments[i];
            }
            CGContextRef.CGContextSetLineDash(this.cgContextRef, this.LineDashOffset, ref MemoryMarshal.GetReference(mirrored), mirrored.Length);
        }
    }

    void IPathDrawingContext.Arc(Point center, NFloat radius, NFloat startAngle, NFloat endAngle, Winding winding) =>
        CGContextRef.CGContextAddArc(this.cgContextRef, center.X, center.Y, radius, startAngle, endAngle, (int)winding);

    void IPathDrawingContext.ArcTo(Point cp1, Point cp2, NFloat radius) =>
        CGContextRef.CGContextAddArcToPoint(this.cgContextRef, cp1.X, cp1.Y, cp2.X, cp2.Y, radius);

    void IPathDrawingContext.BeginPath() =>
        CGContextRef.CGContextBeginPath(this.cgContextRef);

    void IPathDrawingContext.Clip() =>
        CGContextRef.CGContextClip(this.cgContextRef);

    void IPathDrawingContext.ClosePath() =>
        CGContextRef.CGContextClosePath(this.cgContextRef);

    void IPathDrawingContext.CurveTo(Point cp1, Point to) =>
        CGContextRef.CGContextAddQuadCurveToPoint(this.cgContextRef, cp1.X, cp1.Y, to.X, to.Y);

    void IPathDrawingContext.CurveTo(Point cp1, Point cp2, Point to) =>
        CGContextRef.CGContextAddCurveToPoint(this.cgContextRef, cp1.X, cp1.Y, cp2.X, cp2.Y, to.X, to.Y);

    void IDisposable.Dispose()
    {
        if (this.cgContextRef != nint.Zero)
        {
            this.cgContextRef = nint.Zero;
        }

        this.fill.Reset();
        this.stroke.Reset();

        this.baseTransform = null;

        if (this.nsFont != 0)
        {
            CFRelease(this.nsFont);
            this.nsFont = 0;
        }
    }

    void IPathDrawingContext.Ellipse(Point center, NFloat radiusX, NFloat radiusY, NFloat rotation, NFloat startAngle, NFloat endAngle, Winding winding)
    {
        CGContextRef.CGContextSaveGState(this.cgContextRef);
        CGContextRef.CGContextTranslateCTM(this.cgContextRef, center.X, center.Y);
        CGContextRef.CGContextRotateCTM(this.cgContextRef, rotation);
        CGContextRef.CGContextScaleCTM(this.cgContextRef, radiusX, radiusY);
        CGContextRef.CGContextAddArc(this.cgContextRef, 0, 0, 1, endAngle, startAngle, (int)winding);
        CGContextRef.CGContextRestoreGState(this.cgContextRef);
    }

    void IPathDrawingContext.Fill(FillRule fillRule)
    {
        if (this.fill.style == PaintStyle.SolidColor)
        {
            if (fillRule == FillRule.EvenOdd)
            {
                CGContextRef.CGContextEOFillPath(this.cgContextRef);
            }
            else
            {
                CGContextRef.CGContextFillPath(this.cgContextRef);
            }
        }
        else if (this.fill.style == PaintStyle.LinearGradient)
        {
            CGContextRef.CGContextSaveGState(this.cgContextRef);
            if (fillRule == FillRule.EvenOdd)
            {
                CGContextRef.CGContextEOClip(this.cgContextRef);
            }
            else
            {
                CGContextRef.CGContextClip(this.cgContextRef);
            }

            CGContextRef.CGContextDrawLinearGradient(
                this.cgContextRef,
                this.fill.cgGradientRef,
                this.fill.startPoint,
                this.fill.endPoint);
            CGContextRef.CGContextRestoreGState(this.cgContextRef);
        }
        else
        {
            CGContextRef.CGContextSaveGState(this.cgContextRef);
            if (fillRule == FillRule.EvenOdd)
            {
                CGContextRef.CGContextEOClip(this.cgContextRef);
            }
            else
            {
                CGContextRef.CGContextClip(this.cgContextRef);
            }

            CGContextRef.CGContextDrawRadialGradient(
                this.cgContextRef,
                this.fill.cgGradientRef,
                this.fill.startPoint,
                this.fill.startRadius,
                this.fill.endPoint,
                this.fill.endRadius);
            CGContextRef.CGContextRestoreGState(this.cgContextRef);
        }
    }

    void IRectDrawingContext.FillRect(Rect rect)
    {
        if (this.fill.style == PaintStyle.SolidColor)
        {
            CGContextRef.CGContextFillRect(this.cgContextRef, rect);
        }
        else if (this.fill.style == PaintStyle.LinearGradient)
        {
            CGContextRef.CGContextSaveGState(this.cgContextRef);
            CGContextRef.CGContextClipToRect(this.cgContextRef, rect);
            CGContextRef.CGContextDrawLinearGradient(
                this.cgContextRef,
                this.stroke.cgGradientRef,
                this.stroke.startPoint,
                this.stroke.endPoint);
            CGContextRef.CGContextRestoreGState(this.cgContextRef);
        }
        else if (this.fill.style == PaintStyle.RadialGradient)
        {
            CGContextRef.CGContextSaveGState(this.cgContextRef);
            CGContextRef.CGContextClipToRect(this.cgContextRef, rect);
            CGContextRef.CGContextDrawRadialGradient(
                this.cgContextRef,
                this.stroke.cgGradientRef,
                this.stroke.startPoint,
                this.stroke.startRadius,
                this.stroke.endPoint,
                this.stroke.endRadius);
            CGContextRef.CGContextRestoreGState(this.cgContextRef);
        }
    }

    void IPathDrawingContext.LineTo(Point to) =>
        CGContextRef.CGContextAddLineToPoint(this.cgContextRef, to.X, to.Y);

    void IPathDrawingContext.MoveTo(Point to) =>
        CGContextRef.CGContextMoveToPoint(this.cgContextRef, to.X, to.Y);

    void IPathDrawingContext.Rect(Rect rect) =>
        CGContextRef.CGContextAddRect(this.cgContextRef, rect);

    void ITransformContext.Rotate(NFloat angle)
    {
        this.baseTransform ??= CGContextRef.CGContextGetCTM(this.cgContextRef);
        CGContextRef.CGContextRotateCTM(this.cgContextRef, angle);
    }

    void IPathDrawingContext.RoundRect(Rect rect, NFloat radius)
    {
        using var cgPathRef = CGPathRef.CreateWithRoundedRect(rect, radius);
        CGContextRef.CGContextAddPath(this.cgContextRef, cgPathRef);
    }

    void IPathDrawingContext.RoundRect(Rect rect, CornerRadius radius)
    {
        var context = (IContext)this;

        // TODO: Clap radius to available space in rect

        if (radius.TopLeft == 0)
        {
            context.MoveTo(rect.TopLeft);
        }
        else
        {
            context.MoveTo(new (rect.X, rect.Y + radius.TopLeft));
            context.ArcTo(rect.TopLeft, rect.TopRight, radius.TopLeft);
        }

        if (radius.TopRight == 0)
        {
            context.LineTo(rect.TopRight);
        }
        else
        {
            context.ArcTo(rect.TopRight, rect.BottomRight, radius.TopRight);
        }

        if (radius.BottomRight == 0)
        {
            context.LineTo(rect.BottomRight);
        }
        else
        {
            context.ArcTo(rect.BottomRight, rect.BottomLeft, radius.BottomRight);
        }

        if (radius.BottomLeft == 0)
        {
            context.LineTo(rect.TopLeft);
        }
        else
        {
            context.ArcTo(rect.BottomLeft, rect.TopLeft, radius.BottomLeft);
        }

        context.ClosePath();
    }

    void ITransformContext.Scale(Vector vector)
    {
        this.baseTransform ??= CGContextRef.CGContextGetCTM(this.cgContextRef);
        CGContextRef.CGContextScaleCTM(this.cgContextRef, vector.X, vector.Y);
    }

    void IPenContext.SetFill(Color color)
    {
        this.fill.Set(color);
        CGContextRef.CGContextSetRGBFillColor(this.cgContextRef, color.Red, color.Green, color.Blue, color.Alpha);
    }

    void IPenContext.SetFill(LinearGradient linearGradient) =>
        this.fill.Set(linearGradient);

    void IPenContext.SetFill(RadialGradient radialGradient) =>
        this.fill.Set(radialGradient);

    void IPenContext.SetStroke(Color color)
    {
        this.stroke.Set(color);
        CGContextRef.CGContextSetRGBStrokeColor(this.cgContextRef, color.Red, color.Green, color.Blue, color.Alpha);
    }

    void IPenContext.SetStroke(LinearGradient linearGradient) =>
        this.stroke.Set(linearGradient);

    void IPenContext.SetStroke(RadialGradient radialGradient) =>
        this.stroke.Set(radialGradient);

    void ITransformContext.SetTransform(AffineTransform transform)
    {
        this.baseTransform ??= CGContextRef.CGContextGetCTM(this.cgContextRef);
        var invert = CGContextRef.CGContextGetCTM(this.cgContextRef).Invert;
        CGContextRef.CGContextConcatCTM(this.cgContextRef, invert);
        CGContextRef.CGContextConcatCTM(this.cgContextRef, this.baseTransform.Value);
        CGContextRef.CGContextConcatCTM(this.cgContextRef, transform);
    }

    void IPathDrawingContext.Stroke()
    {
        if (this.stroke.style == PaintStyle.SolidColor)
        {
            CGContextRef.CGContextStrokePath(this.cgContextRef);
        }
        else if (this.stroke.style == PaintStyle.LinearGradient)
        {
            CGContextRef.CGContextSaveGState(this.cgContextRef);
            CGContextRef.CGContextReplacePathWithStrokedPath(this.cgContextRef);
            CGContextRef.CGContextClip(this.cgContextRef);
            CGContextRef.CGContextDrawLinearGradient(
                this.cgContextRef,
                this.stroke.cgGradientRef,
                this.stroke.startPoint,
                this.stroke.endPoint);
            CGContextRef.CGContextRestoreGState(this.cgContextRef);
        }
        else if (this.stroke.style == PaintStyle.RadialGradient)
        {
            CGContextRef.CGContextSaveGState(this.cgContextRef);
            CGContextRef.CGContextReplacePathWithStrokedPath(this.cgContextRef);
            CGContextRef.CGContextClip(this.cgContextRef);
            CGContextRef.CGContextDrawRadialGradient(
                this.cgContextRef,
                this.stroke.cgGradientRef,
                this.stroke.startPoint,
                this.stroke.startRadius,
                this.stroke.endPoint,
                this.stroke.endRadius);
            CGContextRef.CGContextRestoreGState(this.cgContextRef);
        }
    }

    void IRectDrawingContext.StrokeRect(Rect rect)
    {
        if (this.stroke.style == PaintStyle.SolidColor)
        {
            CGContextRef.CGContextStrokeRect(this.cgContextRef, rect);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    void ITransformContext.Transform(AffineTransform matrix)
    {
        this.baseTransform ??= CGContextRef.CGContextGetCTM(this.cgContextRef);
        CGContextRef.CGContextConcatCTM(this.cgContextRef, matrix);
    }

    void ITransformContext.Translate(Vector vector)
    {
        this.baseTransform ??= CGContextRef.CGContextGetCTM(this.cgContextRef);
        CGContextRef.CGContextTranslateCTM(this.cgContextRef, vector.X, vector.Y);
    }

    void ITextDrawingContext.FillText(string text, Point pos)
    {
        using var nsStringRef = new CFStringRef(text);
        using var attributes = new CFMutableDictionaryRef();
        using var foreground = new UIColorRef(this.fill.color);
        attributes.SetValue(NSAttributedString.Key.ForegroundColor, foreground);
        if (this.nsFont != 0)
        {
            attributes.SetValue(NSAttributedString.Key.Font, this.nsFont);
        }

        Vector offset = (0, 0);
        switch(this.TextAlign)
        {
            case TextAlign.Left:
                break;
            case TextAlign.Center:
                offset.X +=
                    ObjC.objc_msgSend_retCGSize(nsStringRef, NSString.SizeWithAttributesSel, attributes)
                    .Width * -0.5f;;
                break;
            case TextAlign.Right:
                offset.X += ObjC.objc_msgSend_retCGSize(nsStringRef, NSString.SizeWithAttributesSel, attributes)
                    .Width * -1.0f;
                break;
            case TextAlign.Start:
            case TextAlign.End:
                throw new NotSupportedException();
        }

        NSString.objc_msgSend(nsStringRef, NSString.DrawAtPointWithAttributesSel, pos + offset, attributes);
    }

    Vector ITextDrawingContext.MeasureText(string text)
    {
        using var nsStringRef = new CFStringRef(text);
        using var attributes = new CFMutableDictionaryRef();
        using var foreground = new UIColorRef(this.fill.color);
        attributes.SetValue(NSAttributedString.Key.ForegroundColor, foreground);
        if (this.nsFont != 0)
        {
            attributes.SetValue(NSAttributedString.Key.Font, this.nsFont);
        }

        return ObjC.objc_msgSend_retCGSize(nsStringRef, NSString.SizeWithAttributesSel, attributes);
    }

    void ITextDrawingContext.SetFont(Font font)
    {
        if (this.nsFont != 0)
        {
            CFRelease(this.nsFont);
            this.nsFont = 0;
        }

        try
        {
            using var attributes = new CFMutableDictionaryRef();

            if (font.FontFamily.Length >= 1)
            {
                using var fontFamilyNameRef = new CFStringRef(font.FontFamily[0]);
                attributes.SetValue(CTFontDescriptor.FontAttributes.FamilyName, fontFamilyNameRef);

                if (font.FontFamily.Length > 1)
                {
                    using var nsCascadingFontArray = new CFMutableArrayRef();
                    foreach(var f in font.FontFamily)
                    {
                        using var desc = new CTFontDescriptorRef(f);
                        nsCascadingFontArray.Add(desc);
                    }
                    // TODO: Here the dictionary does not retain the array and the array is disposed...
                    attributes.SetValue(CTFontDescriptor.FontAttributes.CascadeList, nsCascadingFontArray);
                }
            }

            using var fontSizeRef = new CFNumberRef(font.FontSize);
            attributes.SetValue(CTFontDescriptor.FontAttributes.Size, fontSizeRef);

            using var fontTraits = new CFMutableDictionaryRef();

            // 400 is regular, 700 is bold set through symbolic traits
            if (font.FontWeight != 400 && font.FontWeight != 700)
            {
                NFloat webWeightToCTWeight = MapWebWeightToCTWeight(font.FontWeight);
                using var fontWeight = new CFNumberRef(webWeightToCTWeight);
                fontTraits.SetValue(CTFontDescriptor.FontTrait.Weight, fontWeight);
            }

            if (font.FontStyle.IsItalic || font.FontWeight == 700)
            {
                int symTrait = 0;
                if (font.FontStyle.IsItalic)
                {
                    symTrait |= (int)CTFontDescriptor.SymbolicTrait.Italic;
                }

                if (font.FontWeight == 700)
                {
                    symTrait |= (int)CTFontDescriptor.SymbolicTrait.Bold;
                }

                using var symTraitRef = new CFNumberRef(symTrait);
                fontTraits.SetValue(CTFontDescriptor.FontTrait.Symbolic, symTraitRef);
            }

            if (font.FontStyle.IsOblique)
            {
                var slant = new CFNumberRef(NFloat.Clamp(font.FontStyle.ObliqueAngle / 90f, -1f, 1f));
                fontTraits.SetValue(CTFontDescriptor.FontTrait.Slant, slant);
            }

            // fontTraits.SetValue(CTFontDescriptor.FontTrait.Width, /* oblique */);

            attributes.SetValue(CTFontDescriptor.FontAttributes.Traits, fontTraits);

            using var descriptor = CTFontDescriptorRef.WithAttributes(attributes);
            nint options = 0;

            // Important - do not dispose, no "using", we will keep it manually
            var ctFont = new CTFontRef(descriptor, options);
            this.nsFont = ctFont;
        }
        catch(Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    private struct Paint
    {
        public PaintStyle style;
        public Color color;
        public nint cgGradientRef;
        public Point startPoint;
        public Point endPoint;
        public NFloat startRadius;
        public NFloat endRadius;

        public void Set(Color color)
        {
            this.Reset(PaintStyle.SolidColor);
            this.color = color;
        }

        public void Set(LinearGradient linearGradient)
        {
            this.Reset(PaintStyle.LinearGradient);
            this.cgGradientRef = new CGGradientRef(linearGradient.GradientStops);
            this.startPoint = linearGradient.StartPoint;
            this.endPoint = linearGradient.EndPoint;
        }

        public void Set(RadialGradient radialGradient)
        {
            this.Reset(PaintStyle.RadialGradient);
            this.cgGradientRef = new CGGradientRef(radialGradient.GradientStops);
            this.startPoint = radialGradient.StartCenter;
            this.startRadius = radialGradient.StartRadius;
            this.endPoint = radialGradient.EndCenter;
            this.endRadius = radialGradient.EndRadius;
        }

        public void Reset(PaintStyle style = PaintStyle.SolidColor)
        {
            if (this.cgGradientRef != 0)
            {
                CGGradientRef.CGGradientRelease(cgGradientRef);
                this.cgGradientRef = 0;
            }
            this.style = style;
        }
    }

    /// <summary>
    /// <code>
    /// NSFontWeightUltraLight: -0.800000 - 100 Thin (Hairline)
    /// NSFontWeightThin: -0.600000       - 200 Extra Light (Ultra Light)
    /// NSFontWeightLight: -0.400000      - 300 Light
    /// NSFontWeightRegular: 0.000000     - 400 Normal (Regular)
    /// NSFontWeightMedium: 0.230000      - 500 Medium
    /// NSFontWeightSemibold: 0.300000    - 600 Semi Bold (Demi Bold)
    /// NSFontWeightBold: 0.400000        - 700 Bold
    ///                                   - 800 Extra Bold (Ultra Bold)
    /// NSFontWeightHeavy: 0.560000       - 900 Black (Heavy)
    ///                                   - 950 Extra Black (Ultra Black)
    /// </code>
    /// </summary>
    /// <param name="webWeight"></param>
    /// <returns></returns>
    private NFloat MapWebWeightToCTWeight(NFloat webWeight)
    {
        if (webWeight == 400) return 0;
        webWeight = NFloat.Clamp(webWeight, 1, 1000);
        if (webWeight < 100f) return NFloat.Lerp(-1, -0.8f, (webWeight - 1f) / 100f);
        if (webWeight <= 300f) return NFloat.Lerp(-0.8f, -0.4f, (webWeight - 100f) / 200f);
        if (webWeight <= 400f) return NFloat.Lerp(-0.4f, 0f, (webWeight - 300f) / 100f);
        if (webWeight <= 500f) return NFloat.Lerp(0f, 0.23f, (webWeight - 400f) / 100f);
        if (webWeight <= 600f) return NFloat.Lerp(0.23f, 0.3f, (webWeight - 500f) / 100f);
        if (webWeight <= 700f) return NFloat.Lerp(0.3f, 0.4f, (webWeight - 600f) / 100f);
        if (webWeight <= 900f) return NFloat.Lerp(0.4f, 0.56f, (webWeight - 700f) / 200f);
        return NFloat.Lerp(0.56f, 1f, (webWeight - 900f) / 100f);
    }

    public void SetLineDash(ReadOnlySpan<NFloat> segments)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}