using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch
{
    public class CounterInfo
    {
        public CounterInfo(string host, string category, string name, string instance)
        {
            Host = host;
            Category = category;
            Name = name;
            Instance = instance;
        }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Host { get; private set; }
        public string Instance { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}/{3}", Host, Category, Name, Instance);
        }
    }
}
