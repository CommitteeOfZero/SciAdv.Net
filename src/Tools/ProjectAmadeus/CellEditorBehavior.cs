using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ProjectAmadeus
{
    public sealed class CellEditorBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += TextChanged;
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Focus();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= TextChanged;
            AssociatedObject.Loaded -= OnLoaded;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
