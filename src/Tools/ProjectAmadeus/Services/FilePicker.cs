using Microsoft.Win32;

namespace ProjectAmadeus.Services
{
    public sealed class FilePicker : IFilePicker
    {
        public string PickOpen(string extensions, string description)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = $"{description}|{extensions}";

            dialog.ShowDialog();
            return dialog.FileName;
        }

        public string PickSave(string extensions, string description)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = $"{description}|{extensions}";

            dialog.ShowDialog();
            return dialog.FileName;
        }
    }
}
