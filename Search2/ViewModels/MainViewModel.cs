using System;
using Caliburn.Micro;
using Application = System.Windows.Application;

namespace Search2.ViewModels
{
    public class MainViewModel : Screen
    {
        private FinderViewModel _finder = new FinderViewModel();
        public FinderViewModel Finder
        {
            get => _finder;
            set
            {
                if (_finder != value)
                {
                    _finder = value;
                    NotifyOfPropertyChange(() => Finder);
                }
            }
        }
       
        public void Close(object sender, EventArgs e)
        {
            foreach (var l in Finder.Elements)
            {
                l.Dispose();
            }

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();
        }
    }
}