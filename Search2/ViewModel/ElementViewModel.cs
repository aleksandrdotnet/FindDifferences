using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;
using Search2.Hook;
using Search2.Model.Rectangle;
using Search2.Static;
using Point = System.Drawing.Point;

namespace Search2.ViewModel
{
    public sealed class ElementViewModel : PropertyChangedBase
    {
        private AreaRectangleModel _area = new AreaRectangleModel(new Point(), 0, 0);
        public AreaRectangleModel Area
        {
            get => _area;
            private set
            {
                if (_area != value)
                {
                    _area = value;
                    NotifyOfPropertyChange(() => Area);
                }
            }
        }

        private ObservableCollection<RectangleModel> _serachMatrix = new ObservableCollection<RectangleModel>();
        public ObservableCollection<RectangleModel> SearchMatrix
        {
            get => _serachMatrix;
            set
            {
                if (!Equals(_serachMatrix, value))
                {
                    _serachMatrix = value;
                    NotifyOfPropertyChange(() => SearchMatrix);
                }
            }
        }

        #region Command
        public ICommand ScissorCommand => new RelayCommand(Scissors);

        public void Scissors(object sender)
        {
            if (IsChecked)
            {
                MouseHook.Start();
                MouseHook.MouseLeftButton += MouseHook_MouseLeftButton;
                MouseHook.MouseLeftMove += MouseHook_MouseLeftMove;
            }
        }
        #endregion

        #region Mouse
        private bool _isPressed;

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

        private bool _isExist;

        public bool IsExist
        {
            get => _isExist;
            set
            {
                if (_isExist != value)
                {
                    _isExist = value;
                    NotifyOfPropertyChange(() => IsExist);
                }
            }
        }

        private void MouseHook_MouseLeftButton(object sender, Tuple<MouseButtonState, Point> e)
        {
            switch (e.Item1)
            {
                case MouseButtonState.Pressed:
                    SearchMatrix.Clear();
                    Area.StartPoint = e.Item2;
                    Area.EndPoint = e.Item2;

                    _isPressed = true;
                    break;

                case MouseButtonState.Released:
                    MouseHook.Stop();
                    MouseHook.MouseLeftButton -= MouseHook_MouseLeftButton;
                    MouseHook.MouseLeftMove -= MouseHook_MouseLeftMove;
                    
                    _isPressed = false;
                    IsChecked = false;
                    IsExist = true;
                    break;
            }
        }

        private void MouseHook_MouseLeftMove(object sender, Point e)
        {
            if (IsChecked && _isPressed)
            {
                Area.EndPoint = e;
            }
        }
        #endregion

        public ElementViewModel(Color color)
        {
            Area.Color = new SolidColorBrush(color);
        }

        public ElementViewModel() : this(Colors.DarkRed)
        {
            
        }
        
        public void Dispose()
        {
            if (IsChecked)
            {
                MouseHook.Stop();
                MouseHook.MouseLeftButton -= MouseHook_MouseLeftButton;
                MouseHook.MouseLeftMove -= MouseHook_MouseLeftMove;
            }
        }
    }
}