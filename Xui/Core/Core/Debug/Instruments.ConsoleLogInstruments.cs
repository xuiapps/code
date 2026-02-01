namespace Xui.Core.Debug;

public static partial class Instruments
{
    /// <summary>
    /// An <see cref="IInstruments"/> implementation that creates run-loop–aligned
    /// instruments which emit diagnostic information to the console.
    ///
    /// This factory maintains a list of all run-loop instrumentation instances
    /// it has created, allowing correlation or aggregation of output across
    /// multiple run loops when applicable.
    /// </summary>
    public class ConsoleLogInstruments : IInstruments
    {
        private readonly List<ConsoleLogRunLoopInstruments> runloops = new List<ConsoleLogRunLoopInstruments>();

        /// <summary>
        /// Creates a new console-based instrumentation instance for a single run loop.
        ///
        /// This method is invoked once per run loop during application startup.
        /// The returned instance is owned by the run loop for its entire lifetime
        /// and is expected to be used exclusively on the run loop's thread.
        /// </summary>
        /// <returns>
        /// A run-loop–aligned instrumentation instance that writes diagnostic
        /// output to the console.
        /// </returns>
        public IRunLoopInstruments CreateRunLoop()
        {
            var i = new ConsoleLogRunLoopInstruments(this);
            this.runloops.Add(i);
            return i;
        }

        internal void OnRunLoopDispose(ConsoleLogRunLoopInstruments runLoop)
        {
            runloops.Remove(runLoop);
        }

        /// <summary>
        /// Run-loop–aligned instrumentation instance that records and emits
        /// diagnostic events for a single run loop.
        ///
        /// Instances of this type are created by <see cref="ConsoleLogInstruments"/>
        /// and must not be shared across multiple run loops or threads.
        /// </summary>
        public class ConsoleLogRunLoopInstruments : IRunLoopInstruments
        {
            private readonly ConsoleLogInstruments parent;
            private readonly System.Text.StringBuilder messageBuffer = new System.Text.StringBuilder(256);

            /// <summary>
            /// Initializes a new run-loop–aligned console instrumentation instance.
            /// </summary>
            /// <param name="parent">
            /// The owning <see cref="ConsoleLogInstruments"/> factory that created
            /// this run-loop instrumentation instance.
            /// </param>
            public ConsoleLogRunLoopInstruments(ConsoleLogInstruments parent)
            {
                this.parent = parent;
            }

            /// <inheritdoc />
            public void Event(LevelOfDetail levelOfDetail, Aspect aspect, ref InstrumentsInterpolatedStringHandler message)
            {
                System.Console.Write("event: ");
            }

            /// <inheritdoc />
            public TraceScope Trace(LevelOfDetail levelOfDetail, Aspect aspect, ref InstrumentsInterpolatedStringHandler message)
            {
                System.Console.Write("start: ");
                System.Console.Write(messageBuffer);
                System.Console.WriteLine();
                messageBuffer.Clear();
                return new TraceScope(this);
            }

            /// <inheritdoc />
            public void EndTrace()
            {
                System.Console.WriteLine("end");
            }

            void IMessageBuilder.AppendLiteral(string value) =>
                messageBuffer.Append(value);

            void IMessageBuilder.AppendFormatted<T>(T value) =>
                messageBuffer.Append(value);

            void IMessageBuilder.AppendFormatted<T>(T value, string? format) =>
                messageBuffer.AppendFormat($"{{0:{format}}}", value);

            void IMessageBuilder.EndMessage()
            {
                System.Console.Write(messageBuffer);
                System.Console.WriteLine();
                messageBuffer.Clear();
            }

            void IDisposable.Dispose() => parent.OnRunLoopDispose(this);
        }
    }
}
