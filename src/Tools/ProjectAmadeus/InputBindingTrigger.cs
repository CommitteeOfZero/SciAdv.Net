using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ProjectAmadeus
{
    public sealed class InputBindingTrigger : TriggerBase<FrameworkElement>, ICommand
    {
        public static readonly DependencyProperty InputBindingProperty = DependencyProperty.Register("InputBinding",
            typeof(InputBinding), typeof(InputBindingTrigger), new UIPropertyMetadata(null));

        public InputBinding InputBinding
        {
            get { return (InputBinding)GetValue(InputBindingProperty); }
            set { SetValue(InputBindingProperty, value); }
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            InvokeActions(parameter);
        }

        protected override void OnAttached()
        {
            if (InputBinding != null)
            {
                InputBinding.Command = this;
                AssociatedObject.Loaded += (o, a) =>
                {
                    var window = GetWindow(AssociatedObject);
                    window.InputBindings.Add(InputBinding);
                };
            }
            base.OnAttached();
        }

        private Window GetWindow(FrameworkElement frameworkElement)
        {
            if (frameworkElement is Window)
            {
                return frameworkElement as Window;
            }

            var parent = frameworkElement.Parent as FrameworkElement;
            return GetWindow(parent);
        }

        private void RaiseExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
