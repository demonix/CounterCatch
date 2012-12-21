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
                throw new ConfigurationException("CounterCatch configuration not found.");

            return section;
        }

        [ConfigurationProperty("counters")]
        [ConfigurationCollection(typeof(CountersCollection))]
        public CountersCollection Counters
        {
            get
            {
                return (CountersCollection)base["counters"];
            }
        }
    }

    public class CountersCollection : ConfigurationElementCollection, IEnumerable<CounterElement>
    {
        public CountersCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CounterElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((CounterElement)element).Id;
        }

        public CounterElement this[int index]
        {
            get
            {
                return (CounterElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public CounterElement this[string id]
        {
            get
            {
                return (CounterElement)BaseGet(id);
            }
        }

        public new IEnumerator<CounterElement> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
                yield return this[i];
        }
    }

    public class CounterElement : ConfigurationElement
    {
        public CounterElement()
        {
        }

        public CounterElement(string id, string host, string category, string name, string instance)
        {
            Id = id;
            Host = host;
            Name = name;
            Category = category;
            Instance = instance;
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

        [ConfigurationProperty("host", IsRequired = false, DefaultValue="")]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
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

        [ConfigurationProperty("name", IsRequired = true)]
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

        [ConfigurationProperty("instance", IsRequired = false, DefaultValue="")]
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

    }
}
