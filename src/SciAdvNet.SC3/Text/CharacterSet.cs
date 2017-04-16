using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// Represents a character set of a particular SciADV game.
    /// </summary>
    public sealed class CharacterSet
    {
        private static readonly Dictionary<SC3Game, EncodingMaps> s_encodingMapCache;

        private readonly GameSpecificData _data;
        private readonly string _userChracters;

        private Dictionary<char, ushort> _mainEncodingMap;
        private Dictionary<string, ushort> _compoundCharsEncodingMap;
        private Dictionary<char, ushort> _userCharsEncodingMap;

        static CharacterSet()
        {
            s_encodingMapCache = new Dictionary<SC3Game, EncodingMaps>();
        }

        public CharacterSet(SC3Game game, string userCharacters)
        {
            Game = game;
            _data = GameSpecificData.For(game);
            _userChracters = userCharacters;

            EncodingMaps maps = null;
            if (s_encodingMapCache.TryGetValue(game, out maps))
            {
                _mainEncodingMap = maps.Main;
                _compoundCharsEncodingMap = maps.Compound;
            }
            else
            {
                ConstructEncodingMaps();
            }
        }

        public SC3Game Game { get; }

        private static int CodeToIndex(ushort code) => code & 0x7FFF;

        public ushort EncodeCharacter(char character)
        {
            ushort code;
            if (_mainEncodingMap.TryGetValue(character, out code))
            {
                return code;
            }
            if (_userCharsEncodingMap.TryGetValue(character, out code))
            {
                return code;
            }

            throw UnsupportedCharacter(character.ToString());
        }

        public ushort EncodeCompoundCharacter(string compoundCharacter)
        {
            KeyValuePair<int, string> tableEntry;
            try
            {
                tableEntry = _data.CompoundCharacters.Single(x => x.Value == compoundCharacter);
            }
            catch
            {
                throw UnsupportedCharacter(compoundCharacter);
            }

            int privateUseCodepoint = tableEntry.Key;
            return EncodeCharacter((char)privateUseCodepoint);
        }

        /// <summary>
        /// Decodes exactly one character.
        /// If the specified character code corresponds to a compound character, <see cref="char.MaxValue"/> is returned.
        /// </summary>
        /// <param name="code">The character code</param>
        /// <returns></returns>
        public char DecodeCharacter(ushort code)
        {
            int index = CodeToIndex(code);
            if (index >= _data.CharacterSet.Length)
            {
                throw InvalidCharacterCode(code);
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

            throw InvalidCharacterCode(code);
        }

        public string DecodeCompoundCharacter(ushort code)
        {
            int index = CodeToIndex(code);
            if (index >= _data.CharacterSet.Length)
            {
                throw InvalidCharacterCode(code);
            }

            int privateUseCodepoint = _data.CharacterSet[index];
            string compound;
            if (!_data.CompoundCharacters.TryGetValue(privateUseCodepoint, out compound))
            {
                throw InvalidCharacterCode(code);
            }

            return compound;
        }

        public bool Contains(char character)
        {
            return _data.CharacterSet.Contains(character);
        }

        private void ConstructEncodingMaps()
        {
            if (_mainEncodingMap == null)
            {
                _mainEncodingMap = new Dictionary<char, ushort>();
                for (int index = 0; index < _data.CharacterSet.Length; index++)
                {
                    byte highByte = (byte)(0x80 + (byte)(index / 256));
                    byte lowByte = (byte)(index % 256);

                    char character = _data.CharacterSet[index];
                    ushort code = (ushort)(highByte << 8 | lowByte);
                    _mainEncodingMap[character] = code;
                }
            }

            if (_compoundCharsEncodingMap == null)
            {
                _compoundCharsEncodingMap = new Dictionary<string, ushort>();
                foreach (var entry in _data.CompoundCharacters)
                {
                    string compoundChar = entry.Value;
                    int privateUseCodepoint = entry.Key;
                    ushort code = _mainEncodingMap[(char)privateUseCodepoint];
                    _compoundCharsEncodingMap[compoundChar] = code;
                }
            }

            if (_userCharsEncodingMap == null)
            {
                _userCharsEncodingMap = new Dictionary<char, ushort>();
                var reservedRange = _data.ReservedCharacterRange;
                for (int i = 0; i < _userChracters.Length; i++)
                {
                    char character = _userChracters[i];
                    int privateUseCodepoint = reservedRange.Start + i;
                    if (!_data.ReservedCharacterRange.ContainsValue(privateUseCodepoint))
                    {
                        throw new ArgumentException("Too many user specified characters.");
                    }

                    ushort code = _mainEncodingMap[(char)privateUseCodepoint];
                    _userCharsEncodingMap[character] = code;
                }
            }
        }

        private Exception UnsupportedCharacter(string character)
        {
            return new ArgumentException($"Unable to encode character '{character}': not present in the game's character set.");
        }

        private Exception InvalidCharacterCode(ushort characterCode)
        {
            return new ArgumentException($"Invalid character code: {characterCode:X2}.");
        }

        private sealed class EncodingMaps
        {
            public EncodingMaps(Dictionary<char, ushort> main, Dictionary<string, ushort> compound)
            {
                Main = main;
                Compound = compound;
            }

            public Dictionary<char, ushort> Main { get; }
            public Dictionary<string, ushort> Compound { get; }
        }
    }
}
