using Caliburn.Micro;
using Newtonsoft.Json;
using ProjectAmadeus.Messages;
using ProjectAmadeus.Models;
using ProjectAmadeus.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace ProjectAmadeus.ViewModels
{
    public sealed class ShellViewModel : Conductor<ITab>.Collection.OneActive, IHandle<OpenFontEditorCommand>
    {
        private readonly Func<DocumentViewModel> _documentFactory;
        private readonly Func<FontEditorViewModel> _fontEditorFactory;
        private readonly SharedData _sharedData;
        private readonly IFilePicker _filePicker;
        private readonly IEventAggregator _eventAggregator;

        private readonly FileDialogFilter GameScriptFilter;

        public ShellViewModel()
        {
        }

        public ShellViewModel(NotificationAreaViewModel notificationArea, Func<DocumentViewModel> documentFactory,
            Func<FontEditorViewModel> fontEditorFactory, SharedData sharedData, IFilePicker filePicker, IEventAggregator eventAggregator)
        {
            NotificationArea = notificationArea;
            _documentFactory = documentFactory;
            _fontEditorFactory = fontEditorFactory;
            _sharedData = sharedData;
            _filePicker = filePicker;
            _eventAggregator = eventAggregator;

            DisplayName = "Project Amadeus";
            GameScriptFilter = new FileDialogFilter("SciADV game scripts", new[] { ".scx", ".scr" });
            CloseStrategy = new AmadeusCloseStrategy();

            _eventAggregator.Subscribe(this);
        }

        public NotificationAreaViewModel NotificationArea { get; }
        public bool CanCloseFile => ActiveItem != null;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadLanguages();
        }

        private void LoadLanguages()
        {
            string json = File.ReadAllText("Data/languages.json");
            _sharedData.Languages = JsonConvert.DeserializeObject<ImmutableArray<Language>>(json);
        }

        public void Browse()
        {
            string path = _filePicker.PickOpen(GameScriptFilter);
            if (!string.IsNullOrEmpty(path))
            {
                OpenFile(path);
                NotifyOfPropertyChange(nameof(CanCloseFile));
            }
        }

        public void HandleFileDrop(IEnumerable<string> files)
        {
            foreach (string path in files)
            {
                OpenFile(path);
            }
        }

        private void OpenFile(string path)
        {
            var documentTab = _documentFactory();
            documentTab.FilePath = path;
            NewTab(documentTab);
        }

        private void NewTab(ITab tab, bool activate = true)
        {
            Items.Add(tab);
            if (activate)
            {
                ActivateItem(tab);
            }
        }

        public void SaveChanges()
        {
            var doc = ActiveItem as IDocumentTab;
            if (doc != null)
            {
                doc.SaveChanges();
            }
        }

        public void CloseFile() => CloseTab(ActiveItem);
        public void CloseTab(ITab tab)
        {
            DeactivateItem(ActiveItem, close: true);
            NotifyOfPropertyChange(nameof(CanCloseFile));
        }

        public void Exit()
        {
            TryClose();
        }

        public void ShowFontEditor(string userCharacters)
        {
            var editor = _fontEditorFactory();
            NewTab(editor);
        }

        public void Handle(OpenFontEditorCommand command)
        {
            ShowFontEditor(command.UserCharacters);
        }
    }
}
