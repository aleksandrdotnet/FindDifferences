using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Search2.Static
{
    public static class ImageEx
    {
        public static BitmapImage ToBitmapImage(this Bitmap src)
        {
            try
            {
                var ms = new MemoryStream();
                src.Save(ms, ImageFormat.Png);
                var image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"{nameof(ToBitmap)}", ex);
                return null;
            }
        }

        public static Bitmap ToBitmap(this BitmapImage bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                var enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                var bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        public static Image<Bgr, Byte> ToImageBgrByte(this Bitmap src)
        {
            return new Image<Bgr, byte>(src);
        }

        public static void SaveImage(this Bitmap bitmap)
        {
            bitmap.Save($"{DateTime.Now:dd.MM.yyyy HH-mm-ss}.png", ImageFormat.Png);
        }
    }
}