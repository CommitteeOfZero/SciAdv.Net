using System.Collections.Generic;

namespace ProjectAmadeus.Services
{
    public interface IFilePicker
    {
        string PickFolder();
        string PickOpen(IEnumerable<FileDialogFilter> filters);
        string PickSave(IEnumerable<FileDialogFilter> filters);
        string PickOpen(FileDialogFilter filter);
        string PickSave(FileDialogFilter filter);
    }

    public sealed class FileDialogFilter
    {
        public FileDialogFilter(string displayName, IEnumerable<string> extensions)
        {
            DisplayName = displayName;
            Extensions = extensions;
        }

        public string DisplayName { get; }
        public IEnumerable<string> Extensions { get; }
    }
}
