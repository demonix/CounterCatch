using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterCatch.Observers
{
    public interface CounterObserver : IObserver<CounterValue>
    {
        void Reset();

        void Init(IList<CounterInfo> counters);
    }
}
