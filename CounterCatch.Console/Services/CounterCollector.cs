using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CounterCatch
{
    public class CounterCollector : IDisposable, IObservable<CounterData>
    {
        PerformanceCounter _performanceCounter;
        public CounterInfo Counter { get; private set; }
        const int Timeout = 5000;
        IObservable<CounterData> _data;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public CounterCollector(CounterInfo counter)
        {
            if (string.IsNullOrWhiteSpace(counter.Host))
                _performanceCounter = new PerformanceCounter(counter.Category, counter.Name, counter.Instance);
            else
                _performanceCounter = new PerformanceCounter(counter.Category, counter.Name, counter.Instance, counter.Host);

            Counter = counter;
            _data = Observable.Interval(TimeSpan.FromSeconds(1), NewThreadScheduler.Default)
 //                               .TakeWhile((t) => _cancellationTokenSource.IsCancellationRequested)
                                .Select((t) => NextData());
        }

        //public void Start()
        //{
        //    if (_thread != null)
        //        throw new InvalidOperationException(string.Format("CounterCollector '{0}' already started.", Counter));

        //    _thread = new Thread(CollectData);
        //    _thread.Start();
        //}

        //public void Stop()
        //{
        //    if (_thread == null)
        //        return;

        //    _stopRequested = true;

        //    bool completed = _thread.Join(ThreadStopTimeout);
        //    if (!completed)
        //        throw new TimeoutException(string.Format("CounterCollector '{0}' cannot be stopped.", Counter));

        //    _thread = null;
        //    _stopRequested = false;
        //}

        CounterData NextData()
        {
            float counterValue = _performanceCounter.NextValue();

            return new CounterData(Counter, DateTime.Now, counterValue);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel(true);
        }

        public void Dispose()
        {
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            if (_performanceCounter != null)
            {
                _performanceCounter.Dispose();
                _performanceCounter = null;
            }
        }

        public IDisposable Subscribe(IObserver<CounterData> observer)
        {
            return _data.Subscribe(observer);
        }
    }
}
