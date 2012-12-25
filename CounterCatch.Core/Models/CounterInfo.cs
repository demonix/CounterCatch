﻿using System;
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
                            PerformanceCounterType type)
        {
            Host = host;
            Category = category;
            Name = name;
            Instance = instance;
            Type = type;
        }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Host { get; private set; }
        public string Instance { get; private set; }
        public PerformanceCounterType Type { get; private set; }

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
