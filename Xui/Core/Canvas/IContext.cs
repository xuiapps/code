namespace Xui.Core.Canvas;

public interface IContext :
    IStateContext,
    IPenContext,
    IPathDrawingContext,
    IRectDrawingContext,
    ITextDrawingContext,
    IImageDrawingContext,
    ITransformContext,
    IDisposable
{
}