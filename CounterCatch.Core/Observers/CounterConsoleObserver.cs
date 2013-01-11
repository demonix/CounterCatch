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
        private Dictionary<string, int> _rowsPosition = new Dictionary<string, int>();
        private int _consoleRows = 4;
        const int LAST_UPDATE_ROW = 3;
        const int DEFAULT_BUFFERWIDTH = 60;

        public CounterConsoleObserver()
        {
            if (Console.BufferWidth < DEFAULT_BUFFERWIDTH)
                Console.BufferWidth = DEFAULT_BUFFERWIDTH;
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
            string counterName = GetCounterShortName(value.Counter).PadRight(50).Substring(0, 50);
            string counterValue = value.Value.ToString().PadLeft(10);
            string counterRow = string.Format("{0}: {1}", counterName, counterValue);

            lock (_lock)
            {
                UpdateConsoleRow(LAST_UPDATE_ROW, lastUpdate);
                UpdateConsoleRow(GetConsoleRow(value.Counter), counterRow);
            }
        }

        string GetCounterShortName(CounterInfo counterInfo)
        {
            string instanceString = "";
            if (!string.IsNullOrWhiteSpace(counterInfo.Instance))
                instanceString = string.Format("/{0}", counterInfo.Instance);

            if (counterInfo.IsLocalHost)
                return string.Format("{0}{1}", counterInfo.Name, instanceString);

            return string.Format("{0}/{1}{2}", counterInfo.Host, counterInfo.Name, instanceString);
        }

        void UpdateConsoleRow(int row, string str)
        {
            Console.SetCursorPosition(0, row);

            Console.Write(str.PadRight(Console.BufferWidth));
        }

        int GetConsoleRow(CounterInfo info)
        {
            string key = info.ToString();

            int row;
            if (_rowsPosition.TryGetValue(key, out row))
                return row;

            row = _consoleRows++;
            _rowsPosition.Add(key, row);

            return row;
        }
    }
}
