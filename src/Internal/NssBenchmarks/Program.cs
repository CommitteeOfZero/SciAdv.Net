using BenchmarkDotNet.Running;

namespace NssBenchmarks
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            //new Benchmarks().ParseBoot();
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
