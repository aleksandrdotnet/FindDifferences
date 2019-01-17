using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Search2.Util
{
    public static class ImageEx
    {
        public static BitmapSource ToBitmapSource(this Bitmap src)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                src.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
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

        public static Bitmap ToBitmap(this BitmapImage src)
        {
            using (var outStream = new MemoryStream())
            {
                var enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(src));
                enc.Save(outStream);
                var bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        public static Image<Bgr, Byte> ToImageBgrByte(this Bitmap src)
        {
            return new Image<Bgr, byte>(src);
        }

        public static Image<Gray, Byte> ToImageGrayByte(this Bitmap src)
        {
            return new Image<Gray, byte>(src);
        }

        public static void SaveImage(this Bitmap src)
        {
            src.Save($"{DateTime.Now:dd.MM.yyyy HH-mm-ss}.png", ImageFormat.Png);
        }
    }
}