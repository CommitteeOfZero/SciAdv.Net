using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SciAdvNet.SC3
{
    public sealed class GameSpecificData
    {
        private const string RootDirectoryName = "Data";
        private const string SupportedModulesFileName = "SupportedModules.lst";
        private const string CommonInstructionStubsFileName = "InstructionStubs.Common.xml";
        private const string InstructionStubsFileName = "InstructionStubs.xml";
        private const string CharacterSetFileName = "Charset.utf8";
        private const string PuaTableFileName = "PrivateUseCharacters.tbl";
        private const string GameResourcesFileName = "Resources.lst";

        private static readonly Dictionary<SC3Game, GameSpecificData> s_instances;

        private readonly Lazy<ImmutableArray<string>> _supportedModules;
        private readonly Lazy<string> _characterSet;
        private readonly Lazy<ImmutableDictionary<int, string>> _privateUseCharacters;
        private readonly Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>> _commonInstrStubs;
        private readonly Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>> _opcodeTable;

        static GameSpecificData()
        {
            SupportedGames = Enum.GetValues(typeof(SC3Game)).Cast<SC3Game>().ToImmutableArray();
            s_instances = new Dictionary<SC3Game, GameSpecificData>();
            foreach (SC3Game game in SupportedGames)
            {
                s_instances[game] = new GameSpecificData(game);
            }
        }

        private GameSpecificData(SC3Game game)
        {
            Game = game;

            _supportedModules = new Lazy<ImmutableArray<string>>(ReadSupportedModules);
            _commonInstrStubs = new Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>>(ReadCommonInstructionStubs);
            _opcodeTable = new Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>>(ReadInstructionStubs);
            _characterSet = new Lazy<string>(ReadCharset);
            _privateUseCharacters = new Lazy<ImmutableDictionary<int, string>>(ReadPuaCharacters);
        }

        public SC3Game Game { get; }
        public string CharacterSet => _characterSet.Value;
        public ImmutableDictionary<int, string> PrivateUseCharacters => _privateUseCharacters.Value;
        public ImmutableDictionary<ImmutableArray<byte>, InstructionStub> OpcodeTable => _opcodeTable.Value;

        public static ImmutableArray<SC3Game> SupportedGames { get; }
        internal ImmutableArray<string> SupportedModules => _supportedModules.Value;

        public static GameSpecificData For(SC3Game game)
        {
            return s_instances[game];
        }

        private ImmutableArray<string> ReadSupportedModules()
        {
            var builder = ImmutableArray.CreateBuilder<string>();
            using (var stream = GetResourceFileStream(SupportedModulesFileName))
            using (var reader = new StreamReader(stream))
            {
                string moduleHash;
                while ((moduleHash = reader.ReadLine()) != null)
                {
                    builder.Add(moduleHash);
                }
            }

            return builder.ToImmutable();
        }

        private ImmutableDictionary<ImmutableArray<byte>, InstructionStub> ReadCommonInstructionStubs()
        {
            using (var stream = GetResourceFileStream(CommonInstructionStubsFileName, gameSpecific: false))
            {
                return InstructionStubsParser.Parse(stream);
            }
        }

        private ImmutableDictionary<ImmutableArray<byte>, InstructionStub> ReadInstructionStubs()
        {
            var commonStubs = _commonInstrStubs.Value;
            var stream = GetResourceFileStream(InstructionStubsFileName);
            ImmutableDictionary<ImmutableArray<byte>, InstructionStub> gameSpecificStubs;
            if (stream != null)
            {
                using (stream)
                {
                    gameSpecificStubs = InstructionStubsParser.Parse(stream);
                }
            }
            else
            {
                gameSpecificStubs = ImmutableDictionary<ImmutableArray<byte>, InstructionStub>.Empty;
            }

            var bldStubs = commonStubs.ToBuilder();
            foreach (var stub in gameSpecificStubs)
            {
                bldStubs[stub.Key] = stub.Value;
            }

            return bldStubs.ToImmutable();
        }

        private string ReadCharset()
        {
            using (var stream = GetResourceFileStream(CharacterSetFileName))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private ImmutableDictionary<int, string> ReadPuaCharacters()
        {
            var stream = GetResourceFileStream(PuaTableFileName);
            if (stream == null)
            {
                return ImmutableDictionary<int, string>.Empty;
            }

            using (stream)
            {
                return PrivateUseCharacterTable.Parse(stream);
            }
        }

        private Stream GetResourceFileStream(string name, bool gameSpecific = true)
        {
            var assembly = typeof(SC3Game).GetTypeInfo().Assembly;
            string resourceNamespace = typeof(SC3Game).Namespace;
            string fullName;
            if (gameSpecific)
            {
                fullName = $"{resourceNamespace}.{RootDirectoryName}.{Game}.{name}";
            }
            else
            {
                fullName = $"{resourceNamespace}.{RootDirectoryName}.{name}";
            }

            return assembly.GetManifestResourceStream(fullName);
        }
    }
}
