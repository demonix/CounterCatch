using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reactive;
using System.Reactive.Disposables;

namespace CounterCatch.Observers
{
    public class CounterAggregateObserver : IDisposable, IObserver<CounterValue>
    {
        readonly IObserver<CounterValue>[] _observers;

        public CounterAggregateObserver(params IObserver<CounterValue>[] observers)
        {
            if (observers != null)
                _observers = observers;
            else
                _observers = new IObserver<CounterValue>[0];
        }

        public CounterAggregateObserver(IEnumerable<IObserver<CounterValue>> observers)
        {
            _observers = observers.ToArray();
        }

        public void OnCompleted()
        {
            foreach (var o in _observers)
                o.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var o in _observers)
                o.OnError(error);
        }

        public void OnNext(CounterValue value)
        {
            foreach (var o in _observers)
                o.OnNext(value);
        }

        public void Dispose()
        {
            foreach (var o in _observers)
            {
                var disposable = o as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }
    }
}
