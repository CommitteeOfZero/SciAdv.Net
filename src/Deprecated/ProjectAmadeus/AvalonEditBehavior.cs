using ICSharpCode.AvalonEdit;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace ProjectAmadeus
{
    public sealed class AvalonEditBehavior : Behavior<TextEditor>
    {
        public static readonly DependencyProperty BindableTextProperty = DependencyProperty.Register("BindableText", typeof(string),
            typeof(AvalonEditBehavior), new PropertyMetadata(OnBindableTextPropertyChanged));

        private static void OnBindableTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as AvalonEditBehavior;
            if (behavior.AssociatedObject != null)
            {
                if (behavior.AssociatedObject.Text != (string)e.NewValue)
                {
                    behavior.AssociatedObject.Text = (string)e.NewValue;
                }
            }
        }

        public string BindableText
        {
            get => (string)GetValue(BindableTextProperty);
            set => SetValue(BindableTextProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            BindableText = AssociatedObject.Text;
        }
    }
}
