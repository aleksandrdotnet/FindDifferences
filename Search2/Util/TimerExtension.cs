using System.Timers;

namespace Search2.Util
{
    public static class TimerExtension
    {
        /// <summary>
        /// Reset System.Timers.Timer
        /// </summary>
        /// <param name="timer"></param>
        public static void Reset(this Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}