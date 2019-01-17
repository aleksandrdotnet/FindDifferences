using System;
using System.Diagnostics;

namespace Search2.Static
{
    public static class StopwatchExtension
    {
        public static TimeSpan GetElapsedTime(this Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            return sw.Elapsed;
        }
    }
}
