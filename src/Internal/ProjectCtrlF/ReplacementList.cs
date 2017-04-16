using System.Collections.Generic;
using System.IO;

namespace ProjectCtrlF
{
    public sealed class ReplacementList
    {
        public ReplacementList(string filePath, List<Replacement> items)
        {
            FilePath = filePath;
            FriendlyName = Path.GetFileNameWithoutExtension(filePath);
            Items = items;
        }

        public string FilePath { get; }
        public string FriendlyName { get; }
        public List<Replacement> Items { get; }
    }
}
