using Caliburn.Micro;
using ProjectAmadeus.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace ProjectAmadeus.ViewModels
{
    public sealed class ShellViewModel : Conductor<ITab>.Collection.OneActive
    {
        private const string ScriptExtensions = "*.scx;*.scr";
        private const string FileTypeDescription = "SciADV game script";

        private readonly Func<ITab> _tabFactory;
        private readonly IFilePicker _filePicker;

        public ShellViewModel()
        {
        }

        public ShellViewModel(Func<ITab> tabFactory, IFilePicker filePicker)
        {
            _tabFactory = tabFactory;
            _filePicker = filePicker;

            DisplayName = "Project Amadeus";
        }

        public bool CanCloseFile => ActiveItem != null;

        public void OpenFile()
        {
            string path = _filePicker.PickOpen(ScriptExtensions, FileTypeDescription);
            if (!string.IsNullOrEmpty(path))
            {
                var tab = CreateTab(path);
                ActivateItem(tab);
                NotifyOfPropertyChange(nameof(CanCloseFile));
            }
        }

        public void HandleFileDrop(IEnumerable<string> files)
        {
            foreach (string path in files)
            {
                var tab = CreateTab(path);
                if (ActiveItem == null)
                {
                    ActivateItem(tab);
                }
            }
        }

        public void SaveAs()
        {
            string path = _filePicker.PickSave("*.txt", "Text files");
            if (!string.IsNullOrEmpty(path))
            {
                var sb = new StringBuilder();

                var active = ActiveItem;
                active.Module.ApplyPendingUpdates();

                foreach (var stringHandle in active.Module.StringTable)
                {
                    sb.Append($"[{stringHandle.Id}]");
                    sb.Append(stringHandle.Resolve().ToString(normalize: true));
                    sb.AppendLine();
                }

                File.WriteAllText(path, sb.ToString());
            }
        }

        public void CloseTab(ITab tab)
        {
            DeactivateItem(tab, true);
            NotifyOfPropertyChange(nameof(CanCloseFile));
        }

        public void CloseFile()
        {
            CloseTab(ActiveItem);
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        private ITab CreateTab(string path)
        {
            var newTab = _tabFactory();
            newTab.FilePath = path;
            Items.Add(newTab);
            return newTab;
        }
    }
}
