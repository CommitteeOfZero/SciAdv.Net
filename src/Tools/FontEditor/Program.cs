using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FontEditor
{
    public class Program
    {
        private readonly Dictionary<string, Action<IDictionary<string, string>>> _commands;

        public Program()
        {
            _commands = new Dictionary<string, Action<IDictionary<string, string>>>()
            {
                ["insert-characters"] = InsertCharacters
            };
        }

        private int Run(string command, IDictionary<string, string> arguments)
        {
            Action<IDictionary<string, string>> handler;
            if (!_commands.TryGetValue(command.ToLowerInvariant(), out handler))
            {
                Console.WriteLine("Unknown command.");
                return -1;
            }

            try
            {
                handler(arguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }

        private void InsertCharacters(IDictionary<string, string> arguments)
        {
            string inputFileName = string.Empty, outputFileName = String.Empty;
            string characters = string.Empty, fontFamily = string.Empty;
            try
            {
                inputFileName = arguments["input"];
                outputFileName = arguments["output"];
                characters = arguments["characters"];
                fontFamily = arguments["font-family"];
            }
            catch
            {
                throw new ArgumentException("Insufficient arguments.");
            }

            using (var inputStream = File.OpenRead(inputFileName))
            using (var outputStream = File.Create(outputFileName))
            using (var glyphRenderer = new GlyphRenderer())
            using (var updatedStream = glyphRenderer.InsertGlyphs(inputStream, fontFamily, characters, 5616))
            {
                updatedStream.CopyTo(outputStream);
            }
        }

        public static void Main(string[] args)
        {
            if (!args.Any())
            {
                return;
            }

            var instance = new Program();
            string command = args[0];
            var parsedArgs = ParseArguments(args.Skip(1).ToArray());
            int exitCode = instance.Run(command, parsedArgs);
            Environment.Exit(exitCode);
        }

        private static IDictionary<string, string> ParseArguments(string[] args)
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 1 >= args.Length || !args[i].StartsWith("-"))
                {
                    throw new ArgumentException();
                }

                string key = args[i].StartsWith("--") ? args[i].Remove(0, 2) : args[i].Remove(0, 1);
                string value = args[i + 1];
                dict[key.ToLowerInvariant()] = value;
            }

            return dict;
        }
    }
}
