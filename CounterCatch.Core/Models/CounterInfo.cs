using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterCatch
{
    public class CounterInfo
    {
        public const string LocalHost = "localhost";

        public CounterInfo(string host, string category, string name, string instance,
                            PerformanceCounterType type, int samplingInterval,
                            Predicate<double> condition = null, Func<double, double> transform = null)
        {
            Host = host;
            Category = category;
            Name = name;
            Instance = instance;
            Type = type;
            SamplingInterval = samplingInterval;
            Condition = condition;
            Transform = transform;
        }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Host { get; private set; }
        public string Instance { get; private set; }
        public PerformanceCounterType Type { get; private set; }
        /// <summary>
        /// Gets the interval in milliseconds for each sample.
        /// </summary>
        public int SamplingInterval { get; private set; }

        public Predicate<double> Condition { get; private set; }
        public Func<double, double> Transform { get; private set; }

        public static bool IsMachineLocalHost(string host)
        {
            return string.Equals(LocalHost, host, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsLocalHost
        {
            get
            {
                return IsMachineLocalHost(Host);
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
