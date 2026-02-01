using Xui.Core.Abstract;
using Xui.Core.Actual;
using Xui.Core.Debug;

namespace Xui.Runtime.Browser.Actual;

public partial class BrowserRunLoop : IRunLoop
{
    public Application Abstract { get; }

    public IRunLoopInstruments? Instruments { get; }

    public BrowserRunLoop(Application applicationAbstract, IRunLoopInstruments? instruments)
    {
        this.Abstract = applicationAbstract;
        this.Instruments = instruments;
    }

    public int Run()
    {
        Xui.Core.Actual.Runtime.CurrentRunLoop = this;
        this.Abstract.Start();
        return 0;
    }
}
