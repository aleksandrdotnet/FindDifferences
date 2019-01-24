using Search2.Util;
using Search2.ViewModels;

namespace Search2.Views
{
    public partial class MainView
    { 
        public MainView()
        {
            InitializeComponent();

            Logger.InitLogger();

            DataContext = new MainViewModel();
        }
    }
}
