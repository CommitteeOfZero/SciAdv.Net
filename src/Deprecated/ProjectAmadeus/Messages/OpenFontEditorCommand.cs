namespace ProjectAmadeus.Messages
{
    public sealed class OpenFontEditorCommand
    {
        public OpenFontEditorCommand(string userCharacters)
        {
            UserCharacters = userCharacters;
        }

        public string UserCharacters { get; }
    }
}
