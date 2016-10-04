using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SupportedModulesGenerator
{
    public sealed class Program
    {
        private const string OutputFileName = "SupportedModules.lst";
        private const string ScrExtension = "scr";
        private const string ScxExtension = "scx";

        public static void Main(string[] args)
        {
            var moduleHeaders = new HashSet<string>();
            var modules = Directory.EnumerateFiles(Environment.CurrentDirectory, "*", SearchOption.AllDirectories)
                .Where(x => x.ToLowerInvariant().EndsWith(ScrExtension) || x.ToLowerInvariant().EndsWith(ScxExtension));

            foreach (string module in modules)
            {
                using (var stream = new FileStream(module, FileMode.Open, FileAccess.Read))
                using (var reader = new BinaryReader(stream))
                {
                    var header = reader.ReadBytes(12);
                    string strHeader = BitConverter.ToString(header).Replace("-", string.Empty);
                    moduleHeaders.Add(strHeader);
                }
            }

            File.WriteAllLines(OutputFileName, moduleHeaders.ToArray());
        }
    }
}
