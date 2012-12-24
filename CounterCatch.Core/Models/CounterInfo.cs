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

        public CounterInfo(string counterId, string host, string category, string name, string instance)
        {
            CounterId = counterId;
            Host = host;
            Category = category;
            Name = name;
            Instance = instance;
        }

        public string CounterId { get; private set; }
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
            if (IsLocalHost)
                return CounterId;

            return string.Format("{0}/{1}", Host, CounterId);
        }
    }
}
