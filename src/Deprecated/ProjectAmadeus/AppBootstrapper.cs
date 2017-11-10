using Caliburn.Micro;
using ProjectAmadeus.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using ProjectAmadeus.Services;
using ProjectAmadeus.Models;

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
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IFilePicker, FilePicker>();
            _container.Singleton<ShellViewModel>();
            _container.Singleton<NotificationAreaViewModel>();

            _container.PerRequest<DocumentViewModel>();
            _container.PerRequest<StringTableViewModel>();
            _container.PerRequest<CodeEditorViewModel>();
            _container.PerRequest<FontEditorViewModel>();

            _container.Singleton<Workspace>();
            _container.Singleton<SharedData>();
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
