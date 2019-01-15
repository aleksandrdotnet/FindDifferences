using System;
using System.Drawing;
using Caliburn.Micro;

namespace Search2.Model.Rectangle
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

        public RectangleModel(Point leftTop, int height, int width)
        {
            LeftTop = leftTop;
            Height = height;
            Width = width;
        }

        public void SetRectangle(Point fst, Point snd)
        {
            LeftTop = new Point(
                Math.Min(fst.X, snd.X),
                Math.Min(fst.Y, snd.Y));

            Width = Math.Abs(snd.X - fst.X);
            Height = Math.Abs(snd.Y - fst.Y);
        }

        protected virtual void Clear()
        {
            SetRectangle(new Point(0, 0), new Point(0, 0));
        }

        public override string ToString()
        {
            return $"({LeftTop.X},{LeftTop.Y}) ({LeftTop.X+ Width},{LeftTop.Y+ Height})";
        }
    }
}