using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CounterCatch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Counter Catch - Performance counter collectors");

            var section = Configurations.CounterCatchSection.GetSection();

            var data = new MultiCounterStream(section.GetCounters());

            using (data.Subscribe(new Observers.CounterAggregateObserver(new Observers.CounterCsvExportObserver(), new Observers.CounterConsoleObserver())))
            {
                Console.WriteLine("Press enter to stop");
                Console.ReadLine();
            }
        }
    }
}
