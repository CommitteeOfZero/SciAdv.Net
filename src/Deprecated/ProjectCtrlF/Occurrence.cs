namespace ProjectCtrlF
{
    public sealed class Occurrence
    {
        public Occurrence(int scriptId, string scriptName, int stringId, string searchTerm, string text, bool fullwidth)
        {
            ScriptId = scriptId;
            ScriptName = scriptName;
            StringId = stringId;
            SearchTerm = searchTerm;
            Text = text;
            Fullwidth = fullwidth;
        }

        public int ScriptId { get; }
        public string ScriptName { get; }
        public int StringId { get; }
        public string SearchTerm { get; }
        public string Text { get; }
        public bool Fullwidth { get; }
    }
}
