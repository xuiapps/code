using Android.App;
using Android.Content;
using Android.Runtime;

namespace Xui.Apps;

[Application]
public class XuiMainApplication : Android.App.Application
{
    public XuiMainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

    public override void OnCreate()
    {
        Console.WriteLine("OnCreate");
        base.OnCreate();
    }

    public override void OnTerminate()
    {
        Console.WriteLine("OnTerminate");
        base.OnTerminate();
    }

    public override void OnLowMemory()
    {
        Console.WriteLine("OnLowMemory");
        base.OnLowMemory();
    }

    public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
    {
        Console.WriteLine("OnTrimMemory");
        base.OnTrimMemory(level);
    }
}
