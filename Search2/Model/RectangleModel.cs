using System.Drawing;
using System.Windows.Shapes;
using Caliburn.Micro;
using Brushes = System.Windows.Media.Brushes;

namespace Search2.Model
{
    public class RectangleModel : PropertyChangedBase
    {
        private Point _leftTop;
        public Point LeftTop
        {
            get => _leftTop;
            set
            {
                if (_leftTop != value)
                {
                    _leftTop = value;
                    NotifyOfPropertyChange(() => LeftTop);
                }
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    NotifyOfPropertyChange(() => Height);
                }
            }
        }

        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    NotifyOfPropertyChange(() => Width);
                }
            }
        }
        
        public virtual void Clear()
        {
            LeftTop = new Point(0, 0);
            Height = 0;
            Width = 0;
        }
    }
}