using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// Represents a character set specific to a particular MAGES Engine game.
    /// Defines methods for encoding and decoding characters.
    /// </summary>
    public sealed class CharacterSet
    {
        private static readonly Dictionary<SC3Game, EncodingMapSet> s_encodingMapSetCache;

        private readonly GameSpecificData _data;
        private readonly string _userChracters;

        private readonly EncodingMapSet _encodingMaps;

        static CharacterSet()
        {
            s_encodingMapSetCache = new Dictionary<SC3Game, EncodingMapSet>();
        }

        public CharacterSet(SC3Game game, string userCharacters)
        {
            Game = game;
            _data = GameSpecificData.For(game);
            _userChracters = userCharacters;

            if (!s_encodingMapSetCache.TryGetValue(game, out _encodingMaps))
            {
                _encodingMaps = ConstructEncodingMaps();
                s_encodingMapSetCache[game] = _encodingMaps;
            }
        }

        public SC3Game Game { get; }

        private static int CodeToIndex(ushort code) => code & 0x7FFF;

        public ushort EncodeCharacter(char character)
        {
            if (_encodingMaps.Main.TryGetValue(character, out ushort code))
            {
                return code;
            }
            if (_encodingMaps.User.TryGetValue(character, out code))
            {
                return code;
            }

            throw UnsupportedCharacter(character.ToString());
        }

        public ushort EncodeCompoundCharacter(string compoundCharacter)
        {
            return _encodingMaps.Compound.TryGetValue(compoundCharacter, out ushort code) ?
                code : throw UnsupportedCharacter(compoundCharacter);
        }

        /// <summary>
        /// Decodes exactly one character.
        /// If the specified character code corresponds to a compound character, <see cref="char.MaxValue"/> is returned.
        /// </summary>
        /// <param name="code">The character code.</param>
        public char DecodeCharacter(ushort code)
        {
            int index = CodeToIndex(code);
            if (index >= _data.CharacterSet.Length)
            {
                throw CharacterCodeOutOfRange(code);
            }

            char character = _data.CharacterSet[index];
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.PrivateUse)
            {
                return character;
            }

            int codepoint = character;
            if (_data.ReservedCharacterRange.ContainsValue(codepoint))
            {
                int offset = codepoint - _data.ReservedCharacterRange.Start;
                if (offset < _userChracters.Length)
                {
                    return _userChracters[offset];
                }
            }
            else if (_data.CompoundCharacters.ContainsKey(codepoint))
            {
                return char.MaxValue;
            }

            throw CharacterCodeOutOfRange(code);
        }

        public string DecodeCompoundCharacter(ushort code)
        {
            int index = CodeToIndex(code);
            if (index >= _data.CharacterSet.Length)
            {
                throw CharacterCodeOutOfRange(code);
            }

            int privateUseCodepoint = _data.CharacterSet[index];
            if (!_data.CompoundCharacters.TryGetValue(privateUseCodepoint, out string compound))
            {
                throw CharacterCodeOutOfRange(code);
            }

            return compound;
        }

        public bool Contains(char character)
        {
            return _data.CharacterSet.Contains(character);
        }

        private EncodingMapSet ConstructEncodingMaps()
        {
            var mainEncodingMap = new Dictionary<char, ushort>();
            for (int index = 0; index < _data.CharacterSet.Length; index++)
            {
                byte highByte = (byte)(0x80 + (byte)(index / 256));
                byte lowByte = (byte)(index % 256);

                char character = _data.CharacterSet[index];
                ushort code = (ushort)(highByte << 8 | lowByte);
                mainEncodingMap[character] = code;
            }

            var compoundCharsEncodingMap = new Dictionary<string, ushort>();
            foreach (var entry in _data.CompoundCharacters)
            {
                string compoundChar = entry.Value;
                int privateUseCodepoint = entry.Key;
                ushort code = mainEncodingMap[(char)privateUseCodepoint];
                compoundCharsEncodingMap[compoundChar] = code;
            }

            var userCharsEncodingMap = new Dictionary<char, ushort>();
            var reservedRange = _data.ReservedCharacterRange;
            for (int i = 0; i < _userChracters.Length; i++)
            {
                char character = _userChracters[i];
                int privateUseCodepoint = reservedRange.Start + i;
                if (!_data.ReservedCharacterRange.ContainsValue(privateUseCodepoint))
                {
                    throw new ArgumentException("Too many user specified characters.");
                }

                ushort code = mainEncodingMap[(char)privateUseCodepoint];
                userCharsEncodingMap[character] = code;
            }

            return new EncodingMapSet(mainEncodingMap, compoundCharsEncodingMap, userCharsEncodingMap);
        }

        private Exception UnsupportedCharacter(string character)
        {
            return new ArgumentException($"Unable to encode character '{character}': not present in the game's character set.");
        }

        private Exception CharacterCodeOutOfRange(ushort characterCode)
        {
            return new ArgumentOutOfRangeException(nameof(characterCode), $"Character code out of range: 0x{characterCode:X2}.");
        }

        private sealed class EncodingMapSet
        {
            public EncodingMapSet(IReadOnlyDictionary<char, ushort> main,
                IReadOnlyDictionary<string, ushort> compound,
                IReadOnlyDictionary<char, ushort> user)
            {
                Main = main;
                Compound = compound;
                User = user;
            }

            public IReadOnlyDictionary<char, ushort> Main { get; }
            public IReadOnlyDictionary<string, ushort> Compound { get; }
            public IReadOnlyDictionary<char, ushort> User { get; }
        }
    }
}
