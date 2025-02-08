using Android.App;
using Android.Runtime;
using Xui.Runtime.Android.Actual;

namespace Xui.Apps.BlankApp;

[Application]
public class MainApplication : XuiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}
}
