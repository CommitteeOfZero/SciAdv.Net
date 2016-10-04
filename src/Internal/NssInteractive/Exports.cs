using SciAdvNet.NSScript;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace NssInteractive
{
    public sealed class Exports : NssExports
    {
        public override void Wait(TimeSpan delay)
        {
            //Thread.Sleep(delay);
        }

        public override void Unknown(string methodName, ImmutableArray<ConstantValue> args)
        {
            var sb = new StringBuilder(methodName);
            sb.Append("(");

            if (args.Length > 0)
            {
                foreach (var arg in args.Take(args.Length - 1))
                {
                    sb.Append(arg?.RawValue ?? "null");
                    if (args.Length > 1)
                    {
                        sb.Append(", ");
                    }
                }

                sb.Append(args.LastOrDefault()?.RawValue ?? "null");
            }

            sb.Append(")");
            Console.WriteLine(sb.ToString());
        }
    }
}
