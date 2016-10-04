using SciAdvNet.Common;
using SciAdvNet.SC3.Text;
using SciAdvNet.SC3.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SciAdvNet.SC3
{
    public sealed class SC3Module : IDisposable
    {
        private const int HeaderSize = 12;

        private readonly bool _leaveOpen;
        private int _stringTableOffset;
        private int _stringTableEndOffset;
        private int _stringHeapOffset;
        private readonly List<StringHandle> _stringTable;
        private readonly Dictionary<int, byte[]> _pendingStringUpdates;

        private SC3Module(Stream stream, bool leaveOpen)
        {
            _leaveOpen = leaveOpen;
            ModuleStream = stream;
            ModuleReader = new BinaryReader(ModuleStream);
            ModuleWriter = new BinaryWriter(ModuleStream);

            _stringTable = new List<StringHandle>();
            _pendingStringUpdates = new Dictionary<int, byte[]>();

            var headerBytes = ModuleReader.ReadBytes(HeaderSize);
            try
            {
                Game = IdentifySC3Game(headerBytes);
                GameSpecificData = GameSpecificData.For(Game);
            }
            catch
            {
                ReleaseResources();
                throw new InvalidDataException("The specified file is not a supported SC3 module.");
            }

            ParseHeader(headerBytes);
            ReadMetadata();

            ModuleStream.Position = 0;
        }

        public SC3Game Game { get; }
        public GameSpecificData GameSpecificData { get; }
        public ImmutableArray<BlockDefinition> Blocks { get; private set; }
        public CodeBlockDefinition StartBlock => Blocks[0].AsCode();
        public IReadOnlyList<StringHandle> StringTable => new ReadOnlyCollection<StringHandle>(_stringTable);

        internal Stream ModuleStream { get; }
        internal BinaryReader ModuleReader { get; }
        internal BinaryWriter ModuleWriter { get; }

        public static SC3Module Load(Stream stream, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead || !stream.CanSeek)
            {
                throw new ArgumentException(nameof(stream));
            }

            return new SC3Module(stream, leaveOpen);
        }

        public static SC3Module Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            return new SC3Module(stream, leaveOpen: false);
        }

        public SC3String GetString(int id)
        {
            return SC3String.FromBytes(StringTable[id].RawData.ToArray(), Game);
        }

        public void UpdateString(int id, SC3String updatedString)
        {
            var bytes = updatedString.Encode(Game).ToArray();
            _pendingStringUpdates[id] = bytes;
        }

        public void ApplyPendingUpdates()
        {
            if (_pendingStringUpdates.Count == 0)
            {
                return;
            }

            var encodedStrings = _stringTable.Select(x => x.RawData.ToArray()).ToArray();
            foreach (var update in _pendingStringUpdates)
            {
                encodedStrings[update.Key] = update.Value;
            }

            // Rewrite the string heap and update the string handles first
            ModuleWriter.Seek(_stringHeapOffset, SeekOrigin.Begin);
            for (int i = 0; i < encodedStrings.Length; i++)
            {
                var s = encodedStrings[i];
                int offset = (int)ModuleStream.Position;

                ModuleWriter.Write(s);
                _stringTable[i] = new StringHandle(this, i, offset, s.Length);
            }

            // Then rewrite the string table using the updated handles
            ModuleWriter.Seek(_stringTableOffset, SeekOrigin.Begin);
            foreach (var entry in _stringTable)
            {
                ModuleWriter.Write(entry.Offset);
            }
        }

        private SC3Game IdentifySC3Game(byte[] headerBytes)
        {
            string strHeader = BinaryUtils.BytesToHexString(headerBytes);
            var supportedGames = GameSpecificData.SupportedGames;
            return supportedGames.Single(game => GameSpecificData.For(game).SupportedModules.Contains(strHeader));
        }

        private void ParseHeader(byte[] headerBytes)
        {
            using (var stream = new MemoryStream(headerBytes))
            using (var header = new BinaryReader(stream))
            {
                string magic = new string(header.ReadChars(4));
                _stringTableOffset = header.ReadInt32();
                _stringTableEndOffset = header.ReadInt32();
            }
        }

        private void ReadMetadata()
        {
            int codeStartOffset = ModuleReader.PeekInt32();
            var blockOffsets = ReadInt32Array(startOffset: HeaderSize, endOffset: codeStartOffset).ToList();
            var blocks = ImmutableArray.CreateBuilder<BlockDefinition>(blockOffsets.Count);
            for (int idxNext = 1; idxNext <= blockOffsets.Count; idxNext++)
            {
                int idxCurr = idxNext - 1;
                int currStart = blockOffsets[idxCurr];

                int length;
                if (idxNext < blockOffsets.Count)
                {
                    if (blockOffsets[idxNext] > blockOffsets[idxCurr])
                    {
                        length = blockOffsets[idxNext] - blockOffsets[idxCurr];
                    }
                    else
                    {
                        int currEnd = idxNext + 1 < blockOffsets.Count ? blockOffsets[idxNext + 1] : _stringTableEndOffset;
                        length = currEnd - blockOffsets[idxCurr];
                    }
                }
                else
                {
                    length = _stringTableOffset - blockOffsets[idxCurr];
                }

                blocks.Add(new BlockDefinition(this, idxCurr, currStart, length));
            }

            Blocks = blocks.ToImmutable();

            if (_stringTableOffset != _stringTableEndOffset)
            {
                var stringOffsets = ReadInt32Array(startOffset: _stringTableOffset, endOffset: _stringTableEndOffset).ToList();
                for (int idxNext = 1; idxNext <= stringOffsets.Count; idxNext++)
                {
                    int idxCurr = idxNext - 1;
                    int currStart = stringOffsets[idxCurr];

                    int currEnd = idxNext < stringOffsets.Count ? stringOffsets[idxNext] : (int)ModuleStream.Length;
                    int length = currEnd - currStart;

                    _stringTable.Add(new StringHandle(this, idxCurr, currStart, length));
                }
            }
            
            if (_stringTable.Any())
            {
                _stringHeapOffset = _stringTable.First().Offset;
            }
        }

        private IEnumerable<int> ReadInt32Array(int startOffset, int endOffset)
        {
            ModuleStream.Position = startOffset;
            do
            {
                int entry = ModuleReader.ReadInt32();
                yield return entry;
            } while (ModuleStream.Position < endOffset);
        }

        public void Dispose()
        {
            ApplyPendingUpdates();
            ReleaseResources();
        }

        private void ReleaseResources()
        {
            if (!_leaveOpen)
            {
                ModuleReader.Dispose();
                ModuleWriter.Dispose();
                ModuleStream.Dispose();
            }
        }
    }
}
