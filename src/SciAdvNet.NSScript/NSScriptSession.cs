using System.Collections.Generic;
using System.Collections.Immutable;

namespace SciAdvNet.NSScript
{
    public sealed class NSScriptSession
    {
        private const string NssExtension = ".nss";
        private readonly IExecutionEnvironment _env;
        private Dictionary<string, ScriptRoot> _syntaxTrees;
        private Dictionary<string, NSScript> _loadedModules;

        public NSScriptSession(IExecutionEnvironment executionEnvironment)
        {
            _env = executionEnvironment;
            _syntaxTrees = new Dictionary<string, ScriptRoot>();
            _loadedModules = new Dictionary<string, NSScript>();
        }

        public NSScript GetModule(string moduleName)
        {
            NSScript module;
            if (_loadedModules.TryGetValue(moduleName, out module))
            {
                return module;
            }

            return LoadModule(moduleName);
        }

        private NSScript LoadModule(string moduleName)
        {
            if (!moduleName.Contains(NssExtension))
            {
                moduleName = moduleName + NssExtension;
            }

            var syntaxTrees = ImmutableArray.CreateBuilder<ScriptRoot>();
            var main = GetSyntaxTree(moduleName);
            syntaxTrees.Add(main);

            foreach (string include in main.Includes)
            {
                ScriptRoot tree;
                try
                {
                    tree = GetSyntaxTree(include);
                }
                catch
                {
                    continue;
                }

                syntaxTrees.Add(tree);
            }

            return new NSScript(moduleName, syntaxTrees.ToImmutable());
        }

        private ScriptRoot GetSyntaxTree(string fileName)
        {
            ScriptRoot treeRoot;
            if (_syntaxTrees.TryGetValue(fileName, out treeRoot))
            {
                return treeRoot;
            }

            var stream = _env.OpenScript(fileName);
            treeRoot = NSScript.ParseScript(fileName, stream);
            stream.Dispose();
            return treeRoot;
        }
    }
}
