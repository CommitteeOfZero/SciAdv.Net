using SciAdvNet.SC3Script;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static SC3Tools.Program;

namespace SC3Tools
{
    internal static class CommonFunctionality
    {
        public static string GetInputDirectory(string inputPath)
        {
            string directory = Path.GetDirectoryName(inputPath);
            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            return directory;
        }

        public static string GetOutputTextFileName(string scriptFilePath) => Path.GetFileName(scriptFilePath) + ".txt";
        public static string GetOutputTextFilePath(string scriptFilePath, string outputDirectoryName = "")
        {
            string dir = Path.Combine(Path.GetDirectoryName(scriptFilePath), outputDirectoryName);
            return Path.Combine(dir, GetOutputTextFileName(scriptFilePath));
        }

        public static string CreateOutputDirectory(string inputPath, string outputDirectoryName)
        {
            string path = Path.Combine(Path.GetDirectoryName(inputPath), outputDirectoryName);
            Directory.CreateDirectory(path);
            return path;
        }

        public static bool TryCreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                ReportError(e.Message);
                return false;
            }
        }

        public static bool IsWildcardPattern(string s) => s.Contains("*.");
        public static IEnumerable<string> EnumerateFiles(string pathOrPattern)
        {
            try
            {
                return Directory.EnumerateFiles(GetInputDirectory(pathOrPattern), Path.GetFileName(pathOrPattern));
            }
            catch (Exception e)
            {
                ReportError(e.Message);
                return Enumerable.Empty<string>();
            }
        }

        public static bool TryLoadScript(string path, SC3Game game, out SC3Script script)
        {
            try
            {
                script = SC3Script.Load(path, game);
                return true;
            }
            catch (InvalidDataException)
            {
                ReportError($"{path} is not a valid SC3 script.");
                script = null;
                return false;
            }
            catch (Exception e)
            {
                ReportError(e.Message);
                script = null;
                return false;
            }
        }

        public static bool TryExtractText(SC3Script script, string outputFilePath, string userCharacters, bool normalize)
        {
            try
            {
                var strings = script.ExtractStrings(userCharacters, normalize);
                File.WriteAllLines(outputFilePath, strings);
                return true;
            }
            catch (Exception e)
            {
                ReportError(e.Message);
                return false;
            }
        }

        public static bool TryReplaceText(SC3Script script, string textFilePath, string userCharacters)
        {
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(textFilePath);
            }
            catch (Exception e)
            {
                ReportError(e.Message);
                return false;
            }
            
            try
            {
                script.UpdateStringTable(lines, userCharacters);
                int nbChanges = script.PendingStringUpdateCount;
                if (nbChanges > 0)
                {
                    LogInfo("Rewriting the string table...");
                    script.ApplyPendingUpdates();
                    LogInfo($"Success. {nbChanges} out of {script.StringTable.Count} strings were changed.", highlight: true);
                }
                else
                {
                    LogInfo("None of the strings were changed.");
                }

                return true;
            }
            catch (StringReplacementFailed e)
            {
                ReportError(e.Message);
                return false;
            }
        }

        public static bool TryLaunchEditor((string path, string args) editorInfo, string fileName, out Process process)
        {
            string path = editorInfo.path;
            // Replace environment variable names (such as %ProgramFiles%) with their values
            foreach (Match match in Regex.Matches(editorInfo.path, "%.*%"))
            {
                string specialFolder = Environment.GetEnvironmentVariable(match.Value.Substring(1, match.Value.Length - 2));
                path = path.Replace(match.Value, specialFolder.Replace("\\", "/"));
            }

            try
            {
                process = Process.Start(path, fileName + " " + editorInfo.args);
                return true;
            }
            catch (FileNotFoundException)
            {
                NotFound();
                process = null;
                return false;
            }
            catch (Win32Exception)
            {
                NotFound();
                process = null;
                return false;
            }

            void NotFound() => ReportError($"Could not locate the text editor ('{path}').");
        }
    }
}
