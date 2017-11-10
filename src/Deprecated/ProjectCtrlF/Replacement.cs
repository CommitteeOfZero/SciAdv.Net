namespace ProjectCtrlF
{
    public sealed class Replacement
    {
        public Replacement(int scriptId, int stringId, string text)
        {
            ScriptId = scriptId;
            StringId = stringId;
            Text = text;
        }

        public int ScriptId { get; }
        public int StringId { get; }
        public string Text { get; }
    }
}
