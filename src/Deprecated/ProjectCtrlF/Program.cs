using SciAdvNet.SC3Script;
using SciAdvNet.SC3Script.Text;
using SciAdvNet.Vfs;
using SciAdvNet.Vfs.Mages;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ProjectCtrlF
{
    public class Program
    {
        private const string ExitCommand = "exit";
        private readonly Dictionary<string, Action<string[]>> _handlers;

        private MagesArchive _archive;
        private ImmutableDictionary<int, SC3Script> _scripts;
        private readonly ReplacementListManager _replacementListManager;
        private ReplacementList _selectedList;

        public static void Main(string[] args)
        {
            Console.InputEncoding = Console.OutputEncoding = Encoding.Unicode;
            var instance = new Program();
            instance.Run();
        }

        public Program()
        {
            _handlers = new Dictionary<string, Action<string[]>>()
            {
                ["create-list"] = CreateList,
                ["select-list"] = SelectList,
                ["save-lists"] = SaveLists,
                ["find"] = Find,
                ["find-replace"] = FindReplace,
                ["replace-all"] = ReplaceAll,
                ["find-duplicates"] = FindDuplicates,
                ["encode-all"] = EncodeAll,
                ["apply-all"] = ApplyAll,
                ["help"] = PrintHelp
            };

            _replacementListManager = new ReplacementListManager();
        }

        private void Run()
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            EnterMainLoop();
            Shutdown();
        }

        private void Init()
        {
            Console.WriteLine("Project Ctrl+F");
            Console.WriteLine();
            PrintHelp(null);
            Console.WriteLine();

            _scripts = LoadScripts("script.mpk");
            Console.WriteLine($"Loaded {_scripts.Count} scripts from script.mpk");

            foreach (var list in _replacementListManager.LoadExistingLists())
            {
                Console.WriteLine($"Loaded replacement list '{list.FriendlyName}'");
            }
        }

        private void PrintHelp(string[] args)
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("create-list <listName>");
            Console.WriteLine("select-list <listName>");
            Console.WriteLine("save-lists");
            Console.WriteLine("find <searchTerm>");
            Console.WriteLine("find-replace <scriptName>|<scriptId> <lineNumber> <searchTerm> <replaceWith>");
            Console.WriteLine("replace-all <searchTerm> <replaceWith>");
            Console.WriteLine("find-duplicates <list1> <list2>");
            Console.WriteLine("apply-all");
            Console.WriteLine("encode-all");
            Console.WriteLine("help");
            Console.WriteLine("exit");
        }

        private ImmutableDictionary<int, SC3Script> LoadScripts(string fileName)
        {
            var result = ImmutableDictionary.CreateBuilder<int, SC3Script>();
            _archive = MagesArchive.Load(fileName, ArchiveMode.Update);
            {
                foreach (var entry in _archive.Entries)
                {
                    if (entry.Name == "zz.scx")
                    {
                        continue;
                    }

                    var stream = entry.Open();
                    var script = SC3Script.Load(stream, entry.Name, SC3Game.ChaosChild, leaveOpen: false);

                    result[entry.Id] = script;
                }
            }

            return result.ToImmutable();
        }

        private void EnterMainLoop()
        {
            string input;
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
                var args = SplitArgs(input);
                try
                {
                    Execute(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (!input.Equals(ExitCommand, StringComparison.OrdinalIgnoreCase));
        }

        private void Shutdown()
        {
            _replacementListManager.SaveAll();
            foreach (var script in _scripts.Values)
            {
                script.Dispose();
            }

            _archive.Dispose();
        }

        private void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            string command = args[0];
            _handlers.TryGetValue(command.ToLowerInvariant(), out var handler);

            handler?.Invoke(args.Skip(1).ToArray());
        }

        private void CheckArguments(string[] args, int expectedArgCount)
        {
            if (args.Length < expectedArgCount)
            {
                throw new ArgumentException("Insufficient arguments.");
            }
        }

        private void EnsureListSelected()
        {
            if (_selectedList == null)
            {
                throw new InvalidOperationException("You need to select a replacement list.");
            }
        }

        private void FindDuplicates(string[] args)
        {
            CheckArguments(args, expectedArgCount: 2);
            var list1 = _replacementListManager.GetList(args[0]);
            var list2 = _replacementListManager.GetList(args[1]);

            foreach (var replacement in list1.Items)
            {
                if (list2.Items.Any(x => x.ScriptId == replacement.ScriptId && x.StringId == replacement.StringId))
                {
                    Console.WriteLine($"[{replacement.ScriptId}][{replacement.StringId}]");
                }
            }
        }

        private void CreateList(string[] args)
        {
            CheckArguments(args, expectedArgCount: 1);

            string name = args[0];
            _replacementListManager.CreateList(name);

            SelectList(args);
        }

        private void SelectList(string[] args)
        {
            CheckArguments(args, expectedArgCount: 1);

            string name = args[0];
            try
            {
                _selectedList = _replacementListManager.GetList(name);
            }
            catch
            {
                throw new ArgumentException($"'{name}' doesn't exist.");
            }

            Console.WriteLine($"'{name}' is now selected.");
        }

        private void SaveLists(string[] args)
        {
            Console.WriteLine("Roger.");
            _replacementListManager.SaveAll();
        }

        private void Find(string[] args)
        {
            string searchTerm = string.Join(" ", args);
            DoFind(searchTerm);
        }

        private void FindReplace(string[] args)
        {
            CheckArguments(args, 4);
            EnsureListSelected();

            SC3Script script;
            if (int.TryParse(args[0], out int scriptId))
            {
                _scripts.TryGetValue(scriptId, out script);
            }
            else
            {
                var pair = _scripts.FirstOrDefault(x => x.Value.FileName.Equals(args[0], StringComparison.OrdinalIgnoreCase));
                scriptId = pair.Key;
                script = pair.Value;
            }

            if (script == null)
            {
                throw new ArgumentException($"Script '{args[0]}' doesn't exist.");
            }

            int lineId = int.Parse(args[1]);
            string searchTerm = args[2];
            string replaceWith = args[3];

            int count = 0;
            foreach (var occurence in FindOccurences(scriptId, lineId, searchTerm))
            {
                count++;
                Replace(occurence, replaceWith);
            }

            Console.WriteLine($"Replaced {count} occurrences of '{searchTerm}' in {script.FileName}");
        }

        private void ReplaceAll(string[] args)
        {
            CheckArguments(args, expectedArgCount: 2);
            EnsureListSelected();

            string searchTerm = args[0];
            string replaceWith = args[1];

            DoReplaceAll(searchTerm, replaceWith);
        }

        private void ApplyAll(string[] args)
        {
            var affectedScripts = new List<string>();
            foreach (var list in _replacementListManager.LoadedLists)
            {
                foreach (var group in list.Items.GroupBy(x => x.ScriptId))
                {
                    var script = _scripts[group.Key];
                    foreach (var replacement in group)
                    {
                        var bytes = SC3String.Deserialize(replacement.Text).Encode(SC3Game.SteinsGateZero);
                        script.UpdateString(replacement.StringId, bytes);
                    }

                    script.ApplyPendingUpdates();
                    affectedScripts.Add(script.FileName);
                }
            }

            _archive.SaveChanges();

            Console.WriteLine("Done.");
            Console.WriteLine("Affected scripts:");
            foreach (string name in affectedScripts)
            {
                Console.WriteLine(name);
            }
        }

        private void EncodeAll(string[] args)
        {
            new ListEncoder().EncodeLists(_replacementListManager.LoadedLists);
        }

        private void DoFind(string searchTerm)
        {
            int totalOccurrences = 0;
            foreach (var group in FindOccurrences(searchTerm).GroupBy(x => x.ScriptName))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(group.Key);
                Console.ResetColor();

                totalOccurrences += group.Count();
                foreach (var occurrence in group)
                {
                    string normalizedText = StringUtils.NormalizeString(occurrence.Text);
                    Console.WriteLine($"[{occurrence.StringId}] {normalizedText}");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Found {totalOccurrences} occurrences of '{searchTerm}'");
        }

        private void DoReplaceAll(string searchTerm, string replaceWith)
        {
            int count = 0;
            foreach (var occurrence in FindOccurrences(searchTerm))
            {
                count++;
                Replace(occurrence, replaceWith);
            }

            Console.WriteLine($"Replaced {count} occurrences of '{searchTerm}' with '{replaceWith}'");
        }

        private string[] SplitArgs(string userInput)
        {
            var mutable = new StringBuilder(userInput);
            bool inQuotes = false;
            for (int index = 0; index < mutable.Length; index++)
            {
                if (mutable[index] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (!inQuotes && mutable[index] == ' ')
                {
                    mutable[index] = '\n';
                }
            }

            mutable.Replace("\"", string.Empty);
            return mutable.ToString().Split('\n');
        }

        private IEnumerable<Occurrence> FindOccurrences(string searchTerm)
        {
            foreach (var pair in _scripts)
            {
                int scriptId = pair.Key;
                var script = pair.Value;
                foreach (var stringTableEntry in script.StringTable)
                {
                    foreach (var occurence in FindOccurences(scriptId, stringTableEntry.Id, searchTerm))
                    {
                        yield return occurence;
                    }
                }
            }
        }

        private IEnumerable<Occurrence> FindOccurences(int scriptId, int lineId, string searchTerm)
        {
            var culture = CultureInfo.GetCultureInfo("en-US");

            var script = _scripts[scriptId];
            var stringTableEntry = script.StringTable[lineId];
            var sc3String = stringTableEntry.Resolve();
            string text = sc3String.ToString();

            bool contains = culture.CompareInfo.IndexOf(text, searchTerm, CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase) >= 0;
            if (contains)
            {
                bool fullwidth = ShouldUseFullwidthCharacters(script.FileName);
                yield return new Occurrence(scriptId, script.FileName, stringTableEntry.Id, searchTerm, text, fullwidth);
            }
        }

        private bool ShouldUseFullwidthCharacters(string scriptName)
        {
            return !scriptName.ToUpperInvariant().StartsWith("_PHONE");
        }

        private void Replace(Occurrence occurrence, string replaceWith)
        {
            var existing = _selectedList.Items.FirstOrDefault(x => x.ScriptId == occurrence.ScriptId && x.StringId == occurrence.StringId);
            string originalText = existing?.Text ?? occurrence.Text;
            _selectedList.Items.Remove(existing);

            string newText;
            if (occurrence.Fullwidth)
            {
                originalText = originalText.Replace('.', '．');
                string fwSearchTerm = StringUtils.ConvertToFullwidth(occurrence.SearchTerm);
                string fwReplaceWith = StringUtils.ConvertToFullwidth(replaceWith);

                newText = originalText.Replace(fwSearchTerm, fwReplaceWith);
            }
            else
            {

                newText = originalText.Replace(occurrence.SearchTerm.Replace(' ', (char)0x3000), replaceWith);
            }

            _selectedList.Items.Add(new Replacement(occurrence.ScriptId, occurrence.StringId, newText));
        }
    }
}
