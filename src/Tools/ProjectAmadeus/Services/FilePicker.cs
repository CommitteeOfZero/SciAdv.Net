using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Linq;

namespace ProjectAmadeus.Services
{
    public sealed class FilePicker : IFilePicker
    {
        public string PickFolder()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                return dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : string.Empty;
            }
        }

        public string PickOpen(FileDialogFilter filter) => PickOpen(new[] { filter });
        public string PickSave(FileDialogFilter filter) => PickSave(new[] { filter });

        public string PickOpen(IEnumerable<FileDialogFilter> filters)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                foreach (var winApiFilter in WinApiFilters(filters))
                {
                    dialog.Filters.Add(winApiFilter);
                }

                return dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : string.Empty;
            }
        }

        public string PickSave(IEnumerable<FileDialogFilter> filters)
        {
            using (var dialog = new CommonSaveFileDialog())
            {
                foreach (var winApiFilter in WinApiFilters(filters))
                {
                    dialog.Filters.Add(winApiFilter);
                }

                return dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : string.Empty;
            }
        }

        private static IEnumerable<CommonFileDialogFilter> WinApiFilters(IEnumerable<FileDialogFilter> filters)
        {
            return from f in filters
                   let extString = string.Join(";", f.Extensions)
                   select new CommonFileDialogFilter(f.DisplayName, extString);

        }
    }
}
