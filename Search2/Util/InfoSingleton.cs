using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Search2.Util
{
    internal sealed class InfoSingleton
    {
        private static readonly object Sync = new object();
        private static volatile InfoSingleton _instance;
        private readonly SolidColorBrush[] _colors =
        {
            Brushes.DarkRed,
            Brushes.Orange,
            Brushes.YellowGreen,
            Brushes.DarkGreen,
            Brushes.LightBlue,
            Brushes.DarkBlue,
            Brushes.Purple,
            Brushes.Black,
        };

        public IEnumerable<Brush> Fill => _colors;

        public DateTime Time;

        public static InfoSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new InfoSingleton();
                        }
                    }
                }
                
                return _instance;
            }
        }

        private InfoSingleton()
        {
            Time = DateTime.Now;
        }
    }
}
