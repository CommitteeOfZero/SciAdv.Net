using SciAdvNet.NSScript;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;

namespace NssInteractive
{
    public sealed class ExecutionEnvironment : IExecutionEnvironment
    {
        private Dictionary<string, ConstantValue> _variables;

        public ExecutionEnvironment()
        {
            _variables = new Dictionary<string, ConstantValue>();
        }

        public ConstantValue GetConstant(string constantName)
        {
            return default(ConstantValue);
        }

        public Stream OpenScript(string fileName)
        {
            return File.OpenRead(fileName);
        }

        public ConstantValue GetVariable(string variableName)
        {
            ConstantValue value;
            _variables.TryGetValue(variableName, out value);

            if (value == null)
            {
                value = ConstantValue.Zero;
            }

            return value;
        }

        public void SetVariable(string variableName, ConstantValue value)
        {
            _variables[variableName] = value;
        }
    }
}
