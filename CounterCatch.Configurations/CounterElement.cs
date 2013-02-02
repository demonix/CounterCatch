using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
    public class CounterElement : ConfigurationElement
    {
        public CounterElement()
        {
        }

        [ConfigurationProperty("id", IsRequired = true, IsKey=true)]
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

        [ConfigurationProperty("hostGroup", IsRequired = true)]
        public string HostGroup
        {
            get
            {
                return (string)this["hostGroup"];
            }
            set
            {
                this["hostGroup"] = value;
            }
        }

        [ConfigurationProperty("category", IsRequired = true)]
        public string Category
        {
            get
            {
                return (string)this["category"];
            }
            set
            {
                this["category"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = false, DefaultValue="*")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("instance", IsRequired = false, DefaultValue = "*")]
        public string Instance
        {
            get
            {
                return (string)this["instance"];
            }
            set
            {
                this["instance"] = value;
            }
        }

        /// <summary>
        /// Gets the interval in milliseconds for each sample.
        /// </summary>
        [ConfigurationProperty("samplingInterval", IsRequired = false, DefaultValue = 1000)]
        public int SamplingInterval
        {
            get
            {
                return (int)this["samplingInterval"];
            }
            set
            {
                this["samplingInterval"] = value;
            }
        }

        [ConfigurationProperty("condition", IsRequired = false, DefaultValue = "")]
        public string Condition
        {
            get
            {
                return (string)this["condition"];
            }
            set
            {
                this["condition"] = value;
            }
        }
        [ConfigurationProperty("transform", IsRequired = false, DefaultValue = "")]
        public string Transform
        {
            get
            {
                return (string)this["transform"];
            }
            set
            {
                this["transform"] = value;
            }
        }
    }
}
