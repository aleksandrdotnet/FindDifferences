using System.Drawing;
using System.Windows.Media;

namespace Search2.Model.Rectangle
{
    public class AreaRectangleModel : RectangleModel
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
                    
                    SetRectangle(_startPoint, _endPoint);

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

        private SolidColorBrush _color = new SolidColorBrush(Colors.DarkRed);

        public SolidColorBrush Color
        {
            get => _color;
            set
            {
                if (!Equals(_color, value))
                {
                    _color = value;
                    NotifyOfPropertyChange(() => Color);
                }
            }
        }

        protected override void Clear()
        {
            base.Clear();
            
            _startPoint = new Point(0, 0);
            _endPoint = new Point(0, 0);
        }

        public AreaRectangleModel(Point leftTop, int height, int width) : base(leftTop, height, width)
        {
        }
    }
}