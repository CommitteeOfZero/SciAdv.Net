using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System.Windows;

namespace ProjectAmadeus
{
    public class AvalonEditBindingHelper : DependencyObject
    {
        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached("BindableText", typeof(string), typeof(AvalonEditBindingHelper),
                new PropertyMetadata(HandleTextChange));

        public static string GetBindableText(DependencyObject obj) => (string)obj.GetValue(BindableTextProperty);
        public static void SetBindableText(DependencyObject obj, string value) => obj.SetValue(BindableTextProperty, value);

        private static void HandleTextChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as TextEditor;
            if (control.Document == null)
            {
                control.Document = new TextDocument(e.NewValue?.ToString());
            }
            else
            {
                control.Document.Text = e.NewValue?.ToString();
            }
        }
    }
}
