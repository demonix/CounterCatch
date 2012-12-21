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

            var data = GetCountersStream(section);

            using (data.Subscribe((c) => Console.WriteLine(string.Format("{0}:{1}", c.Counter, c.Value))))
            {
                Console.WriteLine("Press enter to stop");
                Console.ReadLine();
            }
        }

        static IObservable<CounterData> GetCountersStream(Configurations.CounterCatchSection section)
        {
            var data = Observable.Empty<CounterData>();
            foreach (var counter in section.Counters)
            {
                var hosts = counter.GetHosts();
                foreach (var host in hosts)
                {
                    var counterInfo = new CounterInfo(host, counter.Category, counter.Name, counter.Instance);
                    Console.WriteLine("Registering to counter {0}", counterInfo);

                    var counterStream = new CounterStream(counterInfo);

                    data = data.Merge(counterStream);
                }
            }
            return data;
        }
    }
}
