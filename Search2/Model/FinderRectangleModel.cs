using System;
using System.Drawing;
using System.Windows.Media;
using Search2.Static;

namespace Search2.Model
{
    public class FinderRectangleModel : RectangleModel
    {
        private Point _startPoint;
        public Point StartPoint
        {
            get => _startPoint;
            set
            {
                if (_startPoint != value)
                {
                    _startPoint = value;

                    LeftTop = _startPoint;
                    NextTop = LeftTop.Y + Height;

                    NotifyOfPropertyChange(() => StartPoint);
                }
            }
        }

        private Point _endPoint;
        public Point EndPoint
        {
            get => _endPoint;
            set
            {
                if (_endPoint != value)
                {
                    _endPoint = value;

                    LeftTop = new Point(
                        Math.Min(_startPoint.X, _endPoint.X),
                        Math.Min(_startPoint.Y, _endPoint.Y));

                    Width = Math.Abs(_endPoint.X - _startPoint.X);
                    Height = Math.Abs(_endPoint.Y - _startPoint.Y);

                    NextTop = LeftTop.Y + Height;

                    NotifyOfPropertyChange(() => EndPoint);
                }
            }
        }

        private int _nextTop;
        public int NextTop
        {
            get => _nextTop;
            private set
            {
                if (_nextTop != value)
                {
                    _nextTop = value;
                    NotifyOfPropertyChange(() => NextTop);
                }
            }
        }

        private SolidColorBrush _color = new SolidColorBrush(Colors.Red);

        public SolidColorBrush Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    NotifyOfPropertyChange(() => Color);
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            
            _startPoint = new Point(0, 0);
            _endPoint = new Point(0, 0);
        }
    }
}