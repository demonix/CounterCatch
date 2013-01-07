using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CounterCatch
{
    public static class PerformanceCounterHelper
    {
        public static PerformanceCounter Get(string category, string counterName, string instance, string host)
        {
            if (CounterInfo.IsMachineLocalHost(host))
                return new PerformanceCounter(category, counterName, instance, true);
            else
                return new PerformanceCounter(category, counterName, instance, host);
        }

        public static IEnumerable<string> GetCounterInstances(string category, string host)
        {
            var perfCategory = GetCategory(category, host);
            if (perfCategory.CategoryType == PerformanceCounterCategoryType.MultiInstance)
                return perfCategory.GetInstanceNames();
            else
                return new []{string.Empty};
        }

        public static IEnumerable<string> GetCounterNames(string category, string instance, string host)
        {
            var perfCounterCategory = GetCategory(category, host);

            PerformanceCounter[] counters;
            if (string.IsNullOrWhiteSpace(instance))
                counters = perfCounterCategory.GetCounters();
            else
                counters = perfCounterCategory.GetCounters(instance);

            try
            {
                return counters
                        .Where(IsNotCounterBase)
                        .Select(p => p.CounterName).ToArray();
            }
            finally
            {
                foreach (var c in counters)
                    c.Dispose();
            }
        }

        private static bool IsNotCounterBase(PerformanceCounter p)
        {
            return p.CounterType != PerformanceCounterType.AverageBase
                && p.CounterType != PerformanceCounterType.CounterMultiBase
                && p.CounterType != PerformanceCounterType.RawBase
                && p.CounterType != PerformanceCounterType.SampleBase;
        }

        private static PerformanceCounterCategory GetCategory(string category, string host)
        {
            if (CounterInfo.IsMachineLocalHost(host))
                return new PerformanceCounterCategory(category);
            else
                return new PerformanceCounterCategory(category, host);
        }
    }
}
