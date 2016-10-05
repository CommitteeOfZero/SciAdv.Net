using System.Linq;
using Xunit;

namespace SciAdvNet.NSScript.Tests
{
    public sealed class LexicalTests
    {
        [Fact]
        public void TestEmptyString()
        {
            var token = LexToken(string.Empty);
            Assert.Equal(SyntaxTokenKind.EndOfFileToken, token.Kind);
        }

        [Fact]
        public void TestStringLiteral()
        {
            string text = "\"literal\"";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.StringLiteralToken, token.Kind);
            Assert.Equal(text, token.Text);
            Assert.Equal("literal", token.Value);
        }

        [Fact]
        public void TestNumericLiteral()
        {
            string text = "42";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.NumericLiteralToken, token.Kind);
            Assert.Equal(42, token.Value);
        }

        [Fact]
        public void TestAtPrefixedNumericLiteral()
        {
            string text = "@42";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.NumericLiteralToken, token.Kind);
            Assert.Equal(42, token.Value);
        }

        [Fact]
        public void TestSingleLetterIdentifier()
        {
            string text = "a";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.IdentifierToken, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Fact]
        public void TestDollarPrefixedIdentifier()
        {
            string text = "$globalVar";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.IdentifierToken, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Fact]
        public void TestHashPrefixedIdentifier()
        {
            string text = "#flag";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.IdentifierToken, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Fact]
        public void TestArrowPrefixedIdentifier()
        {
            string text = "@->test";
            var token = LexToken(text);
            Assert.Equal(SyntaxTokenKind.IdentifierToken, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Fact]
        public void TestJapaneseIdentifier()
        {
            string identifier = "#ev100_06_1_６人祈る_a";
            var token = LexToken(identifier);

            Assert.Equal(SyntaxTokenKind.IdentifierToken, token.Kind);
            Assert.Equal(identifier, token.Text);
        }

        [Fact]
        public void TestIdentifierWithSlash()
        {
            string identifier = "nss/sys_load.nss";
            var token = LexToken(identifier);

            Assert.Equal(SyntaxTokenKind.IdentifierToken, token.Kind);
            Assert.Equal(identifier, token.Text);
        }

        [Fact]
        public void TestSingleLineComment()
        {
            string comment = "// this is a comment.";
            var token = LexToken(comment);
            Assert.Equal(SyntaxTokenKind.EndOfFileToken, token.Kind);
        }

        [Fact]
        public void TestMultiLineComment()
        {
            string comment = @"/*
				初回起動時ではないときは、プレイ速度をバックアップ
			*/";
            var token = LexToken(comment);
            Assert.Equal(SyntaxTokenKind.EndOfFileToken, token.Kind);
        }

        //[Fact]
        //public void TestXmlSyntaxMode()
        //{
        //    // 0. XmlElementStartTag (PRE)
        //    // 1. Xml_TextStartTag
        //    // 2. Xml_Text
        //    // 3. Xml_Text
        //    // 4. Xml_LineBreak
        //    // 5. XmlElementStartTag (voice)
        //    // 6. Xml_Text
        //    // 7. XmlElementStartTag (FONT)
        //    // 8. Xml_Text
        //    // 9. XmlElementEndTag (FONT)
        //    // 10. Xml_Text
        //    // 11. Xml_LineBreak
        //    // 12. XmlElementEndTag (PRE)

        //    string sourceText = TestScripts.Get("XmlSyntax.nss");
        //    var tokens = NSScript.ParseTokens(sourceText).Where(x => x.IsXmlToken).ToList();
        //    Assert.Equal(13, tokens.Count);

        //    Assert.Equal(SyntaxTokenKind.XmlElementStartTag, tokens[0].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_TextStartTag, tokens[1].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_Text, tokens[2].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_Text, tokens[3].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_LineBreak, tokens[4].Kind);
        //    Assert.Equal(SyntaxTokenKind.XmlElementStartTag, tokens[5].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_Text, tokens[6].Kind);
        //    Assert.Equal(SyntaxTokenKind.XmlElementStartTag, tokens[7].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_Text, tokens[8].Kind);
        //    Assert.Equal(SyntaxTokenKind.XmlElementEndTag, tokens[9].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_Text, tokens[10].Kind);
        //    Assert.Equal(SyntaxTokenKind.Xml_LineBreak, tokens[11].Kind);
        //    Assert.Equal(SyntaxTokenKind.XmlElementEndTag, tokens[12].Kind);
        //}

        [Fact]
        public void TestIncludeDirective()
        {
            string text = "#include \"test.nss\"";
            var token = LexToken(text);

            Assert.Equal(SyntaxTokenKind.IncludeDirective, token.Kind);
            Assert.Equal(text, token.Text);
        }

        private SyntaxToken LexToken(string text)
        {
            SyntaxToken result = null;
            foreach (var token in NSScript.ParseTokens(text))
            {
                if (result == null)
                {
                    result = token;
                }
                else if (token.Kind != SyntaxTokenKind.EndOfFileToken)
                {
                    Assert.True(false, "More than one token was lexed.");
                }
            }

            if (result == null)
            {
                Assert.True(false, "No tokens were lexed.");
            }

            return result;
        }
    }
}
