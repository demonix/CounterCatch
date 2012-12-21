using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch
{
    class Program
    {
        static void Main(string[] args)
        {
            var section = Configurations.CounterCatchSection.GetSection();

            var data = Observable.Empty<CounterData>();
            foreach (var counter in section.Counters)
            {
                var collector = new CounterCollector(new CounterInfo(counter.Host, counter.Category, counter.Name, counter.Instance));

                data = data.Merge(collector);
            }

            using (data.Subscribe((c) => Console.WriteLine(c.Value)))
            {
                Console.ReadLine();
            }
        }
    }
}
