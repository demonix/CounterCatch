using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
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
}
