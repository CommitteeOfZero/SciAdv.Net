using Caliburn.Micro;
using ProjectAmadeus.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using ProjectAmadeus.Services;

namespace ProjectAmadeus
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            if (!Execute.InDesignMode)
            {
                MessageBinder.SpecialValues.Add("$droppedfiles", context
                    => ((DragEventArgs)(context?.EventArgs))?.Data?.GetData(DataFormats.FileDrop) as IEnumerable<string>);
            }

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IFilePicker, FilePicker>();
            _container.Singleton<ShellViewModel>();
            _container.PerRequest<ITab, TabViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }
    }
}
