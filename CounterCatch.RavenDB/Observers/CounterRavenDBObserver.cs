﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CounterCatch.Observers
{
    public class CounterRavenDBObserver : IDisposable, CounterObserver
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(CounterValue value)
        {
        }

        public void Dispose()
        {
        }
    }
}