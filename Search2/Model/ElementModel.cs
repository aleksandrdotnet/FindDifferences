using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Search2.Hook;
using Search2.Static;
using Point = System.Drawing.Point;

namespace Search2.Model
{
    public sealed class ElementModel :PropertyChangedBase,IDisposable
    {
        private double _threshold = 0.98;
        public double Threshold 
        {
            get => _threshold;
            set
            {
                if (Math.Abs(_threshold - value) > 0.000001f)
                {
                    _threshold = value;
                    NotifyOfPropertyChange(() => Threshold );
                }
            }
        }

        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    NotifyOfPropertyChange(() => Progress);
                }
            }
        }

        private ObservableCollection<ImageRectangleModel> _collection = new ObservableCollection<ImageRectangleModel>();
        public ObservableCollection<ImageRectangleModel> Collection
        {
            get => _collection;
            set
            {
                if (_collection != value)
                {
                    _collection = value;
                    NotifyOfPropertyChange(() => Collection);
                }
            }
        }

        private FinderRectangleModel _finderRectangle;
        public FinderRectangleModel FinderRectangle
        {
            get => _finderRectangle;
            set
            {
                if (_finderRectangle != value)
                {
                    _finderRectangle = value;
                    NotifyOfPropertyChange(() => FinderRectangle);
                }
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        
        private bool _isPressed;

        private void SetStartPoint(Point point)
        {
            FinderRectangle = new FinderRectangleModel { StartPoint = point};
        }

        private void SetEndPoint(Point point)
        {
            FinderRectangle.EndPoint = point;
        }
        
        public void Crop(object sender, EventArgs e)
        {
            if (IsChecked)
            {
                Clear();

                MouseHook.Start();
                MouseHook.MouseLeftButton += MouseHook_MouseLeftButton;
                MouseHook.MouseLeftMove += MouseHook_MouseLeftMove;
            }
        }

        public async void Find(object sender, EventArgs e)
        {
            //var bm = WorkScreen.GetImageFromScreenRectange(FinderRectangle.LeftTop, FinderRectangle.Width, FinderRectangle.Height);
            //var matrix = WorkScreen.GetMatrix(bm, 4, 10);

            Collection.Clear();

            //for (int i = 0; i < matrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < matrix.GetLength(1); j++)
            //    {
            //        Collection.Add(matrix[i, j]);
            //    }
            //}

            var progress = new Progress<int>();
            progress.ProgressChanged += (s, proc) =>
            {
                // This callback will run on the thread which
                // created the Progress<int> instance.
                // You can update your UI here.
                Progress = proc;
            };
            await Task.Factory.StartNew(() => Checker(progress), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Checker(IProgress<int> progress)
        {
            //var matrix1 = WorkScreen.GetMatrix(new Bitmap("1.png"), 10, 25);
            //var matrix2 = WorkScreen.GetMatrix(new Bitmap("2.png"), 10, 25);

            //for (int i = 0; i < matrix1.GetLength(0); i++)
            //{
            //    for (int j = 0; j < matrix1.GetLength(1); j++)
            //    {
            //        if (WorkScreen.Comparer(new Bitmap(matrix1[i, j].Image.StreamSource),new Bitmap(matrix2[i, j].Image.StreamSource),Threshold))
            //            Collection.Add(matrix1[i, j]);

            //        // progress.Report(((i * j + j) / (matrix1.GetLength(0) * matrix1.GetLength(1)) + 1) * 100);
            //    }
            //}

            var matrix1 = WorkScreen.GetMatrix(new Bitmap("1.png"), 10, 25);
            var matrix2 = WorkScreen.GetMatrix(new Bitmap("2.png"), 4, 10);

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix2.GetLength(1); k++)
                    {
                        if (WorkScreen.Comparer(new Bitmap(matrix1[i, j].Image.StreamSource),
                            new Bitmap(matrix2[i, k].Image.StreamSource), Threshold))
                        {
                            Collection.Add(matrix1[i, j]);
                        }
                    }
                }
            }
        }

        public void Save(object sender, EventArgs e)
        {
            WorkScreen.SaveImageFromScreen(
                FinderRectangle.LeftTop,
                FinderRectangle.Width,
                FinderRectangle.Height);
        }

        private void MouseHook_MouseLeftMove(object sender, Point e)
        {
            if (IsChecked && _isPressed)
            {
                SetEndPoint(e);
            }
        }

        private void MouseHook_MouseLeftButton(object sender, Tuple<MouseButtonState, Point> e)
        {
            switch (e.Item1)
            {
                case MouseButtonState.Pressed:
                    SetStartPoint(e.Item2);

                    _isPressed = true;
                    break;

                case MouseButtonState.Released:
                    MouseHook.Stop();
                    MouseHook.MouseLeftButton -= MouseHook_MouseLeftButton;
                    MouseHook.MouseLeftMove -= MouseHook_MouseLeftMove;

                    _isPressed = false;
                    IsChecked = false;
                    break;
            }
        }

        public void Dispose()
        {
            if (IsChecked)
            {
                MouseHook.Stop();
                MouseHook.MouseLeftButton -= MouseHook_MouseLeftButton;
                MouseHook.MouseLeftMove -= MouseHook_MouseLeftMove;
            }

            Clear();
        }

        private void Clear()
        {
            Collection.Clear();

            FinderRectangle = null;
        }
    }
}