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
        public TabViewModel()
        {
        }

        public string FilePath { get; set; }
        public SC3Module Module { get; private set; }
        public IList<GameStringViewModel> Strings { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            string fileName = Path.GetFileName(FilePath);
            DisplayName = fileName;

            DecodeStrings();
        }

        public void UpdateRow(GameStringViewModel row)
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

            Module.UpdateString(row.Id, SC3String.Deserialize(updatedText));
        }

        private void DecodeStrings()
        {
            var strings = new List<GameStringViewModel>();

            Module = SC3Module.Load(FilePath);
            foreach (var handle in Module.StringTable)
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
                Module.Dispose();
            }
        }
    }
}
