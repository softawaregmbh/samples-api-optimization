using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Ui.Tracing
{
    public class TrafficTracer : IDisposable
    {
        private TraceEventSession etwSession;

        private object counterLock;
        public long ReceivedBytes { get; set; }
        public long SentBytes { get; set; }

        private TrafficTracer() { }

        public static TrafficTracer Create()
        {
            var trafficTracer = new TrafficTracer();
            trafficTracer.Init();
            return trafficTracer;
        }

        private void Init()
        {
            this.counterLock = new object();
            Task.Run(() => StartEtwSession());
        }

        private void StartEtwSession()
        {
            var pid = Process.GetCurrentProcess().Id;
            lock (this.counterLock)
            {
                this.ReceivedBytes = 0;
                this.SentBytes = 0;
            }

            using (this.etwSession = new TraceEventSession("KernelAndClrEventsSession"))
            {
                this.etwSession.EnableKernelProvider(KernelTraceEventParser.Keywords.NetworkTCPIP);

                this.etwSession.Source.Kernel.TcpIpRecv += data =>
                {
                    if (data.ProcessID == pid)
                    {
                        lock (counterLock)
                        {
                            this.ReceivedBytes += data.size;
                        }
                    }
                };

                this.etwSession.Source.Kernel.TcpIpSend += data =>
                {
                    if (data.ProcessID == pid)
                    {
                        lock (counterLock)
                        {
                            this.SentBytes += data.size;
                        }
                    }
                };

                this.etwSession.Source.Process();
            }
        }

        public void Dispose()
        {
            this.etwSession?.Stop();

            lock (counterLock)
            {
                // wait until all write op's are finished
            }

            this.etwSession?.Dispose();
        }
    }
}
