using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CounterCatch.Observers
{
    public class CounterTraceObserver : IObserver<CounterValue>
    {
        static int EventId = 0;
        static TraceSource TraceSource = new TraceSource("CounterCatch", SourceLevels.All);

        public void OnCompleted()
        {
            int eventId = Interlocked.Increment(ref EventId);
            TraceSource.TraceEvent(TraceEventType.Verbose, eventId, "Trace completed");
        }

        public void OnError(Exception error)
        {
            int eventId = Interlocked.Increment(ref EventId);
            TraceSource.TraceEvent(TraceEventType.Warning, eventId, error.Message);
            TraceSource.TraceData(TraceEventType.Warning, eventId, error);
        }

        public void OnNext(CounterValue value)
        {
            int eventId = Interlocked.Increment(ref EventId);
            TraceSource.TraceEvent(TraceEventType.Information, eventId, string.Format("{0} {1} {2}", value.Time, value.Counter, value.Value));
        }
    }
}
