using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
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
}
