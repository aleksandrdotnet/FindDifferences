using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Search2.Model;

namespace Search2.Static
{
    public static class WorkScreen
    {
        [DllImport("user32.dll")]
        static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
        }
        
        public static void SaveImageFromScreen(Point startPoint, int width, int height)
        {
            var printscreen = new Bitmap(width - 4, height - 4);

            var graphics = Graphics.FromImage(printscreen);

            graphics.CopyFromScreen(startPoint.X + 2, startPoint.Y + 2, 0, 0, printscreen.Size);

            printscreen.Save($"{DateTime.Now:dd.MM.yyyy HH-mm-ss}.png",ImageFormat.Png);
        }
        public static void SaveImage(Bitmap bitmap)
        {
            bitmap.Save($"{DateTime.Now:dd.MM.yyyy HH-mm-ss}.png", ImageFormat.Png);
        }

        public static Bitmap GetImageFromProcess(Process process)
        {
            try
            {
                var hwnd = process.MainWindowHandle;
                if (hwnd.ToInt32() != 0)
                {
                    GetWindowRect(hwnd, out var rect);
                    using (var image = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top))
                    {
                        using (var graphics = Graphics.FromImage(image))
                        {
                            var hdcBitmap = graphics.GetHdc();
                            PrintWindow(hwnd, hdcBitmap, 0);
                            graphics.ReleaseHdc(hdcBitmap);
                        }

                        return image;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Bitmap GetImageFromProcessName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            IntPtr handle = FindWindow(null, name);

            GetWindowRect(handle, out var rect);
            Rectangle gameScreenRect = new Rectangle(rect.Left + 2, rect.Top + 2, rect.Right - rect.Left-4, rect.Bottom - rect.Top - 4);

            Bitmap gameBmp = GetImageFromScreenRectange(gameScreenRect);

            return gameBmp;
        }
        public static Bitmap GetImageFromScreenRectange(Point startPoint, int width, int height)
        {
            var printscreen = new Bitmap(width-4, height - 4);

            var graphics = Graphics.FromImage(printscreen);

            graphics.CopyFromScreen(startPoint.X+2, startPoint.Y+2, 0, 0, printscreen.Size);

            return printscreen;
        }
        public static ImageRectangleModel[,] GetMatrix(Bitmap bm, byte procent = 4, ushort pixel = 10)
        {
            if (procent >= 50)
                throw new ArgumentOutOfRangeException(nameof(procent));

            int frameHeight;
            int rank0;
            if (bm.Height < pixel)
            {
                frameHeight = bm.Height;
                rank0 = 1;
            }
            else if (bm.Height * procent / 100 < pixel)
            {
                frameHeight = pixel;
                rank0 = bm.Height / frameHeight;
            }
            else
            {
                frameHeight = bm.Height * procent / 100;
                rank0 = bm.Height / frameHeight;
            }

            int frameWidth;
            int rank1;
            if (bm.Width < pixel)
            {
                frameWidth = bm.Width;
                rank1 = 1;
            }
            else if (bm.Width * procent / 100 < pixel)
            {
                frameWidth = pixel;
                rank1 = bm.Width / frameWidth;
            }
            else
            {
                frameWidth = bm.Width * procent / 100;
                rank1 = bm.Width / frameWidth;
            }

            int h = frameHeight;
            int w = frameWidth;

            var arr = new ImageRectangleModel[rank0, rank1];
            for (var i = 0; i < arr.GetLength(0); i++)
            {
                if (i == (arr.GetLength(0) - 1))
                {
                    h = bm.Height - i * frameHeight;
                }

                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    if (j == arr.GetLength(1) - 1)
                    {
                        w = bm.Width - j * frameWidth;
                    }

                    var point = new Point(j * frameWidth, i * frameHeight);

                    try
                    {
                        arr[i, j] = new ImageRectangleModel { LeftTop = point };
                        var img = bm.Clone(new Rectangle(point.X, point.Y, w, h), bm.PixelFormat);
                        arr[i, j].Image = WorkScreen.Bitmap2BitmapImage(img);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"W={bm.Width} H={bm.Height}\n" +
                                        $"X={point.X} Y={point.Y}\n" +
                                        $"ww={w} hh={h}", ex.Message);
                    }
                }
            }

            return arr;
        }

        public static RectangleModel[,] GetMatrixRectangle(Bitmap bm, int procent = 4, ushort pixel = 10)
        {
            if (procent >= 50)
                throw new ArgumentOutOfRangeException(nameof(procent));

            int frameHeight;
            int rank0;
            if (bm.Height < pixel)
            {
                frameHeight = bm.Height;
                rank0 = 1;
            }
            else if (bm.Height * procent / 100 < pixel)
            {
                frameHeight = pixel;
                rank0 = bm.Height / frameHeight;
            }
            else
            {
                frameHeight = bm.Height * procent / 100;
                rank0 = bm.Height / frameHeight;
            }

            int frameWidth;
            int rank1;
            if (bm.Width < pixel)
            {
                frameWidth = bm.Width;
                rank1 = 1; 
            }
            else if (bm.Width * procent / 100 < pixel)
            {
                frameWidth = pixel;
                rank1 = bm.Width / frameWidth;
            }
            else
            {
                frameWidth = bm.Width * procent / 100;
                rank1 = bm.Width / frameWidth;
            }

            var arr = new RectangleModel[rank0, rank1];

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                var h = i * frameHeight;

                if (i >= arr.GetLength(0))
                {
                    frameHeight = bm.Height - h;
                }

                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    var w = j * frameWidth;

                    if (j >= arr.GetLength(1))
                    {
                        frameWidth = bm.Width - w;
                    }

                    arr[i, j] = new RectangleModel()
                    {
                        LeftTop = new Point(w, h)
                    };
                }
            }

            return arr;
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
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

        public static bool Comparer(Bitmap source, Bitmap template, double threshold)
        {
            var fst = new Image<Bgr, byte>(source);
            var scd = new Image<Bgr, byte>(template);
            var imageToShow = fst.Copy();

            using (var result = fst.MatchTemplate(scd, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out _, out var maxValues, out _, out var maxLocations);

                if (maxValues[0] > threshold)
                {
                    return true;
                }
                return false;
            }
        }

        public static Bitmap GetImageFromScreenRectange(Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width-4, rect.Height - 4, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(rect.Left+2, rect.Top + 2, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }

        public static BitmapImage Bitmap2BitmapImage(Bitmap src)
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
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Image<Bgr, Byte> ConvertBitmapToImageBgrByte(Bitmap src)
        {
            return new Image<Bgr, byte>(src);
        }
    }
}