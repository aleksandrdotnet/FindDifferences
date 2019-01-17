using Search2.Util;
using Search2.ViewModel;

namespace Search2
{
    public partial class MainWindow
    { 
        public MainWindow()
        {
            InitializeComponent();

            Logger.InitLogger();

            DataContext = new MainViewModel();
        }
    }
}
