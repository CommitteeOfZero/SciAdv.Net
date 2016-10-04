using Microsoft.Win32;

namespace ProjectAmadeus.Services
{
    public sealed class FilePicker : IFilePicker
    {
        public string PickFile(string extensions, string description)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = $"{description}|{extensions}";

            dialog.ShowDialog();
            return dialog.FileName;
        }
    }
}
