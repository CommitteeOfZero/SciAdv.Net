using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAmadeus.ViewModels
{
    //[ImplementPropertyChanged]
    public sealed class StringEditorViewModel : Screen
    {
        //public IList<GameStringViewModel> Strings { get; private set; }

        //protected override void OnInitialize()
        //{
        //    base.OnInitialize();
        //    DecodeStrings();
        //}

        //public void UpdateRow(GameStringViewModel row)
        //{
        //    string updatedText;
        //    if (!string.IsNullOrEmpty(row.CharacterName))
        //    {
        //        updatedText = $"[name]{row.CharacterName}[line]{row.DialogueLine}";
        //    }
        //    else
        //    {
        //        updatedText = row.DialogueLine;
        //    }

        //    Module.UpdateString(row.Id, SC3String.Deserialize(updatedText));
        //}

        //private void DecodeStrings()
        //{
        //    var strings = new List<GameStringViewModel>();

        //    Module = SC3Module.Load(FilePath);
        //    foreach (var handle in Module.StringTable)
        //    {
        //        var sc3String = handle.Resolve();

        //        string characterName = sc3String.GetCharacterName().ToString(normalize: true);
        //        string line = sc3String.GetDialogueLine().ToString(normalize: true);

        //        strings.Add(new GameStringViewModel(handle.Id, handle.Offset, characterName, line));
        //    }

        //    Strings = strings;
        //}
    }
}
