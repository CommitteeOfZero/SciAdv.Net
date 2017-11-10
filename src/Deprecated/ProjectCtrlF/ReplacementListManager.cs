using SciAdvNet.SC3Script.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ProjectCtrlF
{
    public sealed class ReplacementListManager
    {
        private const string DirectoryName = "ReplacementLists";
        private readonly List<ReplacementList> _lists = new List<ReplacementList>();

        public IReadOnlyList<ReplacementList> LoadedLists => new ReadOnlyCollection<ReplacementList>(_lists);

        public IEnumerable<ReplacementList> LoadExistingLists()
        {
            var directory = Directory.CreateDirectory("ReplacementLists");
            foreach (var fileInfo in directory.EnumerateFiles("*.txt"))
            {
                var current = LoadReplacementList(fileInfo.FullName);
                _lists.Add(current);
                yield return current;
            }
        }

        public ReplacementList GetList(string name)
        {
            return _lists.Single(x => x.FriendlyName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public ReplacementList CreateList(string name)
        {
            string path = Path.Combine(DirectoryName, name + ".txt");
            var list = new ReplacementList(path, new List<Replacement>());
            _lists.Add(list);
            return list;
        }

        public void SaveChanges(ReplacementList list)
        {
            using (var stream = File.Create(list.FilePath))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var replacement in list.Items.OrderBy(x => x.ScriptId).ThenBy(x => x.StringId))
                {
                    writer.WriteLine($"[{replacement.ScriptId}][{replacement.StringId}]{replacement.Text}");
                }
            }
        }

        public void SaveAll()
        {
            foreach (var list in _lists)
            {
                SaveChanges(list);
            }
        }

        private ReplacementList LoadReplacementList(string filePath)
        {
            var items = new List<Replacement>();
            string[] lines = File.ReadAllLines(filePath);

            var deserializer = new CustomizedDeserializer();
            for (int i = 0; i < lines.Length; i++)
            {
                SC3String sc3String = deserializer.Deserialize(lines[i]);
                string text = sc3String.ToString();

                var replacement = new Replacement(deserializer.ScriptId.Value, deserializer.StringId.Value, text);
                items.Add(replacement);
            }

            return new ReplacementList(filePath, items);
        }
    }
}
