using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ProjectAmadeus.Controls
{
    /// <summary>
    /// Interaction logic for TwitterButton.xaml
    /// </summary>
    public partial class TwitterButton : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TwitterButton), new PropertyMetadata("Tweet us"));


        public TwitterButton()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        private void Hyperlink_RequestNavigate(Object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}
