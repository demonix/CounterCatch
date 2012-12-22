using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Generic;

namespace CounterCatch
{
    public class MultiCounterStream : IObservable<CounterValue>
    {
        IObservable<CounterValue> _data;

        public MultiCounterStream(IEnumerable<CounterInfo> counters)
        {
            _data = Observable.Empty<CounterValue>();
            foreach (var c in counters)
                _data = _data.Merge(new CounterStream(c));
        }

        public IDisposable Subscribe(IObserver<CounterValue> observer)
        {
            return _data.Subscribe(observer);
        }
    }
}
