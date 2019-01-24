using System.Windows;
using Caliburn.Micro;
using Search2.ViewModels;

namespace Search2
{
    public class MainBootstrapper : BootstrapperBase
    {
        public MainBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
