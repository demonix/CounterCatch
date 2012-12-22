using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch
{
    public class CounterInfo
    {
        public const string LocalHost = "localhost";

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

        public bool IsLocalHost
        {
            get
            {
                return string.Equals(LocalHost, Host, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public override string ToString()
        {
            string instanceString = "";
            if (!string.IsNullOrWhiteSpace(Instance))
                instanceString = string.Format("/{0}", Instance);

            if (IsLocalHost)
                return string.Format("{0}/{1}{2}", Category, Name, instanceString);

            return string.Format("{0}/{1}/{2}{3}", Host, Category, Name, instanceString);
        }
    }
}
