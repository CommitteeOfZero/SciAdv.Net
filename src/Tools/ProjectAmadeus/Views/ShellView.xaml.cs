using MahApps.Metro.Controls;
using System.Windows;

namespace ProjectAmadeus.Views
{
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(System.Object sender, RoutedEventArgs e)
        {
            //ShowMessage(new UnsupportedScript());
        }

        private void ShowMessage(object content)
        {
            var flyout = new Flyout()
            {
                Position = Position.Top,
                Content = content,
                TitleVisibility = Visibility.Collapsed,
                CloseButtonVisibility = Visibility.Collapsed,
                Opacity = 0.95f
            };

            MainRegion.Children.Add(flyout);
            flyout.IsOpen = true;
        }
    }
}
