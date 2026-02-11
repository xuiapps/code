namespace Xui.Core.Debug;

public static partial class Instruments
{
    private class ConsoleLogInstruments : IInstruments
    {
        public IInstrumentsSink CreateSink() => new ConsoleLogSink();
    }

    private class ConsoleLogSink : IInstrumentsSink
    {
        private int traceDepth;

        public bool IsEnabled(Scope scope, LevelOfDetail lod) => true;

        public void Log(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message)
        {
            WriteIndent();
            System.Console.Write('[');
            System.Console.Write(scope.ToString());
            System.Console.Write("] ");
            System.Console.Out.WriteLine(message);
        }

        public void BeginTrace(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message)
        {
            WriteIndent();
            System.Console.Write('[');
            System.Console.Write(scope.ToString());
            System.Console.Write("] >> ");
            System.Console.Out.WriteLine(message);
            traceDepth++;
        }

        public void EndTrace()
        {
            if (traceDepth > 0)
                traceDepth--;
        }

        public void Dispose()
        {
        }

        private void WriteIndent()
        {
            for (int i = 0; i < traceDepth; i++)
                System.Console.Write("  ");
        }
    }
}
