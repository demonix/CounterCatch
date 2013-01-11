using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CounterCatch.Observers
{
    public class CounterConsoleWriteLineObserver : CounterObserver
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(CounterValue value)
        {
            Console.WriteLine(string.Format("{0} {1} {2}", value.Time, value.Counter, value.Value));
        }
    }
}
