using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Search2.ViewModels;

namespace Search2
{
    public class MainBootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public MainBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Instance(_container);
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IWindowManager, WindowManager>();
            _container.PerRequest<MainViewModel>();

        }
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }
    }
}
