using System;
using System.Collections.Generic;
using System.Configuration;
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
                    var counterInfo = new CounterInfo(host.Name, counter.Category, counter.Name, counter.Instance);

                    data.Add(counterInfo);
                }
            }
            return data;
        }
    }
}
