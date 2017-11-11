using Newtonsoft.Json;
using SciAdvNet.SC3Script;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;

namespace SC3Tools
{
    internal class Program
    {
        public const string ConfigFileName = "config.json";

        public static ImmutableDictionary<string, Command> AvaliableCommands { get; private set; }
        public static Configuration Config { get; private set; }
        public static (string path, string args) DefaultTextEditor { get; private set; }

        // <alias, SC3Game>
        // For parsing user input.
        public static ImmutableDictionary<string, SC3Game> SupportedGames { get; private set; }
        // <FriendlyName, alias[]>
        // Displayed to the user at startup.
        public static ImmutableDictionary<string, ImmutableArray<string>> GameAliases { get; private set; }

        // TODO: come up with a better name.
        public static IReadOnlyList<ArgumentCommand> Shit { get; private set; }

        static int Main(string[] args)
        {
            Console.InputEncoding = Console.OutputEncoding = Encoding.Unicode;
            return Initialize() && Execute(args.Length > 0 ? args : new[] { "help" }) ? 0 : -1;
        }

        public static bool Execute(string[] args)
        {
            bool displayHelp = false;
            var s = ArgumentSyntax.Parse(args, syntax =>
            {
                HelpCommand helpCommand = null;
                foreach (var command in AvaliableCommands.OrderBy(x => x.Key).Select(x => x.Value))
                {
                    if (command is HelpCommand help)
                    {
                        helpCommand = help;
                        continue;
                    }
                    
                    DefineCommand(command);
                }

                DefineCommand(helpCommand);

                void DefineCommand(Command command)
                {
                    string value = string.Empty;
                    syntax.DefineCommand(command.Name, ref value, command.Description);
                    syntax.DefineOption("h|help", ref displayHelp, "Display help.").IsHidden = true;
                    command.DefineParameters(syntax);
                }
            });

            Shit = s.Commands;

            var activeCommand = AvaliableCommands[s.ActiveCommand.Name];
            if (!(activeCommand is HelpCommand) && !displayHelp)
            {
                bool valid = ValidateArguments(s);
                if (!valid)
                {
                    displayHelp = true;
                    Console.WriteLine();
                }
            }

            if (!displayHelp)
            {
                return activeCommand.Execute();
            }
            else
            {
                Console.WriteLine(s.GetHelpText(300));
                return true;
            }
        }

        public static bool Execute(string commandName)
        {
            if (!AvaliableCommands.TryGetValue(commandName, out var command))
            {
                return false;
            }

            return command.Execute();
        }

        private static bool ValidateArguments(ArgumentSyntax syntax)
        {
            bool valid = true;
            foreach (var x in syntax.GetActiveArguments())
            {
                if (!x.IsOption && !x.IsHidden && !x.IsSpecified)
                {
                    ReportError($"missing parameter '{x.Name}'.");
                    valid = false;
                }
            }

            return valid;
        }

        private static bool Initialize()
        {
            if (!TryReadConfig())
            {
                return false;
            }

            DefaultTextEditor = Config.Editors[Config.DefaultEditorName];
            ConstructSupportedGamesDictionary();

            var thisAssembly = typeof(Program).Assembly;
            AvaliableCommands = thisAssembly.DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(Command)))
                .Select(x => (Command)Activator.CreateInstance(x))
                .ToImmutableDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);

            return true;
        }

        private static bool TryReadConfig()
        {
            try
            {
                using (var configFile = File.OpenRead(ConfigFileName))
                {
                    Config = Configuration.Parse(configFile);
                    return true;
                }
            }
            catch (JsonReaderException)
            {
                ReportError($"The configuration file ('{ConfigFileName}') appears to be malformed. ");
                LogInfo("Restoring the original configuration...");
                BackupExistingConfig();
                return TryRestoreDefaultConfig();
            }
            catch (Exception)
            {
                return TryRestoreDefaultConfig();
            }

            bool TryRestoreDefaultConfig()
            {
                using (var defaultConfigFile = Configuration.GetDefaultConfigStream())
                using (var reader = new StreamReader(defaultConfigFile))
                {
                    string json = reader.ReadToEnd();
                    Config = Configuration.Parse(json);
                    try
                    {
                        File.WriteAllText(ConfigFileName, json);
                        return true;
                    }
                    catch (IOException)
                    {
                        ReportError("Could not access the configuration file. It is likely used to another process.");
                        return false;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        ReportError("Access to the configuration file was denied by the system. ");
                        return false;
                    }
                }
            }

            void BackupExistingConfig()
            {
                try
                {
                    File.Move(ConfigFileName, ConfigFileName + ".bak");
                }
                catch
                {
                    // If the file cannot be accessed, RestoreDefaultConfig() will fail and report the error.
                }
            }
        }

        private static void ConstructSupportedGamesDictionary()
        {
            var gameAliases = ImmutableDictionary.CreateBuilder<string, ImmutableArray<string>>();
            var supportedGames = ImmutableDictionary.CreateBuilder<string, SC3Game>();
            foreach (var game in GameSpecificData.KnownGames)
            {
                var data = GameSpecificData.For(game);
                gameAliases[data.FriendlyName] = data.Aliases;
                foreach (var alias in data.Aliases)
                {
                    supportedGames[alias] = game;
                }
            }

            GameAliases = gameAliases.ToImmutable();
            SupportedGames = supportedGames.ToImmutable();
        }

        public static void LogInfo(string text, bool highlight = false)
        {
            if (highlight)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(text);
            }
        }

        public static void ReportError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message);
            Console.ResetColor();
        }
    }
}
