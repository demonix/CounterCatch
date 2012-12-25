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
                    foreach (var instance in GetCounterInstances(counter, host))
                    {
                        foreach (var counterName in GetCounters(counter, host, instance))
                        {
                            using (var perfCounter = GetPerformanceCounter(counter.Category, counterName, instance, host.Name))
                            {
                                var counterInfo = new CounterInfo(counter.Id, host.Name, perfCounter.CategoryName, perfCounter.CounterName, perfCounter.InstanceName, perfCounter.CounterType);

                                data.Add(counterInfo);
                            }
                        }
                    }
                }
            }
            return data;
        }

        private IEnumerable<string> GetCounterInstances(CounterElement counter, HostElement host)
        {
            if (counter.Instance != "*")
                return new string[]{counter.Instance};

            PerformanceCounterCategory category = GetPerformanceCounterCategory(counter, host);

            return category.GetInstanceNames();
        }

        private static PerformanceCounterCategory GetPerformanceCounterCategory(CounterElement counter, HostElement host)
        {
            PerformanceCounterCategory category;
            if (CounterInfo.IsMachineLocalHost(host.Name))
                category = new PerformanceCounterCategory(counter.Category);
            else
                category = new PerformanceCounterCategory(counter.Category, host.Name);
            return category;
        }

        private static PerformanceCounter GetPerformanceCounter(string category, string counterName, string instance, string host)
        {
            if (CounterInfo.IsMachineLocalHost(host))
                return new PerformanceCounter(category, counterName, instance, true);
            else
                return new PerformanceCounter(category, counterName, instance, host);
        }

        private IEnumerable<string> GetCounters(CounterElement counter, HostElement host, string instance)
        {
            if (counter.Name != "*")
                return new string[] { counter.Name };

            PerformanceCounterCategory category = GetPerformanceCounterCategory(counter, host);

            PerformanceCounter[] counters;
            if (string.IsNullOrWhiteSpace(instance))
                counters = category.GetCounters();
            else
                counters = category.GetCounters(instance);

            try
            {
                return counters.Select(p => p.CounterName).ToArray();
            }
            finally
            {
                foreach (var c in counters)
                    c.Dispose();

            }
        }
    }
}
