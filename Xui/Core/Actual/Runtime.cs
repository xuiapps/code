using Xui.Core.Actual;
using Xui.Core.Canvas;

namespace Xui.Core.Actual;

public static class Runtime
{
    private static IRuntime? current;

    public static IRuntime Current
    {
        get
        {
            if (current == null)
            {
                throw new RuntimeNotAvailable();
            }
            else
            {
                return current;
            }
        }

        set
        {
            current = value;
        }
    }

    public static IContext? DrawingContext => Current?.DrawingContext;
    public static IDispatcher? MainDispatcher => Current?.MainDispatcher;

    public class RuntimeNotAvailable : Exception
    {
        public RuntimeNotAvailable() : base("First thing to do in your app is set Runtime.Current!\n Somehow the execution reached to a Runtime.Current call before a runtime was set.")
        {
        }
    }
}
