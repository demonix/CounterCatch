using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
    public class CounterCatchSection : ConfigurationSection
    {
        public static CounterCatchSection GetSection(bool throwIfNotFound = true)
        {
            var section = (CounterCatchSection)
                            System.Configuration.ConfigurationManager.GetSection("counterCatch");

            if (section == null && throwIfNotFound)
                throw new ConfigurationErrorsException("CounterCatch configuration not found.");

            return section;
        }

        [ConfigurationProperty("counters")]
        [ConfigurationCollection(typeof(CounterElementsCollection))]
        public CounterElementsCollection Counters
        {
            get
            {
                return (CounterElementsCollection)base["counters"];
            }
        }

        [ConfigurationProperty("hostGroups")]
        [ConfigurationCollection(typeof(HostGroupElementCollection))]
        public HostGroupElementCollection HostGroups
        {
            get
            {
                return (HostGroupElementCollection)base["hostGroups"];
            }
        }

        public IList<CounterInfo> GetCounters()
        {
            var data = new List<CounterInfo>();
            foreach (var counter in Counters)
            {
                var hostGroup = HostGroups[counter.HostGroup];
                if (hostGroup == null)
                    throw new ConfigurationErrorsException(string.Format("HostGroup '{0}' not defined.", counter.HostGroup));

                foreach (var host in hostGroup.Hosts)
                {
                    foreach (var instance in GetInstances(counter, host))
                    {
                        foreach (var counterName in GetCounters(counter, host, instance))
                        {
                            using (var perfCounter = PerformanceCounterHelper.Get(counter.Category, counterName, instance, host.Name))
                            {
                                var counterInfo = new CounterInfo(host.Name, perfCounter.CategoryName, perfCounter.CounterName, perfCounter.InstanceName, perfCounter.CounterType);

                                data.Add(counterInfo);
                            }
                        }
                    }
                }
            }
            return data;
        }

        private IEnumerable<string> GetCounters(CounterElement counter, HostElement host, string instance)
        {
            if (counter.Name != "*")
                return new []{ counter.Name };

            return PerformanceCounterHelper.GetCounterNames(counter.Category, instance, host.Name);
        }

        private IEnumerable<string> GetInstances(CounterElement counter, HostElement host)
        {
            if (counter.Instance != "*")
                return new[] { counter.Instance };

            return PerformanceCounterHelper.GetCounterInstances(counter.Category, host.Name);
        }
    }
}
