using SciAdvNet.NSScript;
using System;
using System.IO.Compression;
using System.Text;

namespace NssInteractive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string moduleName = "nss/logo";

            var env = new ExecutionEnvironment();
            var session = new NSScriptSession(env);

            var interpreter = new NSScriptInterpreter(env, new Exports());
            //try
            {
                interpreter.ExecuteModule(moduleName);
            }
            //catch (NssRuntimeErrorException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            Console.ReadLine();
        }
    }
}
