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
    public class MultiCounterObserver : CounterObserver
    {
        readonly IObserver<CounterValue>[] _observers;

        public MultiCounterObserver(params IObserver<CounterValue>[] observers)
        {
            if (observers != null)
                _observers = observers;
            else
                _observers = new IObserver<CounterValue>[0];
        }

        public MultiCounterObserver(IEnumerable<IObserver<CounterValue>> observers)
        {
            _observers = observers.ToArray();
        }

        public void Reset()
        {
            foreach (var o in _observers)
            {
                if (o is CounterObserver)
                    ((CounterObserver)o).Reset();
            }
        }

        public void Init(IList<CounterInfo> counters)
        {
            foreach (var o in _observers)
            {
                if (o is CounterObserver)
                    ((CounterObserver)o).Init(counters);
            }
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
    }
}
