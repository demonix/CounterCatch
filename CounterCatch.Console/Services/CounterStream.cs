using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CounterCatch
{
    public class CounterStream : IObservable<CounterData>
    {
        const string LocalHost = "localhost";
        IObservable<CounterData> _data;
        public CounterInfo Counter { get; private set; }

        public CounterStream(CounterInfo counter)
        {
            Counter = counter;

            PerformanceCounter performanceCounter;
            if (string.Equals(counter.Host, LocalHost, StringComparison.InvariantCultureIgnoreCase))
                performanceCounter = new PerformanceCounter(counter.Category, counter.Name, counter.Instance);
            else
                performanceCounter = new PerformanceCounter(counter.Category, counter.Name, counter.Instance, counter.Host);

            _data = Observable.Interval(TimeSpan.FromSeconds(1), NewThreadScheduler.Default)
                                .Select((t) => NextData(performanceCounter))
                                .Finally(() => performanceCounter.Dispose());
        }

        CounterData NextData(PerformanceCounter performanceCounter)
        {
            float counterValue = performanceCounter.NextValue();

            return new CounterData(Counter, DateTime.Now, counterValue);
        }

        public IDisposable Subscribe(IObserver<CounterData> observer)
        {
            return _data.Subscribe(observer);
        }
    }
}
