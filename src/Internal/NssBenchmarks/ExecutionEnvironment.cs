using SciAdvNet.NSScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace NssBenchmarks
{
    public sealed class ExecutionEnvironment : IExecutionEnvironment, IDisposable
    {
        private readonly ZipArchive _scripts;
        private readonly Dictionary<string, ConstantValue> _variables;

        public ExecutionEnvironment()
        {
            //_scripts = ZipFile.OpenRead("ChaosHeadScripts.zip");
            _variables = new Dictionary<string, ConstantValue>();
            _scripts = ZipFile.OpenRead("ChaosHeadScripts.zip");
        }

        public ConstantValue GetConstant(string constantName)
        {
            return default(ConstantValue);
        }

        public Stream OpenScript(string fileName)
        {
            //string path = Path.Combine("Scripts", fileName.Replace("nss/", string.Empty));
            return _scripts.GetEntry(fileName)?.Open();
        }

        public ConstantValue GetVariable(string variableName)
        {
            ConstantValue value;
            _variables.TryGetValue(variableName, out value);
            return value;
        }

        public void SetVariable(string variableName, ConstantValue value)
        {
            _variables[variableName] = value;
        }

        public void Dispose()
        {
            _scripts.Dispose();
        }
    }
}
