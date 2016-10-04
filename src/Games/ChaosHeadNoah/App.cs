using SciAdvNet.Engine;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System;

namespace ChaosHeadNoah
{
    public sealed class App : IFrameworkView
    {
        private Noah _noah;

        public static void Main()
        {
            CoreApplication.Run(new ViewSource());
        }

        public void Initialize(CoreApplicationView applicationView)
        {
            applicationView.Activated += ApplicationView_Activated;
            CoreApplication.Suspending += CoreApplication_Suspending;
        }

        private void ApplicationView_Activated(CoreApplicationView sender, IActivatedEventArgs args)
        {
            CoreWindow.GetForCurrentThread().Activate();
        }

        private void CoreApplication_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            _noah.Dispose();
        }

        public void Load(string entryPoint)
        {
            var coreWindow = CoreWindow.GetForCurrentThread();
            Action processEventsFunc = () => coreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);
            var gameWindow = new GameWindow(coreWindow, processEventsFunc);

            _noah = new Noah(Platform.WindowsUniversal, gameWindow);
        }

        public void Run()
        {
            _noah.Run();
        }

        public void SetWindow(CoreWindow window)
        {

        }

        public void Uninitialize()
        {

        }
    }

    public sealed class ViewSource : IFrameworkViewSource
    {
        public IFrameworkView CreateView()
        {
            return new App();
        }
    }
}
