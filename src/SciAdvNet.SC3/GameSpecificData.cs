using SciAdvNet.SC3.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace SciAdvNet.SC3
{
    public sealed class GameSpecificData
    {
        private const string RootDirectoryName = "Data";
        private const string CommonInstructionStubsFileName = "InstructionStubs.Common.xml";
        private const string GameXmlFileName = "Game.xml";
        private const string InstructionStubsFileName = "InstructionStubs.xml";
        private const string CharacterSetFileName = "Charset.utf8";
        private const string CompoundCharacterTableFileName = "CompoundCharacters.tbl";

        private static readonly Dictionary<SC3Game, GameSpecificData> s_instances;
        private static readonly Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>> s_commonInstructionStubs;

        private readonly Lazy<string> _characterSet;
        private readonly Lazy<ImmutableDictionary<int, string>> _compoundCharacters;
        private readonly Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>> _opcodeTable;

        static GameSpecificData()
        {
            KnownGames = Enum.GetValues(typeof(SC3Game)).Cast<SC3Game>().ToImmutableArray();

            s_commonInstructionStubs = new Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>>(ReadCommonInstructionStubs);
            s_instances = new Dictionary<SC3Game, GameSpecificData>();
            foreach (SC3Game game in KnownGames)
            {
                s_instances[game] = new GameSpecificData(game);
            }
        }

        private GameSpecificData(SC3Game game)
        {
            Game = game;
            ReadGameXml();

            _opcodeTable = new Lazy<ImmutableDictionary<ImmutableArray<byte>, InstructionStub>>(ReadInstructionStubs);
            _characterSet = new Lazy<string>(ReadCharset);
            _compoundCharacters = new Lazy<ImmutableDictionary<int, string>>(ReadCompoundCharacterTable);
        }

        public SC3Game Game { get; }
        public string GameFriendlyName { get; private set; }
        public Range<int> ReservedCharacterRange { get; private set; }
        public string CharacterSet => _characterSet.Value;
        public ImmutableDictionary<int, string> CompoundCharacters => _compoundCharacters.Value;
        public ImmutableDictionary<ImmutableArray<byte>, InstructionStub> OpcodeTable => _opcodeTable.Value;

        internal static ImmutableArray<SC3Game> KnownGames { get; }
        internal ImmutableArray<string> KnownScripts { get; private set; }

        public static GameSpecificData For(SC3Game game)
        {
            return s_instances[game];
        }

        private static ImmutableDictionary<ImmutableArray<byte>, InstructionStub> ReadCommonInstructionStubs()
        {
            var assembly = typeof(SC3Game).GetTypeInfo().Assembly;
            string resourceNamespace = typeof(SC3Game).Namespace;
            string fullName = $"{resourceNamespace}.{RootDirectoryName}.{CommonInstructionStubsFileName}";

            using (var stream = assembly.GetManifestResourceStream(fullName))
            {
                return InstructionStubsParser.Parse(stream);
            }
        }

        private ImmutableDictionary<ImmutableArray<byte>, InstructionStub> ReadInstructionStubs()
        {
            var commonStubs = s_commonInstructionStubs.Value;
            var stream = GetResourceFileStream(InstructionStubsFileName);
            if (stream == null)
            {
                return commonStubs;
            }

            var gameSpecificStubs = InstructionStubsParser.Parse(stream);
            var allStubs = commonStubs.ToBuilder();
            foreach (var stub in gameSpecificStubs)
            {
                allStubs[stub.Key] = stub.Value;
            }

            return allStubs.ToImmutable();
        }

        private void ReadGameXml()
        {
            var stream = GetResourceFileStream(GameXmlFileName);
            if (stream == null)
            {
                GameFriendlyName = "Unknown";
                KnownScripts = ImmutableArray<string>.Empty;
                return;
            }

            using (stream)
            {
                var root = XDocument.Load(stream).Root;
                GameFriendlyName = root.Attribute("FriendlyName").Value;
                string knownScriptsString = root.Descendants("KnownScripts").Single().Value;
                KnownScripts = knownScriptsString.Split(';').ToImmutableArray();

                var reservedRangeElement = root.Descendants("ReservedCharacterRange").SingleOrDefault();
                if (reservedRangeElement != null)
                {
                    int rangeStart = int.Parse(reservedRangeElement.Attribute("Start").Value, NumberStyles.AllowHexSpecifier);
                    int rangeEnd = int.Parse(reservedRangeElement.Attribute("End").Value, NumberStyles.AllowHexSpecifier);

                    ReservedCharacterRange = new Range<int>(rangeStart, rangeEnd);
                }
            }
        }

        private string ReadCharset()
        {
            var stream = GetResourceFileStream(CharacterSetFileName);
            if (stream == null)
            {
                return string.Empty;
            }

            using (stream)
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private ImmutableDictionary<int, string> ReadCompoundCharacterTable()
        {
            var stream = GetResourceFileStream(CompoundCharacterTableFileName);
            if (stream == null)
            {
                return ImmutableDictionary<int, string>.Empty;
            }

            using (stream)
            using (var reader = new StreamReader(stream))
            {
                var table = ImmutableDictionary.CreateBuilder<int, string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var parts = line.Split('=');
                        string strIndex = parts[0].Substring(1, parts[0].Length - 2);
                        string value = parts[1];

                        parts = strIndex.Split('-');
                        if (parts.Length == 1)
                        {
                            int idx = BinaryUtils.HexStrToInt32(parts[0]);
                            table[idx] = value;
                        }
                        else
                        {
                            int rangeStart = BinaryUtils.HexStrToInt32(parts[0]);
                            int rangeEnd = BinaryUtils.HexStrToInt32(parts[1]);
                            for (int i = rangeStart; i <= rangeEnd; i++)
                            {
                                table[i] = value;
                            }
                        }
                    }
                }

                return table.ToImmutable();
            }
        }

        private Stream GetResourceFileStream(string name)
        {
            var assembly = typeof(SC3Game).GetTypeInfo().Assembly;
            string resourceNamespace = typeof(SC3Game).Namespace;
            string fullName = $"{resourceNamespace}.{RootDirectoryName}.{Game}.{name}";
            return assembly.GetManifestResourceStream(fullName);
        }
    }

    public struct Range<T> where T : IComparable<T>
    {
        public Range(T start, T end)
        {
            Start = start;
            End = end;
        }

        public T Start { get; }
        public T End { get; }

        public bool ContainsValue(T value) => value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;
    }
}
