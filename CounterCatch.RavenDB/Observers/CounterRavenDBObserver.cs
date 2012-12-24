using Raven.Client.Embedded;
using Raven.Database.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CounterCatch.Observers
{
    public class CounterRavenDBObserver : IDisposable, CounterObserver
    {
        readonly int? _port;
        readonly string _dataDirectory;
        EmbeddableDocumentStore _documentStore;

        public CounterRavenDBObserver(string dataDirectory = "ravenDB", int? port = null)
        {
            _port = port;
            _dataDirectory = dataDirectory;

            _documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = _dataDirectory
            };

            if (_port != null)
            {
                NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(_port.Value);
                _documentStore.UseEmbeddedHttpServer = true;
                //_documentStore.Url = 
            }

            _documentStore.Initialize();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(CounterValue value)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(value);

                session.SaveChanges();
            }
        }

        public void Dispose()
        {
            if (_documentStore != null)
            {
                _documentStore.Dispose();
                _documentStore = null;
            }
        }
    }
}
