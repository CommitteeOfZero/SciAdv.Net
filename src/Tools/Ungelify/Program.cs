using SciAdvNet.Vfs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ungelify
{
    public sealed class Program
    {
        private const string ExitCommand = "exit";
        private readonly Dictionary<string, Action<string[]>> _commands;
        private IArchive _currentArchive;
        private string _currentArchiveName;
        private string _currentOutputDir;

        public Program()
        {
            _commands = new Dictionary<string, Action<string[]>>()
            {
                ["cd"] = SetCurrentDirectory,
                ["open"] = Open,
                ["list-contents"] = ListContents,
                ["ls"] = ListContents,
                ["extract"] = Extract,
                ["replace"] = Replace,
                ["write-toc"] = WriteToc,
                ["close"] = Close
            };
        }

        public static void Main(string[] args)
        {
            var ungelify = new Program();
            if (!args.Any())
            {
                ungelify.EnterInteractiveMode();
            }
            else
            {
                Console.WriteLine();
                ungelify.Execute(args);
            }
        }

        private void Execute(string[] args)
        {
            if (args.Any())
            {
                if (!IsCommand(args[0]))
                {

                    return;
                }

                Action<string[]> commandHandler;
                _commands.TryGetValue(args[0].ToLower(), out commandHandler);
                try
                {
                    args = args.Skip(1).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    commandHandler?.Invoke(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                }

                if (commandHandler == null)
                {
                    Console.WriteLine("Unknown command.");
                    Console.WriteLine();
                }
            }
        }

        private bool IsCommand(string text) => _commands.ContainsKey(text.ToLower());

        private void SetCurrentDirectory(string[] args)
        {
            EnsureEnoughArguments(args, 1);

            string path = string.Join(" ", args);
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            if (Directory.Exists(path))
            {
                Environment.CurrentDirectory = path;
                return;
            }
            else if (path == "..")
            {
                var info = new DirectoryInfo(Environment.CurrentDirectory);
                Environment.CurrentDirectory = info.Parent.FullName;
                return;
            }

            string newDir = Path.Combine(Environment.CurrentDirectory, path);
            if (Directory.Exists(newDir))
            {
                Environment.CurrentDirectory = newDir;
            }
        }

        private void Open(string[] args)
        {
            EnsureEnoughArguments(args, 1);

            string path = args[0];
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Couldn't locate the specified file.");
            }

            var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            var decoder = Decoder.Recognize(stream);
            _currentArchive = decoder.LoadArchive(stream);
            _currentArchiveName = Path.GetFileName(path);
            _currentOutputDir = Path.GetFileNameWithoutExtension(path);
        }

        private void Extract(string[] args)
        {
            EnsureEnoughArguments(args, 1);
            EnsureInputSpecified();

            string arg = args[0];
            Directory.CreateDirectory(_currentOutputDir);

            if (arg.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                ExtractAll();
            }
            else
            {
                IFileEntry entry = GetEntry(arg);
                if (entry != null)
                {
                    ExtractFile(entry);
                }
            }
        }

        private IFileEntry GetEntry(string nameOrId)
        {
            int id;
            try
            {
                if (int.TryParse(nameOrId, out id))
                {
                    return _currentArchive.GetEntry(id);
                }
                else
                {
                    return _currentArchive.GetEntry(nameOrId);
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"{_currentArchiveName} does not contain entry '{nameOrId}'.");
            }
        }

        private void Replace(string[] args)
        {
            EnsureInputSpecified();
            EnsureEnoughArguments(args, 1);

            string fileToReplace = args[0];
            var newerFile = File.OpenRead(fileToReplace);
            var archiveEntry = GetEntry(fileToReplace);
            using (var entryStream = archiveEntry.Open())
            {
                entryStream.SetLength(0);
                newerFile.CopyTo(entryStream);
            }

            newerFile.Dispose();
            _currentArchive.SaveChanges();
        }

        private void ListContents(string[] args)
        {
            EnsureInputSpecified();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Size",-5}");
            Console.ResetColor();

            foreach (var fileEntry in _currentArchive.Entries)
            {
                double size = (double)fileEntry.Length / (1024 * 1024);
                Console.WriteLine($"{fileEntry.Id,-5} {fileEntry.Name,-20} {$"{Math.Round(size, 3)} mb",-5}");
            }

            Console.WriteLine();
        }

        private void WriteToc(string[] args)
        {
            EnsureInputSpecified();

            string tocFileName = Path.ChangeExtension(_currentArchiveName, "lst");
            var entryNames = _currentArchive.Entries.Select(x => x.Name).ToArray();
            File.WriteAllText(tocFileName, string.Join(Environment.NewLine, entryNames));
        }

        private void Close(string[] args)
        {
            _currentArchive?.Dispose();
            _currentArchive = null;
            Console.WriteLine();
        }

        private void EnsureEnoughArguments(string[] args, int expectedArgCount)
        {
            if (args.Length < expectedArgCount)
            {
                throw new ArgumentException("Insufficient arguments.");
            }
        }

        private void EnsureInputSpecified()
        {
            if (_currentArchive == null)
            {
                throw new InvalidOperationException("You should specify the input first.");
            }
        }

        private void EnterInteractiveMode()
        {
            DisplayHelp();

            string command;
            do
            {
                string prefix;
                if (_currentArchive != null)
                {
                    prefix = "ungelify>";
                }
                else
                {
                    var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
                    prefix = $"ungelify[{dirInfo.Name}]>";
                }
                Console.Write(prefix);
                command = Console.ReadLine();
                Execute(command.Split(' '));
            } while (!command.Equals(ExitCommand, StringComparison.OrdinalIgnoreCase));
        }

        private void DisplayHelp()
        {
            Console.WriteLine("cd <path>");
            Console.WriteLine("Commands:");
            Console.WriteLine("open <filename>");
            Console.WriteLine("ls OR list-contents");
            Console.WriteLine("extract all OR extract <filename> OR extract <id> (use 'open' first)");
            Console.WriteLine("replace <filename>");
            Console.WriteLine("close");
            Console.WriteLine();
        }

        private void ExtractAll()
        {
            foreach (var entry in _currentArchive.Entries)
            {
                ExtractFile(entry);
            }
        }

        private void ExtractFile(IFileEntry fileEntry)
        {
            string outputPath = Path.Combine(_currentOutputDir, fileEntry.Name);
            using (var inputStream = _currentArchive.GetEntry(fileEntry.Id).Open())
            using (var outputStream = File.Create(outputPath))
            {
                inputStream.CopyTo(outputStream);
            }
        }
    }
}
