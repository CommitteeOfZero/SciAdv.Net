using System;
using System.IO;
using System.IO.Compression;
using Xunit;

namespace SciAdvNet.NSScript.Tests
{
    public sealed class ChaosHeadTests : IClassFixture<NSScriptSessionFixture>
    {
        private readonly NSScriptSessionFixture _fixture;

        public ChaosHeadTests(NSScriptSessionFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ParsingBootWorks()
        {
            _fixture.Session.GetModule("nss/boot");
        }

        [Fact]
        public void ParsingFirstChapterWorks()
        {
            _fixture.Session.GetModule("nss/boot_第一章");
        }
    }

    public sealed class NSScriptSessionFixture : IDisposable
    {
        private readonly ExecutiongEnvironment _env;

        public NSScriptSessionFixture()
        {
            _env = new ExecutiongEnvironment();
            Session = new NSScriptSession(_env);
        }

        public NSScriptSession Session { get; }

        public void Dispose()
        {
            _env.Dispose();
        }
    }

    public sealed class ExecutiongEnvironment : IExecutionEnvironment, IDisposable
    {
        private readonly ZipArchive _archive = ZipFile.OpenRead("Data/ChaosHeadScripts.zip");

        public Stream OpenScript(string fileName)
        {
            return _archive.GetEntry(fileName).Open();
        }

        public ConstantValue GetConstant(string constantName)
        {
            throw new NotImplementedException();
        }

        public ConstantValue GetVariable(string variableName)
        {
            throw new NotImplementedException();
        }

        public void SetVariable(string variableName, ConstantValue value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _archive.Dispose();
        }
    }
}
