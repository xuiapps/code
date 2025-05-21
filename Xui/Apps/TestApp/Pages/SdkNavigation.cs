using Xui.Apps.TestApp.Examples;
using Xui.Core.Abstract;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages;

public class SdkNavigation : Border
{
    private SdkHomePage homePage;

    private View? currentExample;

    public SdkNavigation()
    {
        this.homePage = new SdkHomePage();
        this.Content = this.homePage;
    }

    public void PopExample()
    {
        this.currentExample = null;
        this.Content = this.homePage;

        Window.OpenWindows[0].Invalidate();
    }

    public void PushExamplePage<TPage>() where TPage : Example, new()
    {
        this.currentExample = new SdkExamplePage<TPage>();
        this.Content = this.currentExample;

        Window.OpenWindows[0].Invalidate();
    }
}
