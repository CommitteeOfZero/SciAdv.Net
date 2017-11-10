using Caliburn.Micro;
using SciAdvNet.SC3Script;
using System.IO;
using System;
using ProjectAmadeus.ViewModels.Notifications;
using ProjectAmadeus.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProjectAmadeus.ViewModels
{
    public sealed class DocumentViewModel : Conductor<IEditor>.Collection.OneActive, ITab, IDocumentTab, IAsyncShutdown
    {
        private const string ConfigExtension = ".amadeus";

        private Module _module;
        private string _configPath;

        private readonly Workspace _workspace;
        private readonly NotificationAreaViewModel _notificationArea;

        private readonly StringTableViewModel _stringTable;
        private readonly CodeEditorViewModel _codeEditor;

        public DocumentViewModel(Workspace workspace, StringTableViewModel stringTable, CodeEditorViewModel codeEditor, NotificationAreaViewModel notificationArea)
        {
            _workspace = workspace;
            _stringTable = stringTable;
            _codeEditor = codeEditor;
            _notificationArea = notificationArea;

            Items.Add(stringTable);
        }

        public DocumentViewModel()
        {
        }

        public string FilePath { get; set; }

        public IEditor CurrentEditor => ActiveItem;
        private void SwitchToStringTable() => ActivateItem(_stringTable);

        protected override void OnInitialize()
        {
            base.OnInitialize();

            string fileName = Path.GetFileName(FilePath);
            DisplayName = fileName;

            if (LoadModule())
            {
                _workspace.CurrentModule = _module;
                SwitchToStringTable();
            }
            else
            {
                TryClose();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _workspace.CurrentModule = _module;

        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                _module?.Script.Dispose();
            }

            base.OnDeactivate(close);
        }

        private bool LoadModule()
        {
            try
            {
                SC3Game game = SC3Game.SteinsGateZero;
                string name = Path.GetFileNameWithoutExtension(FilePath);
                if (name.StartsWith("RN"))
                {
                    game = SC3Game.RoboticsNotes;
                }

                var script = SC3Script.Load(FilePath, game);
                var config = LoadConfig();

                _module = new Module(FilePath, script, _configPath, config);
                return true;
            }
            catch (NotSupportedException)
            {
                _notificationArea.ShowNotification(new UnsupportedScriptNotificationViewModel());
            }
            catch (InvalidDataException)
            {
                string message = "Project Amadeus was unable to load this file. It might be damaged or encrypted.";
                _notificationArea.ShowNotification(new GenericNotificationViewModel(message, NotificationKind.Error));
            }
            catch (FileNotFoundException notFound)
            {
                _notificationArea.ShowNotification(new GenericNotificationViewModel(notFound.Message, NotificationKind.Error));
            }
            catch (UnauthorizedAccessException unauthorizedAccess)
            {
                _notificationArea.ShowNotification(new GenericNotificationViewModel(unauthorizedAccess.Message, NotificationKind.Error));
            }

            return false;
        }

        private ScriptConfig LoadConfig()
        {
            _configPath = FilePath + ConfigExtension;
            string json;
            try
            {
                json = File.ReadAllText(_configPath);
            }
            catch (FileNotFoundException)
            {
                return new ScriptConfig { LanguageCode = 1 };
            }

            var config = JsonConvert.DeserializeObject<ScriptConfig>(json);
            return config;
        }

        private void WriteConfig()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string json = JsonConvert.SerializeObject(_module.Config, Formatting.Indented, serializerSettings);
            File.WriteAllText(_module.ConfigPath, json);
        }

        public void SaveChanges()
        {
            WriteConfig();
            CurrentEditor.SaveChanges();
        }

        public async Task ShutdownAsync()
        {
            if (CurrentEditor?.AnyUnsavedChanges == true)
            {
                var prompt = new SaveChangesPromptViewModel(_module.ScriptName);
                bool needToSaveChanges = await _notificationArea.ShowNotificationAsync(prompt);

                if (needToSaveChanges)
                {
                    SaveChanges();
                }
            }
        }
    }
}
