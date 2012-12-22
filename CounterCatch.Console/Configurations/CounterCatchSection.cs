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

    public class CounterElementsCollection : ConfigurationElementCollection, IEnumerable<CounterElement>
    {
        public CounterElementsCollection()
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

    public class HostGroupElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HostGroupElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HostGroupElement)element).Id;
        }

        public HostGroupElement this[int index]
        {
            get
            {
                return (HostGroupElement)BaseGet(index);
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

        new public HostGroupElement this[string id]
        {
            get
            {
                return (HostGroupElement)BaseGet(id);
            }
        }
    }

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

    public class HostElementsCollection : ConfigurationElementCollection, IEnumerable<HostElement>
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HostElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HostElement)element).Name;
        }

        public HostElement this[int index]
        {
            get
            {
                return (HostElement)BaseGet(index);
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

        new public HostElement this[string name]
        {
            get
            {
                return (HostElement)BaseGet(name);
            }
        }

        public new IEnumerator<HostElement> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
                yield return this[i];
        }
    }

    public class HostElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
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
    }

}
