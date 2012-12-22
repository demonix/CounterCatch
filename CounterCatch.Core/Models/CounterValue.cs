using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch
{
    public class CounterValue
    {
        public CounterValue(CounterInfo counter, DateTime time, double value)
        {
            Counter = counter;
            Time = time;
            Value = value;
        }

        public CounterInfo Counter { get; private set; }
        public DateTime Time { get; private set; }
        public double Value { get; private set; }
    }
}
