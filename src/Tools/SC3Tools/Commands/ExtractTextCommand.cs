using SciAdvNet.SC3Script;
using System.CommandLine;
using System;
using static SC3Tools.Program;
using static SC3Tools.CommonFunctionality;

namespace SC3Tools
{
    internal class ExtractTextCommand : Command
    {
        private string _input;
        private SC3Game _game;
        private bool _preserveFullwidthChars;
        private string _userCharacters = string.Empty;

        public ExtractTextCommand() : base("extract-text", "Extract text from a script.")
        {
        }

        public override void DefineParameters(ArgumentSyntax syntax)
        {
            DefinePreserveFwOption(syntax, ref _preserveFullwidthChars);
            DefineLanguageOption(syntax, ref _userCharacters);
            DefineInputParameter(syntax, ref _input);
            DefineGameParameter(syntax, ref _game);
        }

        public override bool Execute()
        {
            string outputDirName = null;
            try
            {
                outputDirName = IsWildcardPattern(_input) ? CreateOutputDirectory(_input, "txt") : "";
            }
            catch (Exception e)
            {
                ReportError(e.Message);
                return false;
            }

            foreach (string inputPath in EnumerateFiles(_input))
            {
                if (TryLoadScript(inputPath, _game, out var script))
                {
                    LogInfo($"Extracting text from '{inputPath}...");
                    using (script)
                    {
                        string outputPath = GetOutputTextFilePath(inputPath, outputDirName);
                        if (TryExtractText(script, outputPath, _userCharacters, normalize: !_preserveFullwidthChars))
                        {
                            LogInfo($"Successfully extracted {script.StringTable.Count} lines from '{inputPath}'.", highlight: true);
                            LogInfo($"See '{outputPath}'.");
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                Console.WriteLine();
            }

            return false;
        }
    }
}
