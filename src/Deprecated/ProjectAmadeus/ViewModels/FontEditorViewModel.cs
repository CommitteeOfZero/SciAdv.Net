using Caliburn.Micro;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectAmadeus.Models;
using ProjectAmadeus.Services;
using SciAdvNet.SC3Script;
using SciAdvNet.SC3Script.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace ProjectAmadeus.ViewModels
{
    public sealed class FontEditorViewModel : Screen, ITab
    {
        private const string FontEditExe = "editfont.exe";

        private readonly IFilePicker _filePicker;
        private readonly JsonSerializerSettings _serializerSettings;

        public FontEditorViewModel(IFilePicker filePicker)
        {
            DisplayName = "Font Editor";
            _filePicker = filePicker;
            _serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public FontEditorViewModel()
        {
        }

        public FontGenSettings FontGenSettings { get; private set; }
        public IReadOnlyList<int> FontSizeOptions { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadDefaults();
        }

        private void LoadDefaults()
        {
            FontGenSettings = new FontGenSettings();
            var fontSizes = new List<int> { 30, 32, 34, 36, 38, 40, 42, 44, 46, 48 };
            FontSizeOptions = new ReadOnlyCollection<int>(fontSizes);

            FontGenSettings.FontFamily = "Segoe UI";
            FontGenSettings.FontSize = 48;
        }

        public void Browse()
        {
            //FontGenSettings.SystemMpkPath = _filePicker.PickOpen("system.mpk", "desc");
        }

        public void GeneratePreview()
        {
            //var fontgenSettings = JsonConvert.DeserializeObject<FontGenSettings>(SettingsJson);

            var process = Process.Start(FontEditExe, String.Empty);
            process.WaitForExit();

        }

        protected override void OnDeactivate(Boolean close)
        {
            base.OnDeactivate(close);
        }
    }
}
