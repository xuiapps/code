using System.Diagnostics;

namespace Xui.Core.Debug;

public static partial class Instruments
{
    public static IInstruments File(string path) => new FileLogInstruments(path);

    private class FileLogInstruments : IInstruments
    {
        private readonly string path;

        public FileLogInstruments(string path)
        {
            this.path = path;
        }

        public IInstrumentsSink CreateSink() => new FileLogSink(this.path);
    }

    private class FileLogSink : IInstrumentsSink
    {
        private readonly StreamWriter writer;
        private readonly Stopwatch stopwatch;
        private int traceDepth;

        public FileLogSink(string path)
        {
            this.writer = new StreamWriter(path, append: false) { AutoFlush = true };
            this.stopwatch = Stopwatch.StartNew();
            this.writer.WriteLine($"# Instruments log started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            this.writer.WriteLine();
        }

        public bool IsEnabled(Scope scope, LevelOfDetail lod) => true;

        public void Log(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message)
        {
            WriteTimestamp();
            WriteIndent();
            writer.Write('[');
            writer.Write(scope.ToString());
            writer.Write("] ");
            writer.WriteLine(message);
        }

        public void BeginTrace(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message)
        {
            WriteTimestamp();
            WriteIndent();
            writer.Write('[');
            writer.Write(scope.ToString());
            writer.Write("] >> ");
            writer.WriteLine(message);
            traceDepth++;
        }

        public void EndTrace()
        {
            if (traceDepth > 0)
                traceDepth--;
        }

        public void Dispose()
        {
            writer.WriteLine();
            writer.WriteLine($"# Instruments log ended at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            writer.Dispose();
        }

        private void WriteTimestamp()
        {
            var elapsed = stopwatch.Elapsed;
            writer.Write(elapsed.ToString(@"mm\:ss\.fff"));
            writer.Write(' ');
        }

        private void WriteIndent()
        {
            for (int i = 0; i < traceDepth; i++)
                writer.Write("  ");
        }
    }
}
