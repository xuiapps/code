namespace Xui.Core.Actual;

public interface IDispatcher
{
    /// <summary>
    /// Post the callback to execute on the dispatcher's thread.
    /// Use this method when you are sure that you are running on a different thread,
    /// and want to transition to the dispatcher's thread.
    /// </summary>
    void Post(Action callback);
}
