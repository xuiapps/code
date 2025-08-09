using Xui.Apps.TestApp.Examples;
using Xui.Core.Abstract;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages;

public class SdkNavigation : ViewCollection
{
    private View? content;

    private SdkHomePage homePage;

    private View? currentExample;

    private View? Content
    {
        get => this.content;
        set
        {
            if (value == this.content) return;
            if (this.content is not null) this.Remove(this.content);
            if (value is not null) this.Add(value);
            this.content = value;

            if (Window.OpenWindows.Count > 0)
            {
                Window.OpenWindows[0].Invalidate();
            }
        }
    }

    public SdkNavigation()
    {
        this.homePage = new SdkHomePage();
        this.Content = this.homePage;
    }

    public void PopExample()
    {
        this.currentExample = null;
        this.Content = this.homePage;
    }

    public void PushExamplePage<TPage>() where TPage : Example, new()
    {
        this.currentExample = new SdkExamplePage<TPage>();
        this.Content = this.currentExample;
    }

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        if (this.TryFindParent<RootView>(out var rootView))
        {
            availableBorderEdgeSize = rootView.Window.SafeArea.Size;
        }

        return this.Content?.Measure(availableBorderEdgeSize, context) ?? (0, 0);
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        if (this.TryFindParent<RootView>(out var rootView))
        {
            rect = rootView.Window.SafeArea;
        }

        this.Content?.Arrange(rect, context);
    }
}
