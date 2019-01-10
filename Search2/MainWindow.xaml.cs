using Search2.ViewModel;

namespace Search2
{
    public partial class MainWindow
    { 
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
