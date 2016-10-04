using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace SciAdvNet.NSScript
{
    internal sealed partial class Parser
    {
        public DialogueBlock ParseDialogueBlock()
        {
            string boxName = ExtractBoxName();
            EatToken(SyntaxTokenKind.XmlElementStartTag);
            var textStartTag = EatToken(SyntaxTokenKind.Xml_TextStartTag);
            string blockIdentifier = textStartTag.Text.Substring(1, textStartTag.Text.Length - 2);

            var parts = ImmutableArray.CreateBuilder<IDialogueBlockPart>();
            while (CurrentToken.Text != "</PRE>")
            {
                if (CurrentToken.Kind == SyntaxTokenKind.Xml_LineBreak)
                {
                    EatToken();
                    continue;
                }

                parts.Add(ParsePart());
            }

            EatToken(SyntaxTokenKind.XmlElementEndTag);
            return StatementFactory.DialogueBlock(blockIdentifier, boxName, parts.ToImmutable());
        }

        private IDialogueBlockPart ParsePart()
        {
            switch (CurrentToken.Kind)
            {
                case SyntaxTokenKind.Xml_Text:
                    return ParseDialogueLine();

                case SyntaxTokenKind.XmlElementStartTag:
                    if (ExtractTagName() == "voice")
                    {
                        return ParseVoiceTag();
                    }
                    else
                    {
                        return ParseDialogueLine();
                    }

                case SyntaxTokenKind.OpenBraceToken:
                    return ParseBlock();

                default:
                    throw ExceptionUtilities.UnexpectedToken(FileName, CurrentToken.Text);
            }
        }

        private DialogueLine ParseDialogueLine()
        {
            var segments = ImmutableArray.CreateBuilder<TextSegment>();
            while (CurrentToken.Kind != SyntaxTokenKind.Xml_LineBreak && CurrentToken.Kind != SyntaxTokenKind.OpenBraceToken)
            {
                var tk = CurrentToken.Kind;
                if (tk == SyntaxTokenKind.XmlElementStartTag || tk == SyntaxTokenKind.XmlElementEndTag)
                {
                    string tagName = ExtractTagName();
                    if (tagName == "PRE")
                    {
                        break;
                    }
                    else if (tagName == "pre")
                    {
                        EatToken();
                        continue;
                    }
                }

                segments.Add(ParseTextSegment());
            }

            if (CurrentToken.Kind == SyntaxTokenKind.Xml_LineBreak)
            {
                EatToken();
            }

            return StatementFactory.DialogueLine(segments.ToImmutable());
        }

        private TextSegment ParseTextSegment()
        {
            switch (CurrentToken.Kind)
            {
                case SyntaxTokenKind.Xml_Text:
                    var s = StatementFactory.TextSegment(CurrentToken.Text, 0x00000000);
                    EatToken();
                    return s;

                case SyntaxTokenKind.XmlElementStartTag:
                    if (ExtractTagName() == "FONT")
                    {
                        return ParseFontTag();
                    }
                    else
                    {
                        s = StatementFactory.TextSegment(CurrentToken.Text, 0x00000000);
                        var tag = EatToken(SyntaxTokenKind.XmlElementStartTag);
                        return s;
                    }

                default:
                    throw new Exception();
            }
        }

        private string ExtractBoxName()
        {
            var tag = CurrentToken;
            int idxStart = 5;
            int idxEnd = tag.Text.Length - 1;

            return tag.Text.Substring(idxStart, idxEnd - idxStart);
        }

        private Voice ParseVoiceTag()
        {
            var tag = ParseXmlTag();
            string fileName = tag.Attributes["src"];
            string character = tag.Attributes["name"];
            return StatementFactory.Voice(character, fileName);
        }

        private TextSegment ParseFontTag()
        {
            var tag = ParseXmlTag();

            string text = EatToken(SyntaxTokenKind.Xml_Text).Text;
            string strColor = tag.Attributes["incolor"].Substring(1);
            int colorCode = int.Parse(strColor, NumberStyles.HexNumber);

            EatToken(SyntaxTokenKind.XmlElementEndTag);
            return StatementFactory.TextSegment(text, colorCode);
        }

        private sealed class PseudoXmlTag
        {
            public PseudoXmlTag(string name, ImmutableDictionary<string, string> attributes)
            {
                Name = name;
                Attributes = attributes;
            }

            public string Name { get; }
            public ImmutableDictionary<string, string> Attributes { get; }
        }

        private string ExtractTagName()
        {
            string text = CurrentToken.Text;
            int idxNameStart = 0;
            while (text[idxNameStart] == '<' || text[idxNameStart] == '/' && idxNameStart < text.Length)
            {
                idxNameStart++;
            }

            int idxNameEnd = text.IndexOf(' ', idxNameStart);
            if (idxNameEnd == -1)
            {
                idxNameEnd = text.IndexOf('>', idxNameStart);
            }

            return text.Substring(idxNameStart, idxNameEnd - idxNameStart);
        }

        private PseudoXmlTag ParseXmlTag()
        {
            string text = CurrentToken.Text;
            string name = ExtractTagName();
            int position = name.Length + 2;

            var attributes = ImmutableDictionary.CreateBuilder<string, string>();
            while (text[position] != '>')
            {
                if (text[position] == ' ')
                {
                    position++;
                }
                else
                {
                    attributes.Add(ParseXmlAttribute(ref position));
                }
            }

            EatToken();
            return new PseudoXmlTag(name, attributes.ToImmutable());
        }

        private KeyValuePair<string, string> ParseXmlAttribute(ref int position)
        {
            string text = CurrentToken.Text;
            int idxKeyStart = position;
            int idxKeyEnd = text.IndexOf('=', idxKeyStart);
            string key = text.Substring(idxKeyStart, idxKeyEnd - idxKeyStart);

            string value = string.Empty;
            int idxValueStart = idxKeyEnd + 2;
            int idxValueEnd = text.IndexOf('"', idxValueStart);
            value = text.Substring(idxValueStart, idxValueEnd - idxValueStart);

            position = idxValueEnd + 1;
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
