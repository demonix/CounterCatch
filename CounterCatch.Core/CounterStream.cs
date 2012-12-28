﻿using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CounterCatch
{
    public class CounterStream : IObservable<CounterValue>
    {
        IObservable<CounterValue> _data;
        public CounterInfo Counter { get; private set; }

        public CounterStream(CounterInfo counter)
        {
            Counter = counter;

            var performanceCounter = PerformanceCounterHelper.Get(counter.Category, counter.Name, counter.Instance, counter.Host);

            _data = Observable.Interval(TimeSpan.FromMilliseconds(counter.SamplingInterval), NewThreadScheduler.Default)
                                .Select((t) => NextData(performanceCounter))
                                .Finally(() => performanceCounter.Dispose());
        }

        CounterValue NextData(PerformanceCounter performanceCounter)
        {
            float counterValue = performanceCounter.NextValue();

            return new CounterValue(Counter, DateTime.Now, counterValue);
        }

        public IDisposable Subscribe(IObserver<CounterValue> observer)
        {
            return _data.Subscribe(observer);
        }
    }
}
