using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
    public class HostGroupElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("hosts", IsDefaultCollection=true)]
        [ConfigurationCollection(typeof(HostElementsCollection))]
        public HostElementsCollection Hosts
        {
            get
            {
                return (HostElementsCollection)base["hosts"];
            }
        }
    }
}
