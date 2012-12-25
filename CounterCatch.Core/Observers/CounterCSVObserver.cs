﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;

namespace CounterCatch.Observers
{
    public class CounterCSVObserver : CounterObserver, IDisposable
    {
        StreamWriter _writer;
        readonly CultureInfo _culture;
        readonly string DateTimeFormat = "G";
        DateTime _initialDate;

        public CounterCSVObserver(string destinationFile, string culture = "en-US")
        {
            _writer = new StreamWriter(destinationFile);
            _culture = CultureInfo.GetCultureInfo(culture);
            _initialDate = DateTime.Now;

            WriteFields("Time", "Elapsed Milliseconds", "Host", "Tag", "CounterCategory", "CounterInstance", "CounterName", "Value", "CounterType");
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(CounterValue value)
        {
            WriteFields(value.Time,
                (value.Time - _initialDate).TotalMilliseconds,
                value.Counter.Host,
                value.Counter.Tag,
                value.Counter.Category, 
                value.Counter.Instance, 
                value.Counter.Name,
                value.Value,
                value.Counter.CounterType);
        }

        void WriteFields(params object[] values)
        {
            var fieldValues = values.Select(ValueAsField);

            _writer.WriteLine(string.Join(_culture.TextInfo.ListSeparator, fieldValues));
        }

        string ValueAsField(object obj)
        {
            if (obj is string)
                return string.Format("\"{0}\"", obj);
            if (obj is DateTime)
                return ((DateTime)obj).ToString(DateTimeFormat, _culture);
            else
                return Convert.ToString(obj, _culture);
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
        }
    }
}
