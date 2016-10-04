using Caliburn.Micro;
using ProjectAmadeus.Models;
using SciAdvNet.SC3;
using SciAdvNet.SC3.Text;
using System.Collections.Generic;
using System.IO;

namespace ProjectAmadeus.ViewModels
{
    public sealed class TabViewModel : Screen, ITab
    {
        private SC3Module _module;

        public TabViewModel()
        {
        }

        public string FilePath { get; set; }
        public IList<GameStringViewModel> Strings { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            string fileName = Path.GetFileName(FilePath);
            DisplayName = fileName;

            DecodeStrings();
        }

        public void UpdateRow(GameString row)
        {
            string updatedText;
            if (!string.IsNullOrEmpty(row.CharacterName))
            {
                updatedText = $"[name]{row.CharacterName}[line]{row.DialogueLine}";
            }
            else
            {
                updatedText = row.DialogueLine;
            }

            _module.UpdateString(row.Id, SC3String.Deserialize(updatedText));
        }

        private void DecodeStrings()
        {
            var strings = new List<GameStringViewModel>();

            _module = SC3Module.Load(FilePath);
            foreach (var handle in _module.StringTable)
            {
                var sc3String = handle.Resolve();

                string characterName = sc3String.GetCharacterName().ToString(normalize: true);
                string line = sc3String.GetDialogueLine().ToString(normalize: true);

                strings.Add(new GameStringViewModel(handle.Id, handle.Offset, characterName, line));
            }

            Strings = strings;
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                _module.Dispose();
            }
        }
    }
}
