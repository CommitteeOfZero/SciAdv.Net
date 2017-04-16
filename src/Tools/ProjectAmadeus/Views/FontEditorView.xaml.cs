using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectAmadeus.Views
{
    /// <summary>
    /// Interaction logic for FontEditorView.xaml
    /// </summary>
    public partial class FontEditorView : UserControl
    {
        public FontEditorView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            foreach (var fontFamily in Fonts.SystemFontFamilies)
            {
                SystemFonts.Items.Add(fontFamily.Source);
            }
        }
    }
}
