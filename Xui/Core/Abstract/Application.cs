using Xui.Core.Actual;

namespace Xui.Core.Abstract;

public abstract class Application
{
    public Application()
    {
    }

    public int Run() =>
        Runtime.Current
            .CreateRunloop(this)
            .Run();

    public abstract void Start();
}
