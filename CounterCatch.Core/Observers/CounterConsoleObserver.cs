using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CounterCatch.Observers
{
    public class CounterConsoleObserver : CounterObserver
    {
        private object _lock = new object();
        private Dictionary<string, Stats> _data = new Dictionary<string, Stats>(StringComparer.InvariantCultureIgnoreCase);
        const int LAST_UPDATE_ROW = 3;
        const int HEADER_ROW = 4;
        const int FIRST_DATA_ROW = 5;

        public CounterConsoleObserver()
        {
        }

        public void Reset()
        {
            lock (_lock)
            {
                foreach (var v in _data.Values)
                {
                    v.CounterValues.Clear();

                    UpdateCounterStats(v.Counter, v, null);
                }
            }
        }

        public void Init(IList<CounterInfo> counters)
        {
            int row = FIRST_DATA_ROW;
            foreach (var c in counters)
            {
				string key = GetCounterShortName(c);
				if (!_data.ContainsKey(key))
	                _data.Add(GetCounterShortName(c), new Stats(c, row++));
            }

            UpdateConsoleDataRow(HEADER_ROW, "Counter",
                                "Last", "Avg", "Perc90", "Max");
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(CounterValue value)
        {
            string lastUpdate = string.Format("Last Update: {0}", value.Time.ToLongTimeString());

            lock (_lock)
            {
                UpdateConsoleRow(LAST_UPDATE_ROW, lastUpdate);

                var stats = GetStats(value);
                UpdateCounterStats(value.Counter, stats, value.Value);
            }
        }

        private void UpdateCounterStats(CounterInfo counter, Stats stats, double? last)
        {
            string counterName = GetCounterShortName(counter);
            UpdateConsoleDataRow(stats.ConsoleRow, counterName,
                                NumberToString(last), NumberToString(stats.Avarage()),
                                NumberToString(stats.Percentile(0.9)), NumberToString(stats.Max()));
        }

        string NumberToString(double? number)
        {
            if (number == null)
                return string.Empty;

            return number.Value.ToString("#,#0.##;-#,#0.##;0.00");
        }

        string GetCounterShortName(CounterInfo counterInfo)
        {
            string instanceString = "";
            if (!string.IsNullOrWhiteSpace(counterInfo.Instance))
                instanceString = string.Format("/{0}", counterInfo.Instance);

            return string.Format("{0}{1}", counterInfo.Name, instanceString);
        }

        void UpdateConsoleDataRow(int row, string counterName, params string[] values)
        {
            string counterRow = string.Format("{0} {1}", 
                                               counterName.PadRight(40).Substring(0, 40), 
                                               string.Join(" ", values.Select(p => p.PadLeft(12))));
            UpdateConsoleRow(row, counterRow);
        }

        void UpdateConsoleRow(int row, string str)
        {
            Console.SetCursorPosition(0, row);

            Console.Write(str.PadRight(Console.BufferWidth));
        }

        Stats GetStats(CounterValue counterValue)
        {
            string key = GetCounterShortName(counterValue.Counter);

            Stats row;
            if (_data.TryGetValue(key, out row))
            {
                row.CounterValues.Add(counterValue);
                return row;
            }
            else
                throw new ApplicationException("Counter not initialized on the observer");
        }

        class Stats
        {
            public Stats(CounterInfo counter, int consoleRow)
            {
                Counter = counter;
                ConsoleRow = consoleRow;
            }

            public CounterInfo Counter;
            public int ConsoleRow;
            public List<CounterValue> CounterValues = new List<CounterValue>();

            public double? Avarage()
            {
                if (CounterValues.Count == 0)
                    return null;
                return CounterValues.Average(p => p.Value);
            }
            public double? Max()
            {
                if (CounterValues.Count == 0)
                    return null;
                return CounterValues.Max(p => p.Value);
            }
            public double? Min()
            {
                if (CounterValues.Count == 0)
                    return null;
                return CounterValues.Min(p => p.Value);
            }
            public double? Percentile(double percentile)
            {
                if (CounterValues.Count == 0)
                    return null;
                var vals = CounterValues.Select(p => p.Value).OrderBy(p => p).ToList();
                return vals[(int)Math.Floor(percentile * (double)vals.Count)];
            }
        }
   }
}
