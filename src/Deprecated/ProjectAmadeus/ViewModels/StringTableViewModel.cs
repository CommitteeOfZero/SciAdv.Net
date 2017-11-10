using Caliburn.Micro;
using ProjectAmadeus.Models;
using ProjectAmadeus.Services;
using ProjectAmadeus.ViewModels.Notifications;
using SciAdvNet.SC3Script;
using SciAdvNet.SC3Script.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ProjectAmadeus.ViewModels
{
    public sealed class StringTableViewModel : Screen, IEditor
    {
        private readonly Workspace _workspace;
        private readonly NotificationAreaViewModel _notificationArea;
        private readonly SharedData _sharedData;
        private readonly IEventAggregator _eventAggregator;

        private Module _module;
        private SC3StringEncoder _stringEncoder;
        private SC3StringDecoder _stringDecoder;
        private bool _decodingSucceeded;

        public StringTableViewModel(Workspace workspace, NotificationAreaViewModel notificationArea,
            SharedData sharedData, IEventAggregator eventAggregator)
        {
            _workspace = workspace;
            _notificationArea = notificationArea;
            _sharedData = sharedData;
            _eventAggregator = eventAggregator;

            Languages = sharedData.Languages;
        }

        public StringTableViewModel()
        {
            if (Execute.InDesignMode)
            {
                Entries = new List<StringTableEntry>()
                {
                    new StringTableEntry(0, null, "Lukako", "Do you really want me?")
                };

                Languages = new List<Language>()
                {
                    new Language { Name = "English", Code = 1, CharacterSet = string.Empty }
                };
            }
        }

        public IReadOnlyList<StringTableEntry> Entries { get; private set; }
        public StringTableEntry SelectedEntry { get; set; }

        public IReadOnlyList<Language> Languages { get; }
        public Language SelectedLanguage { get; set; }

        public bool AnyUnsavedChanges { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _module = _workspace.CurrentModule;
            SelectedLanguage =  Languages.Single(x => x.Code == _module.Config.LanguageCode);
            OnLanguageChanged();
            DecodeEntries();
        }

        public void OnRowEditEnding()
        {
            var entry = SelectedEntry;
            if (entry.IsDirty)
            {
                AnyUnsavedChanges = true;

                try
                {
                    var poco = ProduceIntermediateRepresentation(entry.CharacterName, entry.DialogueLine);
                    poco = PreprocessIntermediateRepresentation(poco);

                    string newCharacterName = poco.GetCharacterName().ToString();
                    string newDialogueLine = poco.GetDialogueLine().ToString();

                    entry.IntermediateRepresentation = poco;
                    entry.CharacterName = newCharacterName;
                    entry.DialogueLine = newDialogueLine;
                }
                catch (Exception ex)
                {
                    var error = new GenericNotificationViewModel(ex.Message, NotificationKind.Error);
                    _notificationArea.ShowNotification(error);
                }
            }
        }

        private SC3String ProduceIntermediateRepresentation(string characterName, string dialogueLine)
        {
            if (string.IsNullOrEmpty(characterName))
            {
                return SC3String.Deserialize(dialogueLine);
            }

            return SC3String.Deserialize($"[name]{characterName}[line]{dialogueLine}");
        }

        private SC3String PreprocessIntermediateRepresentation(SC3String sc3String)
        {
            var processedSegments = sc3String.Segments.ToBuilder();
            for (int i = 0; i < sc3String.Segments.Length; i++)
            {
                if (sc3String.Segments[i].SegmentKind == SC3StringSegmentKind.Text)
                {
                    var rawText = sc3String.Segments[i] as TextSegment;
                    var preprocessedText = new TextSegment(AmadeusTextProcessor.Preprocess(rawText.Value));

                    processedSegments[i] = preprocessedText;
                }
            }

            return new SC3String(processedSegments.ToImmutable(), true);
        }

        public void OnLanguageChanged()
        {
            _stringEncoder = new SC3StringEncoder(_module.Script.Game, SelectedLanguage.CharacterSet);
            _stringDecoder = new SC3StringDecoder(_module.Script.Game, SelectedLanguage.CharacterSet);
            _module.Config.LanguageCode = SelectedLanguage.Code;

            if (!_decodingSucceeded)
            {
                DecodeEntries();
            }
        }

        public void SaveChanges()
        {
            EncodeEntries();
            AnyUnsavedChanges = false;
        }

        private void DecodeEntries()
        {
            var script = _module.Script;

            var entries = ImmutableArray.CreateBuilder<StringTableEntry>();
            foreach (var stringHandle in _module.Script.StringTable)
            {
                SC3String sc3String = null;
                try
                {
                    sc3String = _stringDecoder.DecodeString(stringHandle.RawData);
                }
                catch (StringDecodingFailedException ex)
                {
                    _decodingSucceeded = false;
                    var error = new GenericNotificationViewModel(ex.Message, NotificationKind.Error);
                    _notificationArea.ShowNotification(error);
                    return;
                }

                var characterName = sc3String.GetCharacterName();
                var dialogueLine = sc3String.GetDialogueLine();

                var row = new StringTableEntry(stringHandle.Id, sc3String, characterName.ToString(), dialogueLine.ToString());
                entries.Add(row);
            }

            Entries = entries.ToImmutable();
            _decodingSucceeded = true;
        }

        private void EncodeEntries()
        {
            var script = _module.Script;
            foreach (var entry in Entries)
            {
                ImmutableArray<byte> bytes;
                try
                {
                    bytes = _stringEncoder.Encode(entry.IntermediateRepresentation);
                    script.UpdateString(entry.Id, bytes);

                    script.ApplyPendingUpdates();
                }
                catch (StringEncodingFailedException ex)
                {
                    var error = new GenericNotificationViewModel(ex.Message, NotificationKind.Error);
                    _notificationArea.ShowNotification(error);
                    return;
                }
            }
        }
    }
}
