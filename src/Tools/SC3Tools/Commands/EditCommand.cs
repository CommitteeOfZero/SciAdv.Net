using SciAdvNet.SC3Script;
using System.CommandLine;
using System.IO;
using System.Linq;
using static SC3Tools.Program;
using static SC3Tools.CommonFunctionality;

namespace SC3Tools
{
    internal class EditCommand : Command
    {
        private string _inputScriptPath;
        private SC3Game _game;
        private string _userCharacters = string.Empty;
        private bool _preserveFullwidthChars;
        private (string path, string args) _textEditor = DefaultTextEditor;

        public EditCommand() : base("edit", "Edit a script using a specified text editor.")
        {
        }

        public override void DefineParameters(ArgumentSyntax syntax)
        {
            DefinePreserveFwOption(syntax, ref _preserveFullwidthChars);
            DefineLanguageOption(syntax, ref _userCharacters);
            syntax.DefineOption("editor", ref _textEditor, GetTextEditorInfo, $"[optional] The name of the editor to use (see '{ConfigFileName}').");
            DefineInputParameter(syntax, ref _inputScriptPath);
            DefineGameParameter(syntax, ref _game);

            (string path, string args) GetTextEditorInfo(string editorName)
            {
                return Config.Editors.TryGetValue(editorName, out var editor) ? editor : DefaultTextEditor;
            }
        }

        public override bool Execute()
        {
            if (TryLoadScript(_inputScriptPath, _game, out var script))
            {
                using (script)
                {
                    // Look for an existing text file first.
                    string dir = GetInputDirectory(_inputScriptPath);
                    string txtPath = Directory.EnumerateFiles(dir, GetOutputTextFileName(_inputScriptPath), SearchOption.AllDirectories)
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(txtPath))
                    {
                        LogInfo($"Opening the existing text file: '{txtPath}'.");
                    }
                    else
                    {
                        txtPath = GetOutputTextFilePath(_inputScriptPath, string.Empty);
                        TryExtractText(script, txtPath, _userCharacters, normalize: !_preserveFullwidthChars);
                    }

                    if (TryLaunchEditor(_textEditor, txtPath, out var process))
                    {
                        process.WaitForExit();
                        return TryReplaceText(script, txtPath, _userCharacters);
                    }
                }
            }

            return false;
        }
    }
}
