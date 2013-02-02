using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CounterCatch
{
    public class CounterStream : IObservable<CounterValue>
    {
        IObservable<CounterValue> _data;
        CounterInfo _counter;
        public CounterInfo Counter { get {return _counter;} }

        public CounterStream(CounterInfo counter)
        {
            _counter = counter;

            var performanceCounter = PerformanceCounterHelper.Get(counter.Category, counter.Name, counter.Instance, counter.Host);

            _data = Observable.Interval(TimeSpan.FromMilliseconds(_counter.SamplingInterval), NewThreadScheduler.Default)
                                .Select((t) => NextData(performanceCounter))
                                .Where(Condition)
                                .Select(Transform)
                                .Finally(() => performanceCounter.Dispose());
        }

        bool Condition(CounterValue counterValue)
        {
            return _counter.Condition == null || _counter.Condition(counterValue.Value);
        }

        CounterValue Transform(CounterValue counterValue)
        {
            return _counter.Transform == null ?
                counterValue :
                new CounterValue(counterValue.Counter, counterValue.Time, _counter.Transform(counterValue.Value));
        }

        CounterValue NextData(PerformanceCounter performanceCounter)
        {
            float counterValue = performanceCounter.NextValue();

            return new CounterValue(_counter, DateTime.Now, counterValue);
        }

        public IDisposable Subscribe(IObserver<CounterValue> observer)
        {
            return _data.Subscribe(observer);
        }
    }
}
