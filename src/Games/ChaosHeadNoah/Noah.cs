using SciAdvNet.Engine;
using SciAdvNet.NSScript;
using System.IO;
using System.IO.Compression;

namespace ChaosHeadNoah
{
    public sealed class Noah : Game
    {
        private readonly ZipArchive _content = ZipFile.OpenRead("Content.zip");

        public Noah(Platform platform, GameWindow gameWindow)
            : base(platform, gameWindow)
        {
        }

        public override Stream GetAsset(string assetName) => _content.GetEntry(assetName).Open();

        public void Run()
        {
            //var stream = GetAsset("nss/ch01_007_円山町殺人現場.nss");
            //var root = NSScript.ParseScript(stream);
            //var method = root.Methods[0];

            //var interpreter = new InterpretingVisitor(new NssExports(this));
            //interpreter.Visit(method);
        }
    }
}
