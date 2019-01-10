using System.Windows.Media.Imaging;

namespace Search2.Model
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
    }
}