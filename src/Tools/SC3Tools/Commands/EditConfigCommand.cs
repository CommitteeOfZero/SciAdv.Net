using System.CommandLine;
using static SC3Tools.Program;
using static SC3Tools.CommonFunctionality;

namespace SC3Tools
{
    internal class EditConfigCommand : Command
    {
        public EditConfigCommand() : base("config", "Edit the configuration file using the default editor.")
        {
        }

        public override void DefineParameters(ArgumentSyntax syntax) { }
        public override bool Execute() => TryLaunchEditor(DefaultTextEditor, ConfigFileName, out _);
    }
}
