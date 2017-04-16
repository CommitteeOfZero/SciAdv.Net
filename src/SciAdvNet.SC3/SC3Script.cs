using SciAdvNet.Common;
using SciAdvNet.SC3.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SciAdvNet.SC3
{
    public sealed class SC3Script : IDisposable
    {
        private const string SC3Signature = "SC3\0";
        private const int HeaderSize = 12;

        private readonly bool _leaveOpen;
        private int _stringTableOffset;
        private int _stringTableEndOffset;
        private int _stringHeapOffset;
        private readonly List<StringHandle> _stringTable;
        private readonly Dictionary<int, ImmutableArray<byte>> _pendingStringUpdates;

        private SC3Script(Stream stream, string fileName, SC3Game game, bool leaveOpen)
        {
            _leaveOpen = leaveOpen;
            Stream = stream;
            FileName = fileName;
            Reader = new BinaryReader(Stream);
            Writer = new BinaryWriter(Stream);

            var headerBytes = Reader.ReadBytes(HeaderSize);
            ParseHeader(headerBytes);

            Game = game;
            if (game == SC3Game.Unknown)
            {
                Game = IdentifyGame();
            }

            GameSpecificData = GameSpecificData.For(Game);

            _stringTable = new List<StringHandle>();
            _pendingStringUpdates = new Dictionary<int, ImmutableArray<byte>>();

            ReadMetadata();
            Stream.Position = 0;
        }

        public string FileName { get; }
        public SC3Game Game { get; }
        public GameSpecificData GameSpecificData { get; }
        public ImmutableArray<BlockDefinition> Blocks { get; private set; }
        public CodeBlockDefinition StartBlock => Blocks[0].AsCode();
        public IReadOnlyList<StringHandle> StringTable => new ReadOnlyCollection<StringHandle>(_stringTable);

        internal Stream Stream { get; }
        internal BinaryReader Reader { get; }
        internal BinaryWriter Writer { get; }

        public static SC3Script Load(Stream stream, string fileName, SC3Game game, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead || !stream.CanSeek)
            {
                throw new ArgumentException("Stream must support read and seek operations.", nameof(stream));
            }

            return new SC3Script(stream, fileName, game, leaveOpen);
        }

        public static SC3Script Load(string path, SC3Game game)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string fileName = Path.GetFileName(path);
            var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            return new SC3Script(stream, fileName, game, leaveOpen: false);
        }

        public SC3String GetString(int id)
        {
            return SC3String.FromBytes(StringTable[id].RawData.ToArray(), Game);
        }

        public void UpdateString(int id, ImmutableArray<byte> bytes)
        {
            _pendingStringUpdates[id] = bytes;
        }

        public void ApplyPendingUpdates()
        {
            if (_pendingStringUpdates.Count == 0)
            {
                return;
            }

            var encodedStrings = _stringTable.Select(x => x.RawData).ToArray();
            foreach (var update in _pendingStringUpdates)
            {
                encodedStrings[update.Key] = update.Value;
            }

            // Rewrite the string heap and update the string handles first
            Writer.Seek(_stringHeapOffset, SeekOrigin.Begin);
            for (int i = 0; i < encodedStrings.Length; i++)
            {
                byte[] current = encodedStrings[i].ToArray();
                int offset = (int)Stream.Position;

                Writer.Write(current);
                _stringTable[i] = new StringHandle(this, i, offset, current.Length);
            }

            // Then rewrite the string table using the updated handles
            Writer.Seek(_stringTableOffset, SeekOrigin.Begin);
            foreach (var entry in _stringTable)
            {
                Writer.Write(entry.Offset);
            }
        }

        private SC3Game IdentifyGame()
        {
            return GameSpecificData.KnownGames.SingleOrDefault(game =>
            {
                var data = GameSpecificData.For(game);
                return data.KnownScripts.Any(expr => Regex.IsMatch(FileName, expr, RegexOptions.IgnoreCase));
            });
        }

        private void ParseHeader(byte[] headerBytes)
        {
            using (var stream = new MemoryStream(headerBytes))
            using (var header = new BinaryReader(stream))
            {
                string magic = new string(header.ReadChars(4));
                if (magic != SC3Signature)
                {
                    throw new InvalidDataException("The signature in the header is not correct.");
                }

                _stringTableOffset = header.ReadInt32();
                _stringTableEndOffset = header.ReadInt32();
            }
        }

        private void ReadMetadata()
        {
            int codeStartOffset = Reader.PeekInt32();
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

                    int currEnd = idxNext < stringOffsets.Count ? stringOffsets[idxNext] : (int)Stream.Length;
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
            Stream.Position = startOffset;
            do
            {
                int entry = Reader.ReadInt32();
                yield return entry;
            } while (Stream.Position < endOffset);
        }

        public void Dispose()
        {
            ReleaseResources();
        }

        private void ReleaseResources()
        {
            if (!_leaveOpen)
            {
                Reader.Dispose();
                Writer.Dispose();
                Stream.Dispose();
            }
        }
    }
}
