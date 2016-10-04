using System.IO;

namespace SciAdvNet.NSScript
{
    public interface IExecutionEnvironment
    {
        Stream OpenScript(string fileName);
        ConstantValue GetVariable(string variableName);
        void SetVariable(string variableName, ConstantValue value);
        ConstantValue GetConstant(string constantName);
    }
}
