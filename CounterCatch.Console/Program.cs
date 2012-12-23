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

            var data = new MultiCounterStream(section.GetCounters());

            var observers = _container.ResolveAll<Observers.CounterObserver>();

            try
            {
                using (data.Subscribe(new Observers.MultiCounterObserver(observers)))
                {
                    Console.WriteLine("Press enter to stop");
                    Console.ReadLine();
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
