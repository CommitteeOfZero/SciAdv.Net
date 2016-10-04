using PropertyChanged;

namespace ProjectAmadeus.ViewModels
{
    [ImplementPropertyChanged]
    public sealed class GameStringViewModel
    {
        public GameStringViewModel(int id, int offset, string characterName, string line)
        {
            Id = id;
            Offset = offset;
            CharacterName = characterName;
            DialogueLine = line;
        }

        public GameStringViewModel()
        {
        }

        public int Id { get; }
        public int Offset { get; set; }
        public string CharacterName { get; set; }
        public string DialogueLine { get; set; }
    }
}
