using PropertyChanged;
using SciAdvNet.SC3Script.Text;

namespace ProjectAmadeus.Models
{
    [AddINotifyPropertyChangedInterface]
    public sealed class StringTableEntry
    {
        private readonly string _originalCharacterName;
        private readonly string _originalDialogueLine;

        public StringTableEntry(int id, SC3String sc3String, string characterName, string dialogueLine)
        {
            Id = id;
            IntermediateRepresentation = sc3String;
            _originalCharacterName = CharacterName = characterName;
            _originalDialogueLine = DialogueLine = dialogueLine;
        }

        public int Id { get; private set; }
        public SC3String IntermediateRepresentation { get; set; }
        public string CharacterName { get; set; }
        public string DialogueLine { get; set; }

        public string NormalizedCharacterName => AmadeusTextProcessor.Normalize(CharacterName);
        public string NormalizedDialogueLine => AmadeusTextProcessor.Normalize(DialogueLine);

        public bool IsDirty => !object.ReferenceEquals(CharacterName, _originalCharacterName) || !object.ReferenceEquals(DialogueLine, _originalDialogueLine);

        public override string ToString() => IntermediateRepresentation.ToString();
    }
}
