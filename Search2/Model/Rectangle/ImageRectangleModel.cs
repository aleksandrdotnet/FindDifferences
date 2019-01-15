using System.Drawing;
using System.Windows.Media.Imaging;

namespace Search2.Model.Rectangle
{
    public class ImageRectangleModel : RectangleModel
    {
        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _image;
            set
            {
                if (!Equals(_image, value))
                {
                    _image = value;
                    NotifyOfPropertyChange(() => Image);
                }
            }
        }

        public ImageRectangleModel(Point leftTop, int height, int width) : base(leftTop, height, width)
        {
        }
    }
}