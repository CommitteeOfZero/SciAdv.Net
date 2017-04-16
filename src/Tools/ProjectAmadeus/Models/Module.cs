using SciAdvNet.SC3;
using System.IO;

namespace ProjectAmadeus.Models
{
    public sealed class Module
    {
        public Module(string scriptPath, SC3Script script, string configPath, ScriptConfig config)
        {
            ScriptPath = scriptPath;
            ScriptName = Path.GetFileName(scriptPath);
            ConfigPath = configPath;
            Script = script;
            Config = config;
        }

        public string ScriptPath { get; }
        public string ScriptName { get; }
        public string ConfigPath { get; }
        public SC3Script Script { get; }
        public ScriptConfig Config { get; }
    }
}
