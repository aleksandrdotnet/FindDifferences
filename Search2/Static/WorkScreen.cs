using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Search2.Model.Rectangle;

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

        public static void SaveFromScreen(RectangleModel rectangle)
        {
            if (rectangle.Height < 5)
                throw new ArgumentOutOfRangeException(nameof(rectangle.Height));
            if (rectangle.Width < 5)
                throw new ArgumentOutOfRangeException(nameof(rectangle.Width));

            var printscreen = new Bitmap(rectangle.Width - 4, rectangle.Height - 4);

            var graphics = Graphics.FromImage(printscreen);

            graphics.CopyFromScreen(rectangle.LeftTop.X + 2, rectangle.LeftTop.Y + 2, 0, 0, printscreen.Size);

            printscreen.Save($"{DateTime.Now:dd.MM.yyyy HH-mm-ss}.png", ImageFormat.Png);
        }

        public static Bitmap GetBitmapFromProcess(Process process)
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
            catch (Exception ex)
            {
                Logger.Log.Error($"{nameof(GetBitmapFromProcess)}", ex);
                return null;
            }
        }

        public static Bitmap GetBitmapFromProcess(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            IntPtr handle = FindWindow(null, name);

            GetWindowRect(handle, out var rect);
            var gameScreenRect = new RectangleModel(leftTop: new Point(rect.Left + 2, rect.Top + 2),
                height: rect.Bottom - rect.Top - 4, width: rect.Right - rect.Left - 4);

            var gameBmp = GetBitmapFromScreen(gameScreenRect);

            return gameBmp;
        }

        public static Bitmap GetBitmapFromScreen(RectangleModel rectangle)
        {
            if (rectangle.Height < 5)
                throw new ArgumentOutOfRangeException(nameof(rectangle.Height));
            if (rectangle.Width < 5)
                throw new ArgumentOutOfRangeException(nameof(rectangle.Width));

            var printscreen = new Bitmap(rectangle.Width - 4, rectangle.Height - 4);

            using (var graphics = Graphics.FromImage(printscreen))
            {
                graphics.CopyFromScreen(rectangle.LeftTop.X + 2, rectangle.LeftTop.Y + 2, 0, 0, printscreen.Size,
                    CopyPixelOperation.SourceCopy);
            }

            return printscreen;
        }

        public static RectangleModel<Bitmap>[,] GetMatrix(Bitmap bm, byte procentHeight = 4, byte procentWidth = 4, ushort pixelHeight = 10,
            ushort pixelWidth = 10)
        {
            if (procentHeight >= 50)
                throw new ArgumentOutOfRangeException(nameof(procentHeight));
            if (procentWidth >= 50)
                throw new ArgumentOutOfRangeException(nameof(procentWidth));

            int frameHeight;
            int rank0;
            if (bm.Height < pixelHeight)
            {
                frameHeight = bm.Height;
                rank0 = 1;
            }
            else if (bm.Height * procentHeight / 100 < pixelHeight)
            {
                frameHeight = pixelHeight;
                rank0 = bm.Height / frameHeight;
            }
            else
            {
                frameHeight = bm.Height * procentHeight / 100;
                rank0 = bm.Height / frameHeight;
            }

            int frameWidth;
            int rank1;
            if (bm.Width < pixelWidth)
            {
                frameWidth = bm.Width;
                rank1 = 1;
            }
            else if (bm.Width * procentWidth / 100 < pixelWidth)
            {
                frameWidth = pixelWidth;
                rank1 = bm.Width / frameWidth;
            }
            else
            {
                frameWidth = bm.Width * procentWidth / 100;
                rank1 = bm.Width / frameWidth;
            }

            var arr = new RectangleModel<Bitmap>[rank0, rank1];
            for (var i = 0; i < arr.GetLength(0); i++)
            {
                int h;
                if (i == (arr.GetLength(0) - 1))
                {
                    h = bm.Height - i * frameHeight;
                }
                else
                {
                    h = frameHeight;
                }

                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    int w;
                    if (j == arr.GetLength(1) - 1)
                    {
                        w = bm.Width - j * frameWidth;
                    }
                    else
                    {
                        w = frameWidth;
                    }
                    
                    try
                    {
                        var point = new Point(j * frameWidth, i * frameHeight);
                        arr[i, j] = new RectangleModel<Bitmap>(point, h, w);
                        var img = bm.Clone(new Rectangle(point.X, point.Y, w, h), bm.PixelFormat);
                        arr[i, j].SetImage(img);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error($"{nameof(GetMatrix)}", ex);
                    }
                }
            }

            return arr;
        }

    }
}