using Castle.Windsor;
using Castle.Windsor.Installer;
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
        static IWindsorContainer _container;
        static void Main(string[] args)
        {
            Console.WriteLine("Counter Catch - Performance counter collectors");

            ContainerBootstrap();

            try
            {
                StartMonitoring();
            }
            finally
            {
                _container.Dispose();
            }
        }

        private static void StartMonitoring()
        {
            var section = Configurations.CounterCatchSection.GetSection();

            var counters = section.GetCounters();
            var data = new MultiCounterStream(counters);

            var observers = _container.ResolveAll<Observers.CounterObserver>();

            try
            {
                Console.WriteLine("Press E to stop, R to reset values");

                var observer = new Observers.MultiCounterObserver(observers);
                observer.Init(counters);
                using (data.Subscribe(observer))
                {
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.E)
                            break;
                        if (key.Key == ConsoleKey.R)
                        {
                            observer.Reset();
                        }
                    }
                }
            }
            finally
            {
                foreach (var o in observers)
                    _container.Release(o);
            }
        }

        static void ContainerBootstrap()
        {
            _container = new WindsorContainer();

            _container.Install(Configuration.FromAppConfig());
        }
    }
}
