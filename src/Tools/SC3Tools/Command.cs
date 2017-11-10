using SciAdvNet.SC3Script;
using System.CommandLine;
using System.Linq;

namespace SC3Tools
{
    internal abstract class Command
    {
        protected Command(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }

        public abstract void DefineParameters(ArgumentSyntax syntax);
        public abstract bool Execute();

        protected void DefineInputParameter(ArgumentSyntax syntax, ref string value)
        {
            syntax.DefineParameter("input", ref value, "The path to the input file or a wildcard pattern.");
        }

        protected void DefineGameParameter(ArgumentSyntax syntax, ref SC3Game value)
        {
            syntax.DefineParameter("game", ref value, ParseGameName, GetGameAliasList());

            string GetGameAliasList() => string.Join(", ", Program.GameAliases.Values.Select(x => string.Join("|", x)));
            SC3Game ParseGameName(string gameName)
            {
                if (!Program.SupportedGames.TryGetValue(gameName, out var game))
                {
                    UnrecognizedValue(syntax, "game", gameName);
                    return SC3Game.Unknown;
                }

                return game;
            }
        }

        protected void DefineLanguageOption(ArgumentSyntax syntax, ref string userCharacters)
        {
            syntax.DefineOption("lang", ref userCharacters, LocateCharsetForLanguage, $"[optional] {GetLanguageList()}");

            string GetLanguageList() => string.Join(", ", Program.Config.Languages.Keys);
            string LocateCharsetForLanguage(string lang)
            {
                string charset = string.Empty;
                if (!string.IsNullOrEmpty(lang) && !Program.Config.Languages.TryGetValue(lang, out charset))
                {
                    UnrecognizedValue(syntax, "lang", lang);
                }

                return charset;
            }
        }

        protected void DefinePreserveFwOption(ArgumentSyntax syntax, ref bool value)
        {
            syntax.DefineOption("preserve-fw", ref value, "[optional] Preserve fullwidth characters.");
        }

        protected void UnrecognizedValue(ArgumentSyntax syntax, string parameterName, string value)
        {
            syntax.ReportError($"Unrecognized value for parameter '{parameterName}': '{value}'.");
        }
    }
}
