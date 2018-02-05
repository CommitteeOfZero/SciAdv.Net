using System;
using System.CommandLine;

namespace SC3Tools
{
    internal class HelpCommand : Command
    {
        private string _commandName;

        public HelpCommand() : base("help", "Display help.")
        {
        }

        public override void DefineParameters(ArgumentSyntax syntax)
        {
            syntax.DefineParameter("command-name", ref _commandName, "The command for which to view more detailed help.");
        }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(_commandName))
            {
                GreetUser();
                return true;
            }

            return Program.Execute(new[] { _commandName, "--help" });
        }

        private void GreetUser()
        {
            Console.WriteLine("sc3tools 1.0 beta\n");
            Console.WriteLine("Available commands:");
            foreach (var command in Program.CommandList)
            {
                Console.WriteLine($"{command.Name,-20} {command.Help,-70}");
            }

            Console.WriteLine();
            Console.WriteLine("Supported games:");
            foreach (var kvp in Program.GameAliases)
            {
                Console.WriteLine($"{kvp.Key,-20} {string.Join(", ", kvp.Value),-50}");
            }

            Console.WriteLine();
            Console.WriteLine("Run 'sc3tools COMMAND --help' or 'sc3tools help COMMAND' to get more information on a command.\n");
        }
    }
}
