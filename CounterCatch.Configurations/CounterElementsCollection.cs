using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch.Configurations
{
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
}
