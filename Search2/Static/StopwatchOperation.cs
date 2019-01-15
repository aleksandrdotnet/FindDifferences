using System;
using System.Diagnostics;

namespace Search2.Static
{
    public class StopwatchOperation : IDisposable
    {
        public string Name { get; set; }
        protected Stopwatch InnerStopwatch { get; set; }

        public StopwatchOperation(string name)
        {
            InnerStopwatch = new Stopwatch();
            Name = name;

            InnerStopwatch.Start();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (InnerStopwatch != null)
                {
                    InnerStopwatch.Stop();

                    Trace.WriteLine($"Duration of '{Name}' is {InnerStopwatch.ElapsedMilliseconds} milliseconds.");

                    InnerStopwatch = null;
                }
            }
        }
        #endregion
    }
}
