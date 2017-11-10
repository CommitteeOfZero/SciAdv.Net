using SciAdvNet.SC3Script;
using System;
using System.CommandLine;
using System.IO;
using static SC3Tools.Program;
using static SC3Tools.CommonFunctionality;

namespace SC3Tools
{
    internal class ReplaceTextCommand : Command
    {
        private SC3Game _game;
        private string _userCharacters = string.Empty;
        private string _scriptInput;
        private string _txtInput;

        public ReplaceTextCommand() : base("replace-text", "Rewrite the string table of a script with the contents of a text file.")
        {
        }

        public override void DefineParameters(ArgumentSyntax syntax)
        {
            DefineLanguageOption(syntax, ref _userCharacters);
            syntax.DefineParameter("scripts", ref _scriptInput, "The path to the input script file or a wildcard pattern.");
            syntax.DefineParameter("text-files", ref _txtInput, "The path to the input text file or a wildcard pattern.");
            DefineGameParameter(syntax, ref _game);
        }

        public override bool Execute()
        {
            bool isWildcardPattern = IsWildcardPattern(_txtInput);
            
            string txtDirectory = Path.GetDirectoryName(_txtInput);
            foreach (string scriptPath in EnumerateFiles(_scriptInput))
            {
                if (TryLoadScript(scriptPath, _game, out var script))
                {
                    LogInfo($"Processing '{scriptPath}'...");
                    string txtFileName;
                    if (isWildcardPattern)
                    {
                        txtFileName = Path.Combine(txtDirectory, Path.GetFileName(scriptPath) + ".txt");
                    }
                    else
                    {
                        txtFileName = _txtInput;
                    }

                    using (script)
                    {
                        TryReplaceText(script, txtFileName, _userCharacters);
                    }
                }

                Console.WriteLine();
            }

            return false;
        }
    }
}
