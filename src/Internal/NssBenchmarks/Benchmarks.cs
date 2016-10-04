using BenchmarkDotNet.Attributes;
using SciAdvNet.NSScript;

namespace NssBenchmarks
{
    public class Benchmarks
    {
        [Benchmark]
        public void ParseBoot()
        {
            LoadModule("nss/boot");
        }

        [Benchmark]
        public void ParseFirstChapter()
        {
            LoadModule("nss/boot_第一章");
        }

        private static void LoadModule(string moduleName)
        {
            using (var env = new ExecutionEnvironment())
            {
                var session = new NSScriptSession(env);
                session.GetModule(moduleName);
            }
        }
    }
}
